using Enemies;
using Environment;
using Items;
using Npcs;
using Player;

namespace Rooms
{
    // A room with one of every kind of object in the first dungeon, for trying things out.
    // It is built here in code until rooms are loaded from files
    internal static class TestRoom
    {
        public static Room Create()
        {
            Room room = new();

            // Link comes in through the open door at the bottom
            room.AddBlock(new OpenDoor(Direction.Down));
            room.AddBlock(new KeyholeLockedDoor(Direction.Up));
            room.AddBlock(new BombedWallOpening(Direction.Left));
            room.AddBlock(new DiamondLockedDoor(Direction.Right));

            // Top row: every kind of block, then the Old Man between two fires
            room.AddBlock(new SquareBlock(RoomLayout.TilePosition(0, 0)));
            room.AddBlock(new FishStatue(RoomLayout.TilePosition(1, 0)));
            room.AddBlock(new DragonStatue(RoomLayout.TilePosition(2, 0)));
            room.AddBlock(new BlueGap(RoomLayout.TilePosition(3, 0)));
            room.AddBlock(new BlackTile(RoomLayout.TilePosition(4, 0)));
            room.AddBlock(new SandTile(RoomLayout.TilePosition(5, 0)));
            room.AddBlock(new Stairs(RoomLayout.TilePosition(6, 0)));
            room.AddBlock(new WhiteBrick(RoomLayout.TilePosition(7, 0)));
            room.AddBlock(new Ladder(RoomLayout.TilePosition(8, 0)));
            room.AddBlock(new FireBlock(RoomLayout.TilePosition(9, 0)));
            room.AddCharacter(new OldMan(RoomLayout.TilePosition(10, 0)));
            room.AddBlock(new FireBlock(RoomLayout.TilePosition(11, 0)));

            // Middle: every enemy, with Aquamentus on the right breathing fire across the room
            room.AddCharacter(new Stalfos(RoomLayout.TilePosition(1, 2)));
            room.AddCharacter(new Zol(RoomLayout.TilePosition(3, 2)));
            room.AddCharacter(new Gel(RoomLayout.TilePosition(5, 2)));
            room.AddCharacter(new Keese(RoomLayout.TilePosition(7, 2)));
            room.AddCharacter(new Goriya(RoomLayout.TilePosition(2, 3)));
            room.AddCharacter(new Wallmaster(RoomLayout.TilePosition(4, 3)));
            room.AddCharacter(new BladeTrap(RoomLayout.TilePosition(6, 3)));
            room.AddCharacter(new Aquamentus(RoomLayout.TilePosition(9.5f, 2)));

            // Second row from the bottom: every item
            room.AddItem(new Heart(RoomLayout.TilePosition(0, 5)));
            room.AddItem(new HeartContainer(RoomLayout.TilePosition(1, 5)));
            room.AddItem(new Rupee(RoomLayout.TilePosition(2, 5)));
            room.AddItem(new TriforcePiece(RoomLayout.TilePosition(3, 5)));
            room.AddItem(new Key(RoomLayout.TilePosition(4, 5)));
            room.AddItem(new Map(RoomLayout.TilePosition(5, 5)));
            room.AddItem(new Compass(RoomLayout.TilePosition(6, 5)));
            room.AddItem(new Bow(RoomLayout.TilePosition(7, 5)));
            room.AddItem(new BoomerangItem(RoomLayout.TilePosition(8, 5)));
            room.AddItem(new BombItem(RoomLayout.TilePosition(9, 5)));
            room.AddItem(new Clock(RoomLayout.TilePosition(10, 5)));
            room.AddItem(new Fairy(RoomLayout.TilePosition(11, 5)));

            return room;
        }
    }
}
