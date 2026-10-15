using Interfaces;
using Rooms;

namespace Commands
{
    internal class NextRoomCommand(RoomManager rooms) : ICommand
    {
        public void Execute()
        {
            rooms.NextRoom();
        }
    }
}