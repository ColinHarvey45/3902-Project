using System.Collections.Generic;
using Audio;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Sprites;

namespace Rooms
{
    // One screen of the dungeon: its walls and floor, and the blocks, doors, items, NPCs and enemies in it.
    // Only the room Link is in gets updated and drawn. name is the room's file name, e.g. Room01_Entrance
    internal sealed class Room(string name)
    {
        private readonly ISprite walls = BlockSpriteFactory.Instance.CreateRoomWallsSprite();
        private readonly ISprite floor = BlockSpriteFactory.Instance.CreateRoomFloorSprite();

        // False for rooms like the underground passage, whose walls are blocks listed in the room's file
        private bool hasDungeonWalls = true;

        // The dungeon map in the HUD is this many rooms across and down
        public const int MapColumns = 8;
        public const int MapRows = 8;

        private readonly List<IBlock> blocks = [];
        private readonly List<IItem> items = [];
        private readonly List<INpc> npcs = [];
        private readonly List<IEnemy> enemies = [];

        // Where the room is on the dungeon map in the HUD (column and row, counted from 0), or null for
        // a room that isn't on it, like the test room and the underground passage
        public Point? MapPosition { get; private set; }

        // True once Link has been in the room, which is when it appears on the pause screen's map
        public bool Visited { get; private set; }

        // The door (or plain wall) filling the gap in each wall, and the sides that have a doorway (a door,
        // locked or not, or an opening) rather than a plain wall
        private readonly Dictionary<Direction, IBlock> doors = [];
        private readonly HashSet<Direction> doorways = [];

        private readonly List<Warp> warps = [];

        public string Name { get; } = name;

        public IReadOnlyList<IBlock> Blocks => blocks;
        public IReadOnlyList<IItem> Items => items;
        public IReadOnlyList<INpc> Npcs => npcs;
        public IReadOnlyList<IEnemy> Enemies => enemies;
        public IReadOnlyList<Warp> Warps => warps;

        // The solid parts of the room's walls, which nothing can walk or fly into, and the strips just
        // outside the room. A room without the usual walls has only the strips; its blocks are its walls
        public IReadOnlyList<Rectangle> Walls => hasDungeonWalls ? RoomLayout.WallsAndOutside : RoomLayout.Outside;

        public void SetMapPosition(int column, int row)
        {
            MapPosition = new Point(column, row);
        }

        public void MarkVisited()
        {
            Visited = true;
        }

        // Doors are blocks too: each one fills the gap in one of the walls. isDoorway is false for a plain wall
        public void AddDoor(Direction side, IBlock door, bool isDoorway)
        {
            blocks.Add(door);
            doors[side] = door;

            if (isDoorway)
                doorways.Add(side);
        }

        // The door (or plain wall) in the given wall, or null if the room has none there, like the underground passage
        public IBlock DoorOn(Direction side)
        {
            return doors.GetValueOrDefault(side);
        }

        public bool HasDoorway(Direction side)
        {
            return doorways.Contains(side);
        }

        public void AddBlock(IBlock block)
        {
            blocks.Add(block);
        }

        public void AddItem(IItem item)
        {
            items.Add(item);
        }

        public void AddNpc(INpc npc)
        {
            npcs.Add(npc);
        }

        public void AddEnemy(IEnemy enemy)
        {
            enemies.Add(enemy);
        }

        public void AddWarp(Warp warp)
        {
            warps.Add(warp);
        }

        // Leaves out the usual teal walls and floor, so the room is just black behind its own blocks
        public void RemoveDungeonWalls()
        {
            hasDungeonWalls = false;
        }

        // An item Link has picked up is gone from the room for good
        public void RemoveItem(IItem item)
        {
            items.Remove(item);
        }

        // Enemies that have died disappear from the room for good
        public void RemoveDeadEnemies()
        {
            enemies.RemoveAll(enemy => enemy is IDamageable damageable && damageable.IsDead);
        }

        public void Update(GameTime gameTime)
        {
            foreach (IBlock block in blocks)
                block.Update(gameTime);

            foreach (IItem item in items)
                item.Update(gameTime);

            foreach (INpc npc in npcs)
                npc.Update(gameTime);

            foreach (IEnemy enemy in enemies)
                enemy.Update(gameTime);

            if (enemies.Count == 0)
                OpenShutters();
        }

        // Shutter doors open once every enemy in the room is gone, with the door sound
        private void OpenShutters()
        {
            bool opened = false;
            foreach (IBlock door in doors.Values)
            {
                if (door is IShutterDoor { IsOpen: false } shutter)
                {
                    shutter.Open();
                    opened = true;
                }
            }

            if (opened)
                AudioManager.Instance.Play(Sound.DoorUnlock);
        }

        // Back to front: walls and floor, blocks and doors, items, NPCs, then enemies
        public void Draw(SpriteBatch spriteBatch)
        {
            if (hasDungeonWalls)
            {
                walls.Draw(spriteBatch, RoomLayout.Origin);
                floor.Draw(spriteBatch, RoomLayout.FloorOrigin);
            }

            foreach (IBlock block in blocks)
                block.Draw(spriteBatch);

            foreach (IItem item in items)
                item.Draw(spriteBatch);

            foreach (INpc npc in npcs)
                npc.Draw(spriteBatch);

            foreach (IEnemy enemy in enemies)
                enemy.Draw(spriteBatch);
        }

        // The arches over the doorways, drawn after Link and his weapons so they go under them (see IDoor)
        public void DrawArches(SpriteBatch spriteBatch)
        {
            foreach (IBlock door in doors.Values)
            {
                if (door is IDoor withArch)
                    withArch.DrawArch(spriteBatch);
            }
        }
    }
}
