namespace Interfaces
{
    // A door that Link opens with a key. The room on its other side has the same door, which opens with it
    internal interface ILockedDoor : IBlock
    {

        public bool IsLocked { get; }

        public void Unlock();

    }
}
