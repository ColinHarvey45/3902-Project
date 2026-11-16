using Microsoft.Xna.Framework;
using Player;

namespace Interfaces
{
    // A block Link can push one tile, once, by walking against it for a moment (see CollisionManager)
    internal interface IPushableBlock : IBlock
    {

        // True once it has finished sliding its tile
        public bool HasMoved { get; }

        // Link is pushing against it this frame, going that way. True once he has pushed the same way for long
        // enough for it to move, if the tile beyond it is clear
        public bool PushedBy(Direction direction);

        // The tile it would slide onto, going that way
        public Rectangle Destination(Direction direction);

        public void Slide(Direction direction);

    }
}
