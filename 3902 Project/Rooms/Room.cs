using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Rooms
{
    // One screen of the dungeon: its walls and floor, and the blocks, doors, items, NPCs and enemies in it.
    // Only the room Link is in gets updated and drawn
    internal sealed class Room
    {
        private readonly ISprite walls = BlockSpriteFactory.Instance.CreateRoomWallsSprite();
        private readonly ISprite floor = BlockSpriteFactory.Instance.CreateRoomFloorSprite();

        // False for rooms like the underground passage, whose walls are blocks listed in the room's file
        private bool hasDungeonWalls = true;

        private readonly List<IBlock> blocks = [];
        private readonly List<IItem> items = [];
        private readonly List<INpc> npcs = [];
        private readonly List<IEnemy> enemies = [];

        public IReadOnlyList<IBlock> Blocks => blocks;
        public IReadOnlyList<IItem> Items => items;
        public IReadOnlyList<INpc> Npcs => npcs;
        public IReadOnlyList<IEnemy> Enemies => enemies;

        // The solid parts of the room's walls, which nothing can walk or fly into, and the strips just
        // outside the room. A room without the usual walls has only the strips; its blocks are its walls
        public IReadOnlyList<Rectangle> Walls => hasDungeonWalls ? RoomLayout.WallsAndOutside : RoomLayout.Outside;

        // Doors are blocks too: each one fills the gap in one of the walls
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
    }
}
