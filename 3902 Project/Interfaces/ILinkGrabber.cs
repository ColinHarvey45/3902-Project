namespace Interfaces
{
    // An enemy that catches Link instead of hurting him, like the Wallmaster, which drags him back to the start of
    // the dungeon (see RoomManager)
    internal interface ILinkGrabber : IEnemy
    {

        // True while it has hold of Link, and once it has carried him off and he should be taken back to the entrance
        public bool IsHoldingLink { get; }
        public bool HasCarriedLinkOff { get; }

        // Touched Link: catches him, unless it is already carrying him
        public void Grab(IPlayer link);

        // Lets go of him, once he has been taken away or has left the room
        public void Release();

    }
}
