using Interfaces;
using Rooms;

namespace Commands
{
    internal sealed class PreviousRoomCommand(RoomManager rooms) : ICommand
    {
        public void Execute()
        {
            rooms.PreviousRoom();
        }
    }
}