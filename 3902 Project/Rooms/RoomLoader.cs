using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Enemies;
using Environment;
using Interfaces;
using Items;
using Microsoft.Xna.Framework;
using Npcs;
using Player;
using Projectiles;

namespace Rooms
{
    // Builds rooms from the CSV files in Levels/. Each line of a room file adds something to the room:
    //   Door,<wall>,<kind>            the door (or plain wall) filling the gap in that wall, e.g. Door,Up,KeyholeLockedDoor
    //   Floor,<12 symbols>            one row of the 12 x 7 floor, top row first; the symbols are in TileSymbols
    //   Tiles,<16 symbols>            one row of the whole 16 x 11 room, walls included, for a room without the
    //                                 usual walls (the underground passage); same symbols, and '.' is empty black
    //   Block,<kind>,<column>,<row>   e.g. Block,FireBlock,9,0
    //   Enemy,<kind>,<column>,<row>   e.g. Enemy,Stalfos,6,4
    //   Npc,<kind>,<column>,<row>     e.g. Npc,OldMan,5.5,2
    //   Item,<kind>,<column>,<row>    e.g. Item,Key,5,3
    // Kinds are class names. Columns run 0-11 and rows 0-6 across the floor, and halves like 5.5 are
    // allowed. Blank lines, and lines starting with #, are ignored.
    // projectiles is where Goriya and Aquamentus put what they throw and breathe
    internal sealed class RoomLoader(ProjectileManager projectiles)
    {
        private const char NothingOnTile = '.';

        // What each symbol on a Floor or Tiles line puts on its tile; '.' leaves the tile as it is
        private static readonly Dictionary<char, Func<Vector2, IBlock>> TileSymbols = new()
        {
            ['Q'] = position => new SquareBlock(position),
            ['F'] = position => new FishStatue(position),
            ['D'] = position => new DragonStatue(position),
            ['G'] = position => new BlueGap(position),
            ['B'] = position => new BlackTile(position),
            ['S'] = position => new SandTile(position),
            ['T'] = position => new Stairs(position),
            ['W'] = position => new WhiteBrick(position),
            ['L'] = position => new Ladder(position)
        };

        private static readonly Dictionary<string, Func<Direction, IBlock>> DoorKinds = new()
        {
            [nameof(OpenDoor)] = side => new OpenDoor(side),
            [nameof(KeyholeLockedDoor)] = side => new KeyholeLockedDoor(side),
            [nameof(DiamondLockedDoor)] = side => new DiamondLockedDoor(side),
            [nameof(BombedWallOpening)] = side => new BombedWallOpening(side),
            [nameof(Wall)] = side => new Wall(side)
        };

        private static readonly Dictionary<string, Func<Vector2, IBlock>> BlockKinds = new()
        {
            [nameof(SquareBlock)] = position => new SquareBlock(position),
            [nameof(FishStatue)] = position => new FishStatue(position),
            [nameof(DragonStatue)] = position => new DragonStatue(position),
            [nameof(BlueGap)] = position => new BlueGap(position),
            [nameof(FloorTile)] = position => new FloorTile(position),
            [nameof(BlackTile)] = position => new BlackTile(position),
            [nameof(SandTile)] = position => new SandTile(position),
            [nameof(Stairs)] = position => new Stairs(position),
            [nameof(WhiteBrick)] = position => new WhiteBrick(position),
            [nameof(Ladder)] = position => new Ladder(position),
            [nameof(FireBlock)] = position => new FireBlock(position)
        };

        private readonly Dictionary<string, Func<Vector2, ICharacter>> enemyKinds = new()
        {
            [nameof(Stalfos)] = position => new Stalfos(position),
            [nameof(Zol)] = position => new Zol(position),
            [nameof(Gel)] = position => new Gel(position),
            [nameof(Keese)] = position => new Keese(position),
            [nameof(Goriya)] = position => new Goriya(position, projectiles),
            [nameof(Wallmaster)] = position => new Wallmaster(position),
            [nameof(BladeTrap)] = position => new BladeTrap(position),
            [nameof(Aquamentus)] = position => new Aquamentus(position, projectiles)
        };

        private static readonly Dictionary<string, Func<Vector2, ICharacter>> NpcKinds = new()
        {
            [nameof(OldMan)] = position => new OldMan(position)
        };

        private static readonly Dictionary<string, Func<Vector2, IItem>> ItemKinds = new()
        {
            [nameof(Heart)] = position => new Heart(position),
            [nameof(HeartContainer)] = position => new HeartContainer(position),
            [nameof(Rupee)] = position => new Rupee(position),
            [nameof(TriforcePiece)] = position => new TriforcePiece(position),
            [nameof(Fairy)] = position => new Fairy(position),
            [nameof(Key)] = position => new Key(position),
            [nameof(Map)] = position => new Map(position),
            [nameof(Compass)] = position => new Compass(position),
            [nameof(Bow)] = position => new Bow(position),
            [nameof(BoomerangItem)] = position => new BoomerangItem(position),
            [nameof(BombItem)] = position => new BombItem(position),
            [nameof(Clock)] = position => new Clock(position)
        };

