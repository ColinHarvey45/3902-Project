using Microsoft.Xna.Framework;

namespace Interfaces
{
    // Anything that can throw a boomerang (Link, Goriya); the boomerang flies back to wherever they are now
    internal interface IBoomerangThrower
    {

        public Vector2 Position { get; }

    }
}
