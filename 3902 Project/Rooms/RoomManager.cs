using CSE_3902_Project;
using Interfaces;
using Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectiles;
using Colliders;

namespace Rooms
{
    // Holds every room in the dungeon and knows which one Link is in. Link goes wherever the
    // current room is, so he is updated and drawn along with it, as is everything fired in it
    internal sealed class RoomManager
    {
        private readonly IPlayer link;
        private readonly CyclingList<Room> rooms;
        private readonly ProjectileManager projectiles;

        public Room CurrentRoom => rooms.Current;

        // Link starts in the first room. projectiles is shared with Link and the enemies, who fire into it
        public RoomManager(IPlayer link, Room[] rooms, ProjectileManager projectiles)
        {
            this.link = link;
            this.rooms = new CyclingList<Room>(rooms);
            this.projectiles = projectiles;
        }

        public void NextRoom()
        {
            rooms.Next();
            OnRoomChanged();
        }

        public void PreviousRoom()
        {
            rooms.Previous();
            OnRoomChanged();
        }

        public void Update(GameTime gameTime)
        {
            link.Update(gameTime);
            CurrentRoom.Update(gameTime);
            // Checks collision between Link and any enemies in the current room.
            // If Link collides with an enemy, put him into the damaged state and knock him back.
            if (link is Link player)
            {
                foreach (ICharacter character in CurrentRoom.characters)
                {
                    if (character is Enemies.Enemy enemy && player.collider != null && enemy.collider != null)
                    {
                        Rectangle overlap = player.collider.Overlap(player.collider.GetCollider(), enemy.collider.GetCollider());
                        if (!overlap.IsEmpty)
                        {
                            // Hurt Link and push him backwards opposite his facing direction
                            player.TakeDamage();
                            Vector2 knockback = -player.Facing.ToVector() * 50f;
                            player.ApplyKnockback(knockback);
                        }
                    }
                }
            }
            projectiles.Update(gameTime);
        }

        // Back to front: the room, then Link, then anything flying over them
        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentRoom.Draw(spriteBatch);
            link.Draw(spriteBatch);
            projectiles.Draw(spriteBatch);
        }

        // Nothing fired in the old room follows Link into the new one
        private void OnRoomChanged()
        {
            projectiles.Clear();
        }
    }
}