        // Every room file in folder, in file-name order, which is the order the rooms are switched through
        public Room[] LoadAll(string folder)
        {
            Room[] rooms = Directory.GetFiles(folder, "*.csv")
                .Order(StringComparer.Ordinal)
                .Select(Load)
                .ToArray();

            if (rooms.Length == 0)
                throw new InvalidDataException($"There are no room files in {folder}");

            return rooms;
        }

        // A mistake in the file stops the game with the file name, the line number and what is wrong
        public Room Load(string path)
        {
            Room room = new();
            TileGrid floor = new("Floor", RoomLayout.FloorColumns, RoomLayout.FloorRows, RoomLayout.TilePosition);
            TileGrid wholeRoom = new("Tiles", RoomLayout.RoomColumns, RoomLayout.RoomRows, RoomLayout.RoomTilePosition);
            string[] lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.Length == 0 || line.StartsWith('#'))
                    continue;

                try
                {
                    AddLine(room, line.Split(',', StringSplitOptions.TrimEntries), floor, wholeRoom);
                }
                catch (InvalidDataException e)
                {
                    throw new InvalidDataException($"{Path.GetFileName(path)}, line {i + 1}: {e.Message}", e);
                }
            }

            try
            {
                floor.CheckComplete();
                wholeRoom.CheckComplete();
            }
            catch (InvalidDataException e)
            {
                throw new InvalidDataException($"{Path.GetFileName(path)}: {e.Message}", e);
            }

            return room;
        }

        private void AddLine(Room room, string[] fields, TileGrid floor, TileGrid wholeRoom)
        {
            switch (fields[0])
            {
                case "Door":
                    ExpectValues(fields, 2);
                    room.AddBlock(Find(DoorKinds, fields[2], "door")(ParseWall(fields[1])));
                    break;

                case "Floor":
                    ExpectValues(fields, 1);
                    floor.AddRow(room, fields[1]);
                    break;

                case "Tiles":
                    ExpectValues(fields, 1);
                    room.RemoveDungeonWalls();
                    wholeRoom.AddRow(room, fields[1]);
                    break;

                case "Block":
                    room.AddBlock(CreateOnTile(BlockKinds, fields, "block"));
                    break;

                case "Enemy":
                    room.AddCharacter(CreateOnTile(enemyKinds, fields, "enemy"));
                    break;

                case "Npc":
                    room.AddCharacter(CreateOnTile(NpcKinds, fields, "NPC"));
                    break;

                case "Item":
                    room.AddItem(CreateOnTile(ItemKinds, fields, "item"));
                    break;

                default:
                    throw new InvalidDataException(
                        $"'{fields[0]}' isn't a kind of line (Door, Floor, Tiles, Block, Enemy, Npc or Item)");
            }
        }

        // Reads a grid of tile symbols one line at a time (the Floor lines, or the Tiles lines), and checks
        // that a room has either none of those lines or all of them, each the right length.
        // tilePosition turns a column and row in the grid into a position on screen
        private sealed class TileGrid(string lineKind, int columns, int rows, Func<float, float, Vector2> tilePosition)
        {

            private int rowsRead;

            public void AddRow(Room room, string symbols)
            {
                if (rowsRead == rows)
                    throw new InvalidDataException($"a room has only {rows} {lineKind} lines");

                if (symbols.Length != columns)
                    throw new InvalidDataException(
                        $"a {lineKind} line needs {columns} symbols, one per tile, but has {symbols.Length}");

                for (int column = 0; column < columns; column++)
                {
                    if (symbols[column] == NothingOnTile)
                        continue;

                    if (!TileSymbols.TryGetValue(symbols[column], out Func<Vector2, IBlock> create))
                        throw new InvalidDataException($"'{symbols[column]}' isn't a tile symbol");

                    room.AddBlock(create(tilePosition(column, rowsRead)));
                }

                rowsRead++;
            }

            public void CheckComplete()
            {
                if (rowsRead is not 0 && rowsRead != rows)
                    throw new InvalidDataException($"has {rowsRead} {lineKind} lines, but a room needs all {rows} or none");
            }

        }

        // A Block, Enemy, Npc or Item line: its kind, then the column and row of its top-left corner
        private static T CreateOnTile<T>(Dictionary<string, Func<Vector2, T>> kinds, string[] fields, string description)
        {
            ExpectValues(fields, 3);
            Func<Vector2, T> create = Find(kinds, fields[1], description);

            return create(RoomLayout.TilePosition(ParseNumber(fields[2]), ParseNumber(fields[3])));
        }

        private static T Find<T>(Dictionary<string, T> kinds, string name, string description)
        {
            return kinds.TryGetValue(name, out T found)
                ? found
                : throw new InvalidDataException($"'{name}' isn't a kind of {description}");
        }

        private static void ExpectValues(string[] fields, int count)
        {
            if (fields.Length - 1 != count)
                throw new InvalidDataException($"{fields[0]} lines need {count} values after '{fields[0]}', but this one has {fields.Length - 1}");
        }

        private static float ParseNumber(string text)
        {
            return float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float number)
                ? number
                : throw new InvalidDataException($"'{text}' isn't a number");
        }

        private static Direction ParseWall(string text)
        {
            return Enum.TryParse(text, out Direction wall) && Enum.IsDefined(wall)
                ? wall
                : throw new InvalidDataException($"'{text}' isn't a wall (Up, Down, Left or Right)");
        }
    }
}
