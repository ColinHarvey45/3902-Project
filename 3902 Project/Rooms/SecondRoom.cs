using Environment;
using Player;

namespace Rooms
{
    // Every room interior from the dungeon spritesheet, in sheet order (left to right, top to bottom).
    // Each layout is the 12 x 7 floor, one character per tile, read straight off the sheet:
    //   .  plain floor (nothing added)      Q  SquareBlock     F  FishStatue     D  DragonStatue
    //   G  BlueGap     B  BlackTile     S  SandTile     T  Stairs
    internal static class DungeonRooms
    {
        public static int Count => Layouts.Length;

        // Builds room number index (0 is the first room on the sheet)
        public static Room Create(int index)
        {
            Room room = new();

            // The same door set TestRoom uses, so every wall gap is filled
            room.AddBlock(new OpenDoor(Direction.Down));
            room.AddBlock(new KeyholeLockedDoor(Direction.Up));
            room.AddBlock(new BombedWallOpening(Direction.Left));
            room.AddBlock(new DiamondLockedDoor(Direction.Right));

            string[] layout = Layouts[index];

            for (int row = 0; row < RoomLayout.FloorRows; row++)
            {
                for (int column = 0; column < RoomLayout.FloorColumns; column++)
                {
                    AddTile(room, layout[row][column], column, row);
                }
            }

            return room;
        }

        private static void AddTile(Room room, char symbol, int column, int row)
        {
            switch (symbol)
            {
                case '.':
                    break;
                case 'Q':
                    room.AddBlock(new SquareBlock(RoomLayout.TilePosition(column, row)));
                    break;
                case 'F':
                    room.AddBlock(new FishStatue(RoomLayout.TilePosition(column, row)));
                    break;
                case 'D':
                    room.AddBlock(new DragonStatue(RoomLayout.TilePosition(column, row)));
                    break;
                case 'G':
                    room.AddBlock(new BlueGap(RoomLayout.TilePosition(column, row)));
                    break;
                case 'B':
                    room.AddBlock(new BlackTile(RoomLayout.TilePosition(column, row)));
                    break;
                case 'S':
                    room.AddBlock(new SandTile(RoomLayout.TilePosition(column, row)));
                    break;
                case 'T':
                    room.AddBlock(new Stairs(RoomLayout.TilePosition(column, row)));
                    break;
                default:
                    throw new System.ArgumentException($"Unknown tile '{symbol}' in a room layout");
            }
        }

        private static readonly string[][] Layouts =
        [
            // Room 1 (sheet row 1, column 1)
            [
                "............",
                "............",
                "............",
                "............",
                "............",
                "............",
                "............"
            ],
            // Room 2 (sheet row 1, column 2)
            [
                "............",
                ".F..F..D..D.",
                "............",
                ".F..F..D..D.",
                ".....SS.....",
                ".F.SFSSDS.D.",
                "...SSSSSS..."
            ],
            // Room 3 (sheet row 1, column 3)
            [
                "............",
                "............",
                "..QQ....QQ..",
                "..QQ....QQ..",
                "..QQ....QQ..",
                "............",
                "............"
            ],
            // Room 4 (sheet row 1, column 4)
            [
                "............",
                "............",
                ".....QQ.....",
                ".....QQ.....",
                ".....QQ.....",
                "............",
                "............"
            ],
            // Room 5 (sheet row 1, column 5)
            [
                "............",
                "............",
                "..Q......Q..",
                "............",
                "..Q......Q..",
                "............",
                "............"
            ],
            // Room 6 (sheet row 1, column 6)
            [
                "............",
                "..Q......Q..",
                "............",
                "............",
                "............",
                "..Q......Q..",
                "............"
            ],
            // Room 7 (sheet row 2, column 1)
            [
                "............",
                "............",
                "............",
                ".....Q......",
                "............",
                "............",
                "............"
            ],
            // Room 8 (sheet row 2, column 2)
            [
                "............",
                ".QQ......QQ.",
                "............",
                ".....QQ.....",
                "............",
                ".QQ......QQ.",
                "............"
            ],
            // Room 9 (sheet row 2, column 3)
            [
                "............",
                ".Q.Q.QQ.Q.Q.",
                "............",
                ".Q.Q.QQ.Q.Q.",
                "............",
                ".Q.Q.QQ.Q.Q.",
                "............"
            ],
            // Room 10 (sheet row 2, column 4)
            [
                "GGGGG..GGGGG",
                "G...G....G.G",
                "G.G.G.GG.G.G",
                "..G.G..G.G..",
                "G.G.GG.G.G.G",
                "G.G....G...G",
                "GGGGG..GGGGG"
            ],
            // Room 11 (sheet row 2, column 5)
            [
                ".......QQQQQ",
                ".........QQQ",
                "..........QQ",
                "............",
                "..........QQ",
                ".........QQQ",
                ".......QQQQQ"
            ],
            // Room 12 (sheet row 2, column 6)
            [
                "............",
                ".QQQQQQQQQQ.",
                ".Q..F..D..Q.",
                ".Q.F....D.Q.",
                ".Q........Q.",
                ".QQQQ..QQQQ.",
                "............"
            ],
            // Room 13 (sheet row 3, column 1)
            [
                "............",
                "......Q.....",
                ".....Q.Q....",
                "....Q.T.Q...",
                ".....Q.Q....",
                "......Q.....",
                "............"
            ],
            // Room 14 (sheet row 3, column 2)
            [
                "............",
                ".GGG....GGG.",
                ".G........G.",
                ".G.GGGGGG.G.",
                ".G........G.",
                ".GGG....GGG.",
                "............"
            ],
            // Room 15 (sheet row 3, column 3)
            [
                "F..........D",
                "............",
                "............",
                "............",
                "............",
                "............",
                "F..........D"
            ],
            // Room 16 (sheet row 3, column 4)
            [
                "SSSSSSSSSSSS",
                "SSSSSSSSSSSS",
                "SSSSSSSSSSSS",
                "SSSSSSSSSSSS",
                "SSSSSSSSSSSS",
                "SSSSSSSSSSSS",
                "SSSSSSSSSSSS"
            ],
            // Room 17 (sheet row 3, column 5)
            [
                "........QQQQ",
                "........QQQQ",
                ".........QQQ",
                "...........T",
                ".........QQQ",
                "........QQQQ",
                "........QQQQ"
            ],
            // Room 18 (sheet row 3, column 6)
            [
                "..Q........Q",
                ".Q...Q....Q.",
                "....Q....Q..",
                "...Q....Q...",
                "..Q....Q....",
                ".Q....Q...Q.",
                "Q........Q.."
            ],
            // Room 19 (sheet row 4, column 1)
            [
                "............",
                "............",
                "QQQQQQQQQQQQ",
                "............",
                "QQQQQQQQQQQQ",
                "............",
                "............"
            ],
            // Room 20 (sheet row 4, column 2)
            [
                "............",
                ".QQQQQQQQQQ.",
                "..........Q.",
                "..........Q.",
                "..........Q.",
                ".QQQQQQQQQQ.",
                "............"
            ],
            // Room 21 (sheet row 4, column 3)
            [
                "............",
                "............",
                "............",
                "....Q..Q....",
                "............",
                "............",
                "............"
            ],
            // Room 22 (sheet row 4, column 4)
            [
                ".Q.Q........",
                ".Q.QQQ.QQQ..",
                ".Q...Q...QQQ",
                ".Q.Q.Q...Q..",
                ".QQQ.Q...Q..",
                ".....Q.Q.Q.Q",
                "..Q....Q...Q"
            ],
            // Room 23 (sheet row 4, column 5)
            [
                "............",
                "............",
                "GGGGGGGGGGGG",
                "............",
                "............",
                "............",
                "............"
            ],
            // Room 24 (sheet row 4, column 6)
            [
                "............",
                ".GGGGGGGGGG.",
                ".GGGGGGGGGG.",
                ".GG......GG.",
                ".GG......GG.",
                ".GGGG..GGGG.",
                ".GGGG..GGGG."
            ],
            // Room 25 (sheet row 5, column 1)
            [
                "............",
                "............",
                "............",
                ".Q........Q.",
                "............",
                "...Q....Q...",
                "F..........D"
            ],
            // Room 26 (sheet row 5, column 2)
            [
                "QQQQQ..QQQQQ",
                "QQQQ....QQQQ",
                "QQ........QQ",
                "............",
                "............",
                "..Q......Q..",
                "............"
            ],
            // Room 27 (sheet row 5, column 3)
            [
                "GGGGG..GGGGG",
                "G...GGGG...G",
                "G.GGG..GGG.G",
                "..G......G..",
                "G.GGG..GGG.G",
                "G...GGGG...G",
                "GGGGG..GGGGG"
            ],
            // Room 28 (sheet row 5, column 4)
            [
                "............",
                "............",
                "..Q......Q..",
                ".Q........Q.",
                "..Q......Q..",
                "............",
                "............"
            ],
            // Room 29 (sheet row 5, column 5)
            [
                "........G...",
                "........G...",
                "........G...",
                "........G...",
                "........G...",
                "........G...",
                "........G..."
            ],
            // Room 30 (sheet row 5, column 6)
            [
                "............",
                ".QQQQQQQQQQ.",
                "............",
                ".QQQQQQQQQQ.",
                "............",
                ".QQQQQQQQQQ.",
                "............"
            ],
            // Room 31 (sheet row 6, column 1)
            [
                "............",
                ".GGGGGGGGGG.",
                ".G........G.",
                ".G........G.",
                ".G........G.",
                ".GGGGGGGGGG.",
                "............"
            ],
            // Room 32 (sheet row 6, column 2)
            [
                "............",
                "............",
                "............",
                "....F..D....",
                "............",
                "............",
                "............"
            ],
            // Room 33 (sheet row 6, column 3)
            [
                "............",
                "..QQQQQQQQ..",
                "..Q......Q..",
                "..Q......Q..",
                "..Q......Q..",
                "..QQQQQQQQ..",
                "............"
            ],
            // Room 34 (sheet row 6, column 4)
            [
                "............",
                "GGGGGGGGGGGG",
                "............",
                "............",
                "............",
                "GGGGGGGGGGGG",
                "............"
            ],
            // Room 35 (sheet row 6, column 5)
            [
                "..........Q.",
                ".QQQQQQQQ.Q.",
                ".Q.....TQ.Q.",
                ".Q...QQQQ.Q.",
                ".Q........Q.",
                ".QQQQQQQQQQ.",
                "............"
            ],
            // Room 36 (sheet row 6, column 6)
            [
                "FBB......BBD",
                "BB..B..B..BB",
                "BB.BB..BB.BB",
                "BB.BB..BB.BB",
                "BB...BB...BB",
                "BBBB....BBBB",
                "FBBB....BBBD"
            ],
            // Room 37 (sheet row 7, column 1)
            [
                "QQQQQQQQQQQQ",
                "Q...QBBQ...Q",
                "Q.QQBBBBQQ.Q",
                "Q.QQBBBBQQ.Q",
                "Q...F..D...Q",
                "Q..........Q",
                "Q.F......D.Q"
            ],
            // Room 38 (sheet row 7, column 2)
            [
                "....Q..Q....",
                "....Q..Q....",
                "....Q..Q....",
                "....Q..Q....",
                "....Q..Q....",
                "....Q..Q....",
                "....Q..Q...."
            ],
            // Room 39 (sheet row 7, column 3)
            [
                "............",
                ".Q.Q.QQ.Q.Q.",
                ".Q.Q.QQ.Q.Q.",
                ".Q.Q.QQ.Q.Q.",
                ".Q.Q.QQ.Q.Q.",
                ".Q.Q.QQ.Q.Q.",
                "............"
            ],
            // Room 40 (sheet row 7, column 4)
            [
                "QQQQQ..QQQQQ",
                "QQQQQQ.QQQQQ",
                "QQQQQQ.QQQQQ",
                "......Q.....",
                "QQQQQQ.QQQQQ",
                "QQQQQQ.QQQQQ",
                "QQQQQ..QQQQQ"
            ],
            // Room 41 (sheet row 7, column 5)
            [
                ".GG.........",
                ".GGGGGGGGGG.",
                ".GGGGGGGGGG.",
                ".G.......GG.",
                ".GG......GG.",
                ".GG......GG.",
                ".GG......GG."
            ],
            // Room 42 (sheet row 7, column 6)
            [
                "BBBBBBBBBBBB",
                "BBBBBBBBBBBB",
                "BBBBBBBBBBBB",
                "BBBBBBBBBBBB",
                "BBBBBBBBBBBB",
                "BBBBBBBBBBBB",
                "BBBBBBBBBBBB"
            ],
            // Room 43 (prototype room 1 at the bottom of the sheet)
            [
                "....Q.......",
                "...QQ.......",
                "..Q.QQQQQQQQ",
                ".Q..........",
                "..Q.QQQQQQQQ",
                "...QQ.......",
                "....Q......."
            ],
            // Room 44 (prototype room 2 at the bottom of the sheet)
            [
                ".GG.........",
                ".GGGGGGGGGG.",
                ".GGGGGGGGGG.",
                ".G.......GG.",
                ".G.......GG.",
                ".G.......GG.",
                ".G.......GG."
            ]
        ];
    }
}