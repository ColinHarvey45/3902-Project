using Enemies;
using Environment;
using Microsoft.Xna.Framework;
using Npcs;
using Projectiles;

namespace Rooms
{
    // Everything that can be placed in a room from the dungeon map
    internal enum SpawnKind
    {
        Stalfos, Zol, Gel, Keese, Goriya, Wallmaster, BladeTrap, Aquamentus,
        OldMan, Fire
    }

    // One thing placed in a room: what it is, and which floor tile its top-left corner is on.
    // Columns run 0-11 and rows 0-6; halves are allowed, e.g. 7.5
    internal readonly record struct SpawnPoint(SpawnKind Kind, float Column, float Row)
    {
        public Vector2 Position => RoomLayout.TilePosition(Column, Row);
    }

    // Creates what a spawn point describes and adds it to the room.
    // projectiles is where Goriya and Aquamentus put what they throw and breathe
    internal static class Spawner
    {
        public static void Spawn(Room room, SpawnPoint spawn, ProjectileManager projectiles)
        {
            Vector2 position = spawn.Position;

            switch (spawn.Kind)
            {
                case SpawnKind.Stalfos:
                    room.AddCharacter(new Stalfos(position));
                    break;
                case SpawnKind.Zol:
                    room.AddCharacter(new Zol(position));
                    break;
                case SpawnKind.Gel:
                    room.AddCharacter(new Gel(position));
                    break;
                case SpawnKind.Keese:
                    room.AddCharacter(new Keese(position));
                    break;
                case SpawnKind.Goriya:
                    room.AddCharacter(new Goriya(position, projectiles));
                    break;
                case SpawnKind.Wallmaster:
                    room.AddCharacter(new Wallmaster(position));
                    break;
                case SpawnKind.BladeTrap:
                    room.AddCharacter(new BladeTrap(position));
                    break;
                case SpawnKind.Aquamentus:
                    room.AddCharacter(new Aquamentus(position, projectiles));
                    break;
                case SpawnKind.OldMan:
                    room.AddCharacter(new OldMan(position));
                    break;
                case SpawnKind.Fire:
                    room.AddBlock(new FireBlock(position));
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(spawn), spawn.Kind, "Unknown spawn kind");
            }
        }
    }
}