using Microsoft.Xna.Framework;

namespace Interfaces
{
    // Anything that can throw a boomerang and catch it again (Link, Goriya)
    internal interface IBoomerangThrower
    {

        public Vector2 Position { get; }

        public void OnBoomerangReturned();

    }
}
