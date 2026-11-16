namespace Interfaces
{
    // An enemy that waits out of sight (in the walls, like the Wallmaster) and comes out when the room lets it, so
    // only one is out at a time (see Room). While hidden it has no hitbox, so nothing can touch it
    internal interface IHidingEnemy : IEnemy
    {

        public bool IsHidden { get; }

        // Hidden, and ready to come out: it has waited long enough, and Link is somewhere it can come out by
        public bool IsReadyToComeOut { get; }

        public void ComeOut();

    }
}
