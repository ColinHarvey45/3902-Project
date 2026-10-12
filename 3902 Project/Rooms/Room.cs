using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Rooms
{
    // One screen of the dungeon: its walls and floor, and the blocks, doors, items and characters in it.
    // Only the room Link is in gets updated and drawn
    internal sealed class Room
    {
        private readonly ISprite walls = BlockSpriteFactory.Instance.CreateRoomWallsSprite();
        private readonly ISprite floor = BlockSpriteFactory.Instance.CreateRoomFloorSprite();

        private readonly List<IBlock> blocks = [];
        private readonly List<IItem> items = [];
        private readonly List<ICharacter> characters = [];

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
            walls.Draw(spriteBatch, RoomLayout.Origin);
            floor.Draw(spriteBatch, RoomLayout.FloorOrigin);

            foreach (IBlock block in blocks)
                block.Draw(spriteBatch);

            foreach (IItem item in items)
                item.Draw(spriteBatch);

            foreach (ICharacter character in characters)
                character.Draw(spriteBatch);
        }
    }
}
