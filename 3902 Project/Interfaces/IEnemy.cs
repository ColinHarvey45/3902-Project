using Microsoft.Xna.Framework;

namespace Interfaces
{
    // A character that fights Link: touching it hurts him
    internal interface IEnemy : ICharacter
    {

        ICollider Collider { get; set; }
        public void UpdateCollider();

        // Called when it has moved into a wall or a solid block; push moves it straight back out.
        // Each enemy decides what to do next (most turn and walk another way)
        public void OnBlocked(Vector2 push);

    }
}
