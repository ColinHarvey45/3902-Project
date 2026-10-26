using Microsoft.Xna.Framework;
using Player;
using Sprites;

namespace Rooms
{
    // Where things go on screen. Every dungeon room has the same shape: a 16 x 11 tile room whose
    // outer two tiles are wall, leaving a floor of 12 x 7 tiles, with a door gap in the middle of each wall
    internal static class RoomLayout
    {
        public const int TileSize = 16 * (int)Sprite.Scale;
        public const int FloorColumns = 12;
        public const int FloorRows = 7;

        // The whole room, walls included
        public const int RoomColumns = FloorColumns + 4;
        public const int RoomRows = FloorRows + 4;

        private const int WallThickness = 2 * TileSize;
        private const int Width = FloorColumns * TileSize + 2 * WallThickness;
        private const int Height = FloorRows * TileSize + 2 * WallThickness;

        // The room's top-left corner on screen. It is the window's corner for now; when the HUD is
        // added above the room, moving this moves everything in the room with it
        public static readonly Vector2 Origin = Vector2.Zero;

        public static Vector2 FloorOrigin => Origin + new Vector2(WallThickness);

        // The whole room, walls included
        public static Rectangle Bounds => new((int)Origin.X, (int)Origin.Y, Width, Height);

        // Just the floor, which is where enemies and the fairy wander
        public static Rectangle FloorBounds =>
            new((int)FloorOrigin.X, (int)FloorOrigin.Y, FloorColumns * TileSize, FloorRows * TileSize);

        // The top-left corner of a floor tile; columns run 0-11 from the left, rows 0-6 from the top.
        // Halves are allowed, e.g. column 5.5 is the middle of the floor
        public static Vector2 TilePosition(float column, float row)
        {
            return FloorOrigin + new Vector2(column, row) * TileSize;
        }

        // The top-left corner of a tile counted from the room's own corner, walls included: columns run
        // 0-15 and rows 0-10. Used by rooms without the usual walls, like the underground passage
        public static Vector2 RoomTilePosition(float column, float row)
        {
            return Origin + new Vector2(column, row) * TileSize;
        }

        // The top-left corner of the gap in the given wall, where a door (or a wall piece filling it) goes
        public static Vector2 DoorPosition(Direction side)
        {
            float middleX = (Width - WallThickness) / 2f;
            float middleY = (Height - WallThickness) / 2f;

            Vector2 offset = side switch
            {
                Direction.Up => new Vector2(middleX, 0),
                Direction.Down => new Vector2(middleX, Height - WallThickness),
                Direction.Left => new Vector2(0, middleY),
                _ => new Vector2(Width - WallThickness, middleY)
            };

            return Origin + offset;
        }
    }
}
