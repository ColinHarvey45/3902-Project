using Interfaces;
using Microsoft.Xna.Framework;

namespace Colliders
{
    internal sealed class BoxCollider(Rectangle collider, Point offset = default) : ICollider
    {
        private Rectangle collider = collider;
        private readonly Point offset = offset;

        public int Width => collider.Width;
        public int Height => collider.Height;

        public bool Intersects(ICollider other)
        {
            return collider.Intersects(other.GetCollider());
        }

        public Rectangle GetCollider()
        {
            return collider;
        }

        public void Update(Vector2 position)
        {
            collider.Location = new Point((int)position.X + offset.X, (int)position.Y + offset.Y);
        }

    }
}

