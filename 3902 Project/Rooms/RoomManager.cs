using CSE_3902_Project;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rooms
{
    // Holds every room in the dungeon and knows which one Link is in. Link goes wherever the
    // current room is, so he is updated and drawn along with it
    internal sealed class RoomManager
    {
        private readonly IPlayer link;
        private readonly CyclingList<Room> rooms;

        public Room CurrentRoom => rooms.Current;

        // Link starts in the first room
        public RoomManager(IPlayer link, Room[] rooms)
        {
            this.link = link;
            this.rooms = new CyclingList<Room>(rooms);
        }

        public void NextRoom()
        {
            rooms.Next();
        }

        public void PreviousRoom()
        {
            rooms.Previous();
        }

        public void Update(GameTime gameTime)
        {
            link.Update(gameTime);
            CurrentRoom.Update(gameTime);
        }

        // The room first, then Link on top of it
        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentRoom.Draw(spriteBatch);
            link.Draw(spriteBatch);
        }
    }
}
