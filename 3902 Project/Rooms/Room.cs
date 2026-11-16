using System.Collections.Generic;
using System.Linq;
using Audio;
using Enemies;
using Interfaces;
using Items;
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

        // The bursts left where enemies have just died
        private readonly List<DeathPuff> deathPuffs = [];

        // Items that appear once the room is cleared, like most of the keys in the NES (see IsCleared)
        private readonly List<IItem> rewards = [];

        // Items enemies are carrying, like the Stalfos with a key in the NES: drawn on the enemy, and dropped where
        // it dies
        private readonly Dictionary<IEnemy, IItem> carriedItems = [];

        // As in the NES, only one enemy hiding in the walls (a Wallmaster) is out at a time
        private const int HidingEnemiesOutAtOnce = 1;

        // Where the room is on the dungeon map in the HUD (column and row, counted from 0), or null for
        // a room that isn't on it, like the test room and the underground passage
        public Point? MapPosition { get; private set; }

        // True for a room seen from the side, like the underground passage (see SideView)
        public bool IsSideView { get; private set; }

        public void ViewFromTheSide()
        {
            IsSideView = true;
        }

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

        // A cracked wall becomes a doorway once it has been blown open
        public bool HasDoorway(Direction side)
        {
            return doorways.Contains(side) || doors.GetValueOrDefault(side) is IBombableWall { IsOpen: true };
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

        public void AddReward(IItem item)
        {
            rewards.Add(item);
        }

        // enemy carries item until it dies
        public void GiveItemTo(IEnemy enemy, IItem item)
        {
            carriedItems[enemy] = item;
        }

        // True once every enemy that can be killed is dead (Blade Traps can't be, so they don't count)
        public bool IsCleared => !enemies.Any(enemy => enemy is IDamageable);

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

        // Enemies that have died disappear from the room for good, leaving a burst of light where they were, and
        // whatever they were carrying, or sometimes an item (see ItemDrops); a Zol that split leaves its two Gels instead
        public void RemoveDeadEnemies()
        {
            List<IEnemy> dead = enemies.FindAll(enemy => enemy is IDamageable { IsDead: true });

            foreach (IEnemy enemy in dead)
            {
                enemies.Remove(enemy);

                if (enemy is ISplittingEnemy { Pieces.Count: > 0 } splitEnemy)
                {
                    enemies.AddRange(splitEnemy.Pieces);
                    continue;
                }

                Vector2 middle = enemy.Collider.GetCollider().Center.ToVector2();
                IItem drop = carriedItems.Remove(enemy, out IItem carried) ? carried : ItemDrops.Roll(middle);
                drop?.CenterOn(middle);
                deathPuffs.Add(new DeathPuff(middle, drop));
            }
        }

        // Once a burst has gone, whatever the enemy dropped is left lying there
        private void UpdateDeathPuffs(GameTime gameTime)
        {
            foreach (DeathPuff deathPuff in deathPuffs)
            {
                deathPuff.Update(gameTime);

                if (deathPuff.IsFinished && deathPuff.Drop != null)
                    items.Add(deathPuff.Drop);
            }

            deathPuffs.RemoveAll(deathPuff => deathPuff.IsFinished);
        }

        // Lets everything that reacts to Link (like the Blade Traps) see where he is
        public void ShowLinkToWatchers(Rectangle link)
        {
            foreach (IEnemy enemy in enemies)
            {
                if (enemy is ILinkWatcher watcher)
                    watcher.Watch(link);
            }

            foreach (IBlock block in blocks)
            {
                if (block is ILinkWatcher watcher)
                    watcher.Watch(link);
            }
        }

        public void Update(GameTime gameTime)
        {
            LetHidingEnemiesOut();

            foreach (IBlock block in blocks)
                block.Update(gameTime);

            foreach (IItem item in items)
                item.Update(gameTime);

            foreach (INpc npc in npcs)
                npc.Update(gameTime);

            foreach (IEnemy enemy in enemies)
                enemy.Update(gameTime);

            foreach ((IEnemy enemy, IItem item) in carriedItems)
            {
                if (enemy.Collider != null)
                    item.CenterOn(enemy.Collider.GetCollider().Center.ToVector2());
            }

            UpdateDeathPuffs(gameTime);

            if (rewards.Count > 0 && IsCleared && deathPuffs.Count == 0)
                GiveRewards();

            if (ShuttersShouldOpen)
                OpenShutters();
        }

        // Once the last enemy's burst has gone, the room's rewards appear, with the NES's chime
        private void GiveRewards()
        {
            items.AddRange(rewards);
            rewards.Clear();
            AudioManager.Instance.Play(Sound.KeyAppear);
        }

        // Lets enemies hiding in the walls come out when they are ready, as long as not too many are out already
        private void LetHidingEnemiesOut()
        {
            int outNow = enemies.Count(enemy => enemy is IHidingEnemy { IsHidden: false });

            foreach (IEnemy enemy in enemies)
            {
                if (outNow >= HidingEnemiesOutAtOnce)
                    return;

                if (enemy is IHidingEnemy { IsReadyToComeOut: true } hiding)
                {
                    hiding.ComeOut();
                    outNow++;
                }
            }
        }

        // As in the NES, a room with a block to push opens its shutters once the block has been pushed; any other
        // room opens them once it is cleared
        private bool ShuttersShouldOpen =>
            blocks.Find(block => block is IPushableBlock) is IPushableBlock pushable ? pushable.HasMoved : IsCleared;

        // As in the NES, a shutter Link comes in by opens to let him through, and shuts behind him once he is in
        // (see RoomTransitionState), unless the room is ready to open it anyway. True if the shutter on side opened
        public bool OpenShutterFor(Direction side)
        {
            if (DoorOn(side) is not IShutterDoor { IsOpen: false } shutter)
                return false;

            shutter.Open();
            return true;
        }

        public void ShutBehindLink(Direction side)
        {
            if (ShuttersShouldOpen || DoorOn(side) is not IShutterDoor { IsOpen: true } shutter)
                return;

            shutter.Close();
            AudioManager.Instance.Play(Sound.DoorUnlock);
        }

        // Opens the shutter doors, with the door sound
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

        // Back to front: walls and floor, blocks and doors, items, NPCs, enemies and what they carry, then the bursts
        // where enemies died
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

            foreach (IItem item in carriedItems.Values)
                item.Draw(spriteBatch);

            foreach (DeathPuff deathPuff in deathPuffs)
                deathPuff.Draw(spriteBatch);
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
