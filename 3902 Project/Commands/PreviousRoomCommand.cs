using Interfaces;
using Rooms;

namespace Commands
{
    internal class PreviousRoomCommand(RoomManager rooms) : ICommand
    {
        public void Execute()
        {
            rooms.PreviousRoom();
        }
    }
}