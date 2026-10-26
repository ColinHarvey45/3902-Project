using CSE_3902_Project;
using Interfaces;
using Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectiles;

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
            projectiles.Update(gameTime, CurrentRoom.characters, link);

            // Projectiles were checked against Link and the enemies above; the rest of the collisions are here
            if (link is Link player)
            {
                // Link's sword hurts every enemy it touches; enemies that are dead, still flashing from a hit,
                // or have no collider are skipped
                if (player.SwordCollider is ICollider sword)
                {
                    foreach (ICharacter character in CurrentRoom.characters)
                    {
                        if (character is IDamageable hitEnemy && !hitEnemy.IsDead && !hitEnemy.IsInvincible
                            && hitEnemy.Collider != null && sword.Intersects(hitEnemy.Collider))
                            hitEnemy.TakeDamage(player.SwordDamage);
                    }
                }

                // Anything killed this frame is gone before it can touch Link
                CurrentRoom.RemoveDeadCharacters();

                // Touching an enemy hurts Link and knocks him back; while he is flashing from a hit, it does nothing
                foreach (ICharacter character in CurrentRoom.characters)
                {
                    if (character is IEnemy enemy && enemy.Collider != null && player.Collider.Intersects(enemy.Collider))
                        player.TakeDamage();
                }

                // Stop Link walking into solid blocks: push him back out the shortest way
                foreach (IBlock block in CurrentRoom.Blocks)
                {
                    if (!block.IsSolid) continue;

                    Rectangle overlap = player.Collider.OverlapWith(block.Collider);
                    if (overlap.IsEmpty) continue;

                    Rectangle linkBox = player.Collider.GetCollider();
                    Rectangle blockBox = block.Collider.GetCollider();

                    Vector2 push;
                    if (overlap.Width < overlap.Height)
                        // Overlapping less side-to-side, so push him left or right
                        push = new Vector2(linkBox.Center.X < blockBox.Center.X ? -overlap.Width : overlap.Width, 0);
                    else
                        // Otherwise push him up or down
                        push = new Vector2(0, linkBox.Center.Y < blockBox.Center.Y ? -overlap.Height : overlap.Height);

                    player.ApplyKnockback(push);
                }
            }
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
