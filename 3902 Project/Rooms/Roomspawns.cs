using System.Collections.Generic;
using Projectiles;

namespace Rooms
{
    // Which enemies start in each room from DungeonRooms, and exactly where. Rooms that appear on the
    // dungeon map use the enemies shown there; the rest have a mix of enemy types so no room is empty.
    // To change a room, edit its list: a spawn is its kind, then the floor column (0-11) and row (0-6)
    // of its top-left corner, and halves like 7.5 are allowed. Rooms not listed here have no enemies.
    // Once spawned, every enemy moves by itself (see Enemy.cs, HoppingEnemy.cs and the states folder)
    internal static class RoomSpawns
    {
        // Adds the enemies for the room built from DungeonRooms layout number index (0 is Room 1)
        public static void AddTo(Room room, int index, ProjectileManager projectiles)
        {
            if (!Spawns.TryGetValue(index + 1, out SpawnPoint[] points))
                return;

            foreach (SpawnPoint point in points)
                Spawner.Spawn(room, point, projectiles);
        }

        // The key is the room number, as in the "Room N" comments in DungeonRooms
        private static readonly Dictionary<int, SpawnPoint[]> Spawns = new()
        {
            // Room 1: 3 Keese
            [1] =
            [
                new(SpawnKind.Keese, 3, 2),
                new(SpawnKind.Keese, 3, 4),
                new(SpawnKind.Keese, 1, 5)
            ],

            // Room 3: 1 Goriya, 4 Stalfos
            [3] =
            [
                new(SpawnKind.Goriya, 2.5f, 0),
                new(SpawnKind.Stalfos, 6, 1.5f),
                new(SpawnKind.Stalfos, 10, 1),
                new(SpawnKind.Stalfos, 6.5f, 4),
                new(SpawnKind.Stalfos, 3, 5.5f)
            ],

            // Room 4: 6 Keese
            [4] =
            [
                new(SpawnKind.Keese, 8, 0),
                new(SpawnKind.Keese, 1, 1),
                new(SpawnKind.Keese, 3, 2),
                new(SpawnKind.Keese, 0, 3),
                new(SpawnKind.Keese, 3, 4),
                new(SpawnKind.Keese, 8, 6)
            ],

            // Room 5: 3 Goriya
            [5] =
            [
                new(SpawnKind.Goriya, 10, 1),
                new(SpawnKind.Goriya, 8, 2),
                new(SpawnKind.Goriya, 8, 3.5f)
            ],

            // Room 6: 8 Keese
            [6] =
            [
                new(SpawnKind.Keese, 3, 0),
                new(SpawnKind.Keese, 10, 1),
                new(SpawnKind.Keese, 8, 2),
                new(SpawnKind.Keese, 11, 3),
                new(SpawnKind.Keese, 6, 4),
                new(SpawnKind.Keese, 8, 4),
                new(SpawnKind.Keese, 10, 5),
                new(SpawnKind.Keese, 3, 6)
            ],

            // Room 7: 3 Gel
            [7] =
            [
                new(SpawnKind.Gel, 7.5f, 0),
                new(SpawnKind.Gel, 6.5f, 2),
                new(SpawnKind.Gel, 7.5f, 4)
            ],

            // Room 8: 5 Gel
            [8] =
            [
                new(SpawnKind.Gel, 6, 1.5f),
                new(SpawnKind.Gel, 8, 2),
                new(SpawnKind.Gel, 6, 4.5f),
                new(SpawnKind.Gel, 8, 4),
                new(SpawnKind.Gel, 11, 3)
            ],

            // Room 9: 2 Wallmaster
            [9] =
            [
                new(SpawnKind.Wallmaster, 2, 0),
                new(SpawnKind.Wallmaster, 0, 3.5f)
            ],

            // Room 10: 3 Stalfos
            [10] =
            [
                new(SpawnKind.Stalfos, 1, 1),
                new(SpawnKind.Stalfos, 10, 1),
                new(SpawnKind.Stalfos, 8, 4)
            ],

            // Room 11: 1 Aquamentus
            [11] =
            [
                new(SpawnKind.Aquamentus, 8.5f, 2.5f)
            ],

            // Room 13: 4 BladeTrap
            [13] =
            [
                new(SpawnKind.BladeTrap, 0, 0),
                new(SpawnKind.BladeTrap, 11, 0),
                new(SpawnKind.BladeTrap, 0, 6),
                new(SpawnKind.BladeTrap, 11, 6)
            ],

            // Room 14: 3 Goriya
            [14] =
            [
                new(SpawnKind.Goriya, 7.5f, 0),
                new(SpawnKind.Goriya, 6.5f, 2),
                new(SpawnKind.Goriya, 4, 1.5f)
            ],

            // Room 15: 3 Stalfos
            [15] =
            [
                new(SpawnKind.Stalfos, 6, 2),
                new(SpawnKind.Stalfos, 11, 4),
                new(SpawnKind.Stalfos, 3, 5)
            ],

            // Room 16: 4 Keese
            [16] =
            [
                new(SpawnKind.Keese, 11, 0),
                new(SpawnKind.Keese, 9, 2),
                new(SpawnKind.Keese, 6, 3),
                new(SpawnKind.Keese, 1, 4)
            ],

            // Room 17: 4 Gel
            [17] =
            [
                new(SpawnKind.Gel, 1, 0),
                new(SpawnKind.Gel, 4, 1),
                new(SpawnKind.Gel, 7, 1),
                new(SpawnKind.Gel, 0, 2)
            ],

            // Room 18: 2 Goriya
            [18] =
            [
                new(SpawnKind.Goriya, 4, 1),
                new(SpawnKind.Goriya, 6, 2)
            ],

            // Room 19: 2 Zol
            [19] =
            [
                new(SpawnKind.Zol, 5, 1),
                new(SpawnKind.Zol, 8, 3)
            ],

            // Room 20: 2 Wallmaster
            [20] =
            [
                new(SpawnKind.Wallmaster, 9, 0),
                new(SpawnKind.Wallmaster, 0, 3)
            ],

            // Room 21: 3 Stalfos
            [21] =
            [
                new(SpawnKind.Stalfos, 0, 3),
                new(SpawnKind.Stalfos, 4, 4),
                new(SpawnKind.Stalfos, 0, 5)
            ],

            // Room 22: 4 Keese
            [22] =
            [
                new(SpawnKind.Keese, 2, 0),
                new(SpawnKind.Keese, 10, 0),
                new(SpawnKind.Keese, 6, 3),
                new(SpawnKind.Keese, 0, 4)
            ],

            // Room 23: 4 Gel
            [23] =
            [
                new(SpawnKind.Gel, 0, 0),
                new(SpawnKind.Gel, 6, 1),
                new(SpawnKind.Gel, 3, 4),
                new(SpawnKind.Gel, 9, 6)
            ],

            // Room 24: 2 Goriya
            [24] =
            [
                new(SpawnKind.Goriya, 0, 3),
                new(SpawnKind.Goriya, 7, 4)
            ],

            // Room 25: 2 Zol
            [25] =
            [
                new(SpawnKind.Zol, 10, 0),
                new(SpawnKind.Zol, 1, 5)
            ],

            // Room 26: 2 Wallmaster
            [26] =
            [
                new(SpawnKind.Wallmaster, 1, 5),
                new(SpawnKind.Wallmaster, 8, 5)
            ],

            // Room 27: 3 Stalfos
            [27] =
            [
                new(SpawnKind.Stalfos, 1, 1),
                new(SpawnKind.Stalfos, 2, 5),
                new(SpawnKind.Stalfos, 10, 5)
            ],

            // Room 28: 4 Keese
            [28] =
            [
                new(SpawnKind.Keese, 0, 1),
                new(SpawnKind.Keese, 3, 2),
                new(SpawnKind.Keese, 1, 4),
                new(SpawnKind.Keese, 9, 5)
            ],

            // Room 29: 4 Gel
            [29] =
            [
                new(SpawnKind.Gel, 7, 2),
                new(SpawnKind.Gel, 5, 3),
                new(SpawnKind.Gel, 0, 6),
                new(SpawnKind.Gel, 9, 6)
            ],

            // Room 30: 2 Goriya
            [30] =
            [
                new(SpawnKind.Goriya, 7, 0),
                new(SpawnKind.Goriya, 11, 1)
            ],

            // Room 31: 2 Zol
            [31] =
            [
                new(SpawnKind.Zol, 5, 0),
                new(SpawnKind.Zol, 11, 3)
            ],

            // Room 32: 2 Wallmaster
            [32] =
            [
                new(SpawnKind.Wallmaster, 9, 3),
                new(SpawnKind.Wallmaster, 11, 5)
            ],

            // Room 33: 3 Stalfos
            [33] =
            [
                new(SpawnKind.Stalfos, 5, 0),
                new(SpawnKind.Stalfos, 1, 6),
                new(SpawnKind.Stalfos, 8, 6)
            ],

            // Room 34: 4 Keese
            [34] =
            [
                new(SpawnKind.Keese, 7, 0),
                new(SpawnKind.Keese, 11, 3),
                new(SpawnKind.Keese, 2, 4),
                new(SpawnKind.Keese, 3, 6)
            ],

            // Room 35: 4 Gel
            [35] =
            [
                new(SpawnKind.Gel, 8, 0),
                new(SpawnKind.Gel, 8, 4),
                new(SpawnKind.Gel, 3, 6),
                new(SpawnKind.Gel, 11, 6)
            ],

            // Room 36: 2 Goriya
            [36] =
            [
                new(SpawnKind.Goriya, 5, 3),
                new(SpawnKind.Goriya, 9, 3)
            ],

            // Room 37: 2 Zol
            [37] =
            [
                new(SpawnKind.Zol, 8, 1),
                new(SpawnKind.Zol, 3, 5)
            ],

            // Room 38: 2 Wallmaster
            [38] =
            [
                new(SpawnKind.Wallmaster, 6, 3),
                new(SpawnKind.Wallmaster, 0, 6)
            ],

            // Room 39: 3 Stalfos
            [39] =
            [
                new(SpawnKind.Stalfos, 8, 0),
                new(SpawnKind.Stalfos, 0, 3),
                new(SpawnKind.Stalfos, 1, 6)
            ],

            // Room 40: 4 Keese
            [40] =
            [
                new(SpawnKind.Keese, 0, 3),
                new(SpawnKind.Keese, 3, 3),
                new(SpawnKind.Keese, 10, 3),
                new(SpawnKind.Keese, 6, 4)
            ],

            // Room 41: 4 Gel
            [41] =
            [
                new(SpawnKind.Gel, 4, 0),
                new(SpawnKind.Gel, 11, 2),
                new(SpawnKind.Gel, 8, 4),
                new(SpawnKind.Gel, 0, 6)
            ],

            // Room 42: 2 Fire, 1 OldMan
            [42] =
            [
                new(SpawnKind.Fire, 2.5f, 2),
                new(SpawnKind.OldMan, 5.5f, 2),
                new(SpawnKind.Fire, 8.5f, 2)
            ],

            // Room 43: 2 Goriya
            [43] =
            [
                new(SpawnKind.Goriya, 8, 5),
                new(SpawnKind.Goriya, 2, 6)
            ],

            // Room 44: 2 Zol
            [44] =
            [
                new(SpawnKind.Zol, 5, 0),
                new(SpawnKind.Zol, 8, 0)
            ]
        };
    }
}