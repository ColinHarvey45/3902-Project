using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;

namespace Rooms
{
    // A room seen from the side, like the underground passage in the NES: Link walks left and right on the bricks,
    // goes up and down only on a ladder (the top of one counts as ground), and falls when there is nothing under him
    internal static class SideView
    {
        private const int FallSpeed = 4; // pixels per frame

        // before is where Link was before he moved this frame
        public static void Apply(IPlayer link, Room room, Vector2 before)
        {
            if (OnLadder(link, room))
                return;

            float climbed = link.Position.Y - before.Y;
            if (climbed < 0)
            {
                // Off the top of a ladder: back down just far enough to stand on top of it (or all the way, if
                // he wasn't on one)
                for (int step = 0; step < -climbed && !OnLadder(link, room); step++)
                    link.Push(new Vector2(0, 1));

                if (OnLadder(link, room))
                    return;
            }
            else if (climbed > 0)
            {
                link.Push(new Vector2(0, -climbed));
            }

            // He falls until he lands on something, so he never sinks into the bricks
            Rectangle box = link.Collider.GetCollider();
            int fall = 0;
            while (fall < FallSpeed && !Blocked(room, box, fall + 1))
                fall++;

            if (fall > 0)
                link.Push(new Vector2(0, fall));
        }

        // A ladder down Link's middle, or just under his feet
        private static bool OnLadder(IPlayer link, Room room)
        {
            Rectangle box = link.Collider.GetCollider();
            Rectangle middle = new(box.Center.X, box.Top, 1, box.Height + 1);

            return room.Blocks.Any(block => block.IsClimbable && block.Collider.GetCollider().Intersects(middle));
        }

        // Whether box, moved down by drop, would be in a solid block or past the edge of the room
        private static bool Blocked(Room room, Rectangle box, int drop)
        {
            box.Offset(0, drop);

            return room.Walls.Any(wall => wall.Intersects(box))
                || room.Blocks.Any(block => block.IsSolid && block.Collider.GetCollider().Intersects(box));
        }
    }
}
