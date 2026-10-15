using Enemies;
using Environment;
using Items;
using Player;
using Projectiles;

namespace Rooms
{
    // The second room: stairs in the middle with a gap on each side, square blocks in the
    // corners, a strip of sand along the top, and a few enemies and items to find
    internal static class SecondRoom
    {
        public static Room Create(ProjectileManager projectiles)
        {
            Room room = new();

            // Down is open so Link can leave the way he came in
            room.AddBlock(new OpenDoor(Direction.Down));
            room.AddBlock(new OpenDoor(Direction.Up));
            room.AddBlock(new BombedWallOpening(Direction.Left));
            room.AddBlock(new KeyholeLockedDoor(Direction.Right));

            // Square blocks near each corner of the floor
            room.AddBlock(new SquareBlock(RoomLayout.TilePosition(2, 1)));
            room.AddBlock(new SquareBlock(RoomLayout.TilePosition(9, 1)));
            room.AddBlock(new SquareBlock(RoomLayout.TilePosition(2, 5)));
            room.AddBlock(new SquareBlock(RoomLayout.TilePosition(9, 5)));

            // A strip of sand along the top
            for (int column = 4; column <= 7; column++)
                room.AddBlock(new SandTile(RoomLayout.TilePosition(column, 1)));

            // Stairs in the middle of the floor with a gap on either side
            room.AddBlock(new BlueGap(RoomLayout.TilePosition(4.5f, 3)));
            room.AddBlock(new Stairs(RoomLayout.TilePosition(5.5f, 3)));
            room.AddBlock(new BlueGap(RoomLayout.TilePosition(6.5f, 3)));

            // Enemies on either side of the stairs
            room.AddCharacter(new Goriya(RoomLayout.TilePosition(1, 3), projectiles));
            room.AddCharacter(new Keese(RoomLayout.TilePosition(3, 3)));
            room.AddCharacter(new Zol(RoomLayout.TilePosition(8, 3)));

            // Something to pick up
            room.AddItem(new Rupee(RoomLayout.TilePosition(5.5f, 5)));
            room.AddItem(new Heart(RoomLayout.TilePosition(11, 3)));

            return room;
        }
    }
}