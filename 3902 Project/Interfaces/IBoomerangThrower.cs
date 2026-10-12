using Microsoft.Xna.Framework;

namespace Interfaces
{
    // Anything that can throw a boomerang (Link, Goriya); the boomerang flies back to wherever they are now
    internal interface IBoomerangThrower
    {

        // The thrower's middle, which the boomerang flies back to
        public Vector2 Center { get; }

    }
}
