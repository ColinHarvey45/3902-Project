using Microsoft.Xna.Framework;

namespace Interfaces
{
    // A hitbox that follows something around the room
    internal interface ICollider
    {

        // Moves the box to its owner's position (the top-left corner, or the middle for arrows and
        // boomerangs), shifted by the box's offset, in screen pixels
        public void Update(Vector2 position);
        public Rectangle GetCollider();

        // True if this box and other overlap
        public bool Intersects(ICollider other);

    }
}
