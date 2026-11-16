using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Rooms;
using Sprites;

namespace Environment
{
    // A door piece in the gap in one of the room's walls (side). Its arch is drawn again over Link (see IDoor)
    internal abstract class Door(Direction side, DoorSprites look)
        : Block(RoomLayout.DoorPosition(side), look.Whole, RoomLayout.DoorSize), IDoor
    {
        private readonly Vector2 position = RoomLayout.DoorPosition(side);
        private ISprite arch = look.Arch;

        protected Direction Side { get; } = side;

        public void DrawArch(SpriteBatch spriteBatch)
        {
            arch.Draw(spriteBatch, position);
        }

        // A door that opens changes how it looks
        protected void ChangeLook(DoorSprites newLook)
        {
            Sprite = newLook.Whole;
            arch = newLook.Arch;
        }
    }
}
