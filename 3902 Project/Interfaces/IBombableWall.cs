namespace Interfaces
{
    // A cracked piece of wall that looks like any other until a bomb goes off against it, when it becomes a hole Link
    // can walk through. The room on its other side has the same wall, which opens with it
    internal interface IBombableWall : IDoor
    {

        public bool IsOpen { get; }

        public void BlowOpen();

    }
}
