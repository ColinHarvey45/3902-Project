using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Audio;
using Collisions;
using CSE_3902_Project;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Projectiles;

namespace Rooms
{
    // Holds every room in the dungeon and knows which one Link is in (and which he has been in). Link goes wherever the
    // current room is, so he is updated and drawn along with it, as is everything fired in it.
    // Rooms are next to each other as they are on the dungeon map: walking out through a doorway leads to the room
    // beside it on that side, and the stairs (warps) lead to rooms that aren't on the map
    internal sealed class RoomManager
    {
        private readonly IPlayer link;
        private readonly CyclingList<Room> rooms;
        private readonly ProjectileManager projectiles;

        // The rooms on the dungeon map, by their place on it
        private readonly Dictionary<Point, Room> roomsOnMap = [];

        // Where Link starts, and where a Wallmaster takes him back to
        private readonly Room entrance;

        // The stairs (or ladder top) Link is standing on, if any
        private Warp warpLinkIsOn;

        public Room CurrentRoom => rooms.Current;

        // Every room in the dungeon, which the HUD draws on its map
        public IReadOnlyList<Room> AllRooms { get; }

        // The wall whose doorway Link walked out through during the last update, into the room beside it, or null.
        // The room doesn't change yet: the screen scrolls to the next room first (see EnterRoom)
        public Direction? DoorwayLinkLeftBy { get; private set; }

        // Link starts in the room called startRoom (its file name). projectiles is shared with Link and the
        // enemies, who fire into it
        public RoomManager(IPlayer link, Room[] rooms, ProjectileManager projectiles, string startRoom)
        {
            this.link = link;
            AllRooms = rooms;
            this.rooms = new CyclingList<Room>(rooms);
            this.projectiles = projectiles;

            foreach (Room room in rooms)
            {
                if (room.MapPosition is Point cell && !roomsOnMap.TryAdd(cell, room))
                    throw new InvalidDataException($"{room.Name} and {roomsOnMap[cell].Name} are in the same place on the map");
            }

            CheckWarpsLeadSomewhere();

            entrance = FindRoom(startRoom);
            this.rooms.MoveTo(entrance);
            CurrentRoom.MarkVisited();
        }

        // The mouse and the O and P keys go through every room in file order, test room included
        public void NextRoom()
        {
            LeaveCurrentRoom();
            rooms.Next();
            OnRoomChanged();
        }

        public void PreviousRoom()
        {
            LeaveCurrentRoom();
            rooms.Previous();
            OnRoomChanged();
        }

        // The room beside room on the given side on the dungeon map, or null if there isn't one (or room isn't on the map)
        public Room NeighbourOf(Room room, Direction side)
        {
            if (room.MapPosition is not Point cell)
                return null;

            return roomsOnMap.GetValueOrDefault((cell.ToVector2() + side.ToVector()).ToPoint());
        }

        // Makes room the one Link is in, once he has got there through a doorway or down the stairs
        public void EnterRoom(Room room)
        {
            LeaveCurrentRoom();
            rooms.MoveTo(room);
            OnRoomChanged();
        }

        // Whatever had hold of Link in the room he is leaving lets go, so it can't carry him off when he comes back
        private void LeaveCurrentRoom()
        {
            foreach (IEnemy enemy in CurrentRoom.Enemies)
            {
                if (enemy is ILinkGrabber grabber)
                    grabber.Release();
            }
        }

        // Everything moves first, then whatever ended up touching is sorted out
        public void Update(GameTime gameTime)
        {
            Vector2 linkBefore = link.Position;
            link.Update(gameTime);
            if (CurrentRoom.IsSideView)
                SideView.Apply(link, CurrentRoom, linkBefore);

            CurrentRoom.ShowLinkToWatchers(link.Collider.GetCollider());
            CurrentRoom.Update(gameTime);
            projectiles.Update(gameTime);

            // Walking out through a doorway with a room beyond it: the scroll to that room takes over from here
            DoorwayLinkLeftBy = RoomLayout.EdgeCrossed(link.Collider.GetCollider()) is Direction side
                && NeighbourOf(CurrentRoom, side) != null ? side : null;
            if (DoorwayLinkLeftBy != null)
                return;

            CollisionManager.HandleCollisions(link, CurrentRoom, projectiles);
            projectiles.RemoveFinished();

            OpenDoorsOnTheOtherSide();
            TakeWarp();
            ReturnLinkIfCarriedOff();
        }

        // A Wallmaster that has carried Link off takes him back to the entrance of the dungeon, as in the NES
        private void ReturnLinkIfCarriedOff()
        {
            if (!CurrentRoom.Enemies.Any(enemy => enemy is ILinkGrabber { HasCarriedLinkOff: true }))
                return;

            EnterRoom(entrance);
            link.PlaceAt(RoomLayout.EntryPosition(Direction.Down));
        }

        // Back to front: the room, then Link, then anything flying over them, then the doorways' arches, which
        // Link and his weapons go under
        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentRoom.Draw(spriteBatch);
            link.Draw(spriteBatch);
            projectiles.Draw(spriteBatch);
            CurrentRoom.DrawArches(spriteBatch);
        }

        // Nothing fired in the old room follows Link into the new one
        private void OnRoomChanged()
        {
            projectiles.Clear();
            CurrentRoom.MarkVisited();
        }

        // A door unlocked (or a wall blown open) from one side is open from the other too: the room next door has
        // the same door
        private void OpenDoorsOnTheOtherSide()
        {
            foreach (Direction side in Enum.GetValues<Direction>())
            {
                IBlock otherSide = NeighbourOf(CurrentRoom, side)?.DoorOn(side.Opposite());

                if (CurrentRoom.DoorOn(side) is ILockedDoor { IsLocked: false } && otherSide is ILockedDoor { IsLocked: true } lockedDoor)
                    lockedDoor.Unlock();

                if (CurrentRoom.DoorOn(side) is IBombableWall { IsOpen: true } && otherSide is IBombableWall { IsOpen: false } crackedWall)
                    crackedWall.BlowOpen();
            }
        }

        // Stepping onto the stairs (Link's middle on them, not just his edge) takes him straight to the room they lead to.
        // Only stepping onto them does: Link who has just come up a ladder onto the stairs, as in the NES, has to walk
        // off them and back on to go down again
        private void TakeWarp()
        {
            Warp warp = WarpUnderLink();
            bool steppedOn = warp != null && warp != warpLinkIsOn;
            warpLinkIsOn = warp;
            if (!steppedOn)
                return;

            EnterRoom(FindRoom(warp.Destination));
            link.PlaceAt(warp.Arrival);
            warpLinkIsOn = WarpUnderLink();
            AudioManager.Instance.Play(Sound.Stairs);
        }

        private Warp WarpUnderLink()
        {
            Point middle = link.Collider.GetCollider().Center;
            return CurrentRoom.Warps.FirstOrDefault(each => each.Area.Contains(middle));
        }

        private Room FindRoom(string name)
        {
            return AllRooms.FirstOrDefault(room => room.Name == name)
                ?? throw new InvalidDataException($"There is no room called {name}");
        }

        // A warp to a room that doesn't exist stops the game when it loads rather than when Link steps on it
        private void CheckWarpsLeadSomewhere()
        {
            foreach (Room room in AllRooms)
            {
                foreach (Warp warp in room.Warps)
                {
                    if (!AllRooms.Any(other => other.Name == warp.Destination))
                        throw new InvalidDataException($"{room.Name} has a Warp to {warp.Destination}, but there is no room file called that");
                }
            }
        }
    }
}
