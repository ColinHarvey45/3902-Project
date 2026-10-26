using Interfaces;
using Rooms;

namespace Commands
{
    internal sealed class NextRoomCommand(RoomManager rooms) : ICommand
    {
        public void Execute()
        {
            rooms.NextRoom();
        }
    }
}