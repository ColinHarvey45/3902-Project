using System;
using System.Collections.Generic;
using System.Linq;
using CSE_3902_Project;
using Interfaces;
using Microsoft.Xna.Framework;
using Player;
using Projectiles;
using Rooms;

namespace Collisions
{
    // Moving about: walls, blocks and NPCs stopping Link (who sidesteps round corners and pushes blocks), walls and
    // blocks stopping enemies, and projectiles ending at them
    internal static class MovementCollisions
    {
        // Link steps round a corner if stepping aside half a tile or less gets him past it, as fast as he walks
        private const int SidestepLimit = RoomLayout.TileSize / 2;
        private const int SidestepSpeed = (int)GameSettings.LinkWalkSpeed;

        // Link can't walk into a wall, a solid block (closed doors included) or an NPC: he is pushed straight back out
        public static void WallsBlocksAndNpcsStopLink(IPlayer link, Room room)
        {
            List<Rectangle> obstacles = [.. Obstacles(room), .. room.Npcs.Select(npc => npc.Collider.GetCollider())];
            Vector2 pushedBack = Vector2.Zero;

            foreach (Rectangle obstacle in obstacles)
            {
                Vector2 push = PushOut(link.Collider.GetCollider(), obstacle);
                if (push != Vector2.Zero)
                {
                    link.Push(push);
                    pushedBack += push;
                }
            }

            LinkPushesBlocks(link, room, obstacles, pushedBack);
            LinkSidestepsCorners(link, obstacles, pushedBack);
        }

        // Link walking straight into a pushable block pushes it; once he has pushed long enough it slides a tile,
        // as long as that tile is clear floor. As in the NES, blocks won't move until the room is cleared
        private static void LinkPushesBlocks(IPlayer link, Room room, List<Rectangle> obstacles, Vector2 pushedBack)
        {
            if (!room.IsCleared || pushedBack == Vector2.Zero || (pushedBack.X != 0 && pushedBack.Y != 0))
                return;

            Direction pushing = (-pushedBack).ToDirection();
            Rectangle reach = link.Collider.GetCollider();
            reach.Offset(pushing.ToVector().ToPoint());

            foreach (IBlock block in room.Blocks)
            {
                if (block is not IPushableBlock pushable || !reach.Intersects(pushable.Collider.GetCollider())
                    || !pushable.PushedBy(pushing))
                    continue;

                Rectangle itself = pushable.Collider.GetCollider();
                Rectangle destination = pushable.Destination(pushing);
                bool clear = RoomLayout.FloorBounds.Contains(destination)
                    && !obstacles.Exists(obstacle => obstacle != itself && obstacle.Intersects(destination))
                    && !room.Enemies.Any(enemy => enemy.Collider != null && enemy.Collider.GetCollider().Intersects(destination));

                if (clear)
                    pushable.Slide(pushing);
            }
        }

        // Link walking into the corner of a wall or block, with most of him past it already, steps sideways round
        // it instead of stopping dead, as in the NES, where he lines up with the gaps between blocks as he turns.
        // pushedBack is how far he was just pushed back out of things, which is the opposite way to where he was going
        private static void LinkSidestepsCorners(IPlayer link, List<Rectangle> obstacles, Vector2 pushedBack)
        {
            // Only when he was stopped going straight along one axis
            if (pushedBack == Vector2.Zero || (pushedBack.X != 0 && pushedBack.Y != 0))
                return;

            Point forward = new(-Math.Sign(pushedBack.X), -Math.Sign(pushedBack.Y));
            Point sideways = new(Math.Abs(forward.Y), Math.Abs(forward.X));
            Rectangle box = link.Collider.GetCollider();

            int towards = SidestepToGetPast(box, forward, sideways, obstacles) ?? int.MaxValue;
            int away = SidestepToGetPast(box, forward, new Point(-sideways.X, -sideways.Y), obstacles) ?? int.MaxValue;

            // Whichever way is shorter. If neither works, or both are as far, he is square on to it and stays stopped
            if (towards == away)
                return;

            int direction = towards < away ? 1 : -1;
            int distance = Math.Min(towards, away);
            link.Push(sideways.ToVector2() * direction * Math.Min(SidestepSpeed, distance));
        }

        // How far box would have to step sideways (along side) before it could move forward without touching
        // anything, or null if that is further than SidestepLimit or something is in the way of the sidestep
        private static int? SidestepToGetPast(Rectangle box, Point forward, Point side, List<Rectangle> obstacles)
        {
            int distance = 0;
            while (distance <= SidestepLimit)
            {
                Rectangle moved = box;
                moved.Offset(forward.X + side.X * distance, forward.Y + side.Y * distance);

                int blocking = obstacles.FindIndex(obstacle => obstacle.Intersects(moved));
                if (blocking < 0)
                {
                    Rectangle sidestep = Rectangle.Union(box, new Rectangle(box.X + side.X * distance, box.Y + side.Y * distance, box.Width, box.Height));
                    return obstacles.Exists(obstacle => obstacle.Intersects(sidestep)) ? null : distance;
                }

                // Far enough along to clear the side of whatever was in the way
                Rectangle obstacle = obstacles[blocking];
                distance += side.X > 0 ? obstacle.Right - moved.Left
                    : side.X < 0 ? moved.Right - obstacle.Left
                    : side.Y > 0 ? obstacle.Bottom - moved.Top
                    : moved.Bottom - obstacle.Top;
            }

            return null;
        }

        // Enemies can't walk into walls or solid blocks either; each one decides what to do instead
        public static void WallsAndBlocksStopEnemies(Room room)
        {
            foreach (IEnemy enemy in room.Enemies)
            {
                if (enemy.Collider == null)
                    continue;

                foreach (Rectangle obstacle in Obstacles(room))
                {
                    Vector2 push = PushOut(enemy.Collider.GetCollider(), obstacle);
                    if (push != Vector2.Zero)
                        enemy.OnBlocked(push);
                }
            }
        }

        // Arrows and fireballs end at walls and solid blocks, and boomerangs turn back
        public static void ProjectilesHitWallsAndBlocks(Room room, ProjectileManager projectiles)
        {
            foreach (IProjectile projectile in projectiles.ActiveProjectiles)
            {
                if (projectile.IsFinished || projectile.Collider == null)
                    continue;

                Rectangle box = projectile.Collider.GetCollider();
                if (Obstacles(room).Any(obstacle => obstacle.Intersects(box)))
                    projectile.Hit();
            }
        }

        // The room's walls (and the strips just outside it), and every solid block, closed doors included
        private static IEnumerable<Rectangle> Obstacles(Room room)
        {
            foreach (Rectangle wall in room.Walls)
                yield return wall;

            foreach (IBlock block in room.Blocks)
            {
                if (block.IsSolid)
                    yield return block.Collider.GetCollider();
            }
        }

        // How far to move a box so it no longer overlaps an obstacle: straight out across whichever side
        // overlaps the least, as in Caden's original block collision. Zero if they don't overlap
        private static Vector2 PushOut(Rectangle box, Rectangle obstacle)
        {
            Rectangle overlap = Rectangle.Intersect(box, obstacle);
            if (overlap.IsEmpty)
                return Vector2.Zero;

            if (overlap.Width < overlap.Height)
                return new Vector2(box.Center.X < obstacle.Center.X ? -overlap.Width : overlap.Width, 0);

            return new Vector2(0, box.Center.Y < obstacle.Center.Y ? -overlap.Height : overlap.Height);
        }
    }
}
