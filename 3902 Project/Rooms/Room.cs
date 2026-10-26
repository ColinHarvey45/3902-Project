using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using Player;

namespace Rooms
{
    // One screen of the dungeon: its walls and floor, and the blocks, doors, items and characters in it.
    // Only the room Link is in gets updated and drawn
    internal sealed class Room
    {
        private readonly ISprite walls = BlockSpriteFactory.Instance.CreateRoomWallsSprite();
        private readonly ISprite floor = BlockSpriteFactory.Instance.CreateRoomFloorSprite();

        // False for rooms like the underground passage, whose walls are blocks listed in the room's file
        private bool hasDungeonWalls = true;

        private readonly List<IBlock> blocks = [];
        public IReadOnlyList<IBlock> Blocks => blocks;
        private readonly List<IItem> items = [];
        public List<ICharacter> characters = [];

        // Doors are blocks too: each one fills the gap in one of the walls
        public void AddBlock(IBlock block)
        {
            blocks.Add(block);
        }

        public void AddItem(IItem item)
        {
            items.Add(item);
        }

        public void AddCharacter(ICharacter character)
        {
            characters.Add(character);
        }

        // Leaves out the usual teal walls and floor, so the room is just black behind its own blocks
        public void RemoveDungeonWalls()
        {
            hasDungeonWalls = false;
        }

        // Enemies that have died disappear from the room for good
        public void RemoveDeadCharacters()
        {
            characters.RemoveAll(character => character is IDamageable damageable && damageable.IsDead);
        }

        public void Update(GameTime gameTime)
        {
            foreach (IBlock block in blocks)
                block.Update(gameTime);

            foreach (IItem item in items)
                item.Update(gameTime);

            foreach (ICharacter character in characters)
                character.Update(gameTime);
        }

        // Back to front: walls and floor, blocks and doors, items, then characters
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

            foreach (ICharacter character in characters)
                character.Draw(spriteBatch);
        }
    }
}
