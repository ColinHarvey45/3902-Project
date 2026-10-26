namespace Interfaces
{
    // A character that never fights, like the Old Man. Link can't walk through one.
    // NPC-only behaviour (showing a message when Link walks up) is added here in a later sprint
    internal interface INpc : ICharacter
    {

        public ICollider Collider { get; }

    }
}
