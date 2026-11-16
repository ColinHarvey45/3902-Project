namespace Interfaces
{
    // A door that stays shut until the room lets it open, e.g. once every enemy in the room is gone
    internal interface IShutterDoor : IDoor
    {

        public bool IsOpen { get; }

        public void Open();

    }
}
