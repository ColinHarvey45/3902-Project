using Interfaces;
using Microsoft.Xna.Framework;
using Rooms;
using Sprites;
using Enemies;

namespace Enemies.Wanderers
{
    // A hand that wanders the room. Touching Link doesn't hurt him: it catches him and drags him off towards the
    // nearest wall, and he is taken back to the entrance of the dungeon, as in the NES (see RoomManager)
    internal sealed class Wallmaster : WanderingEnemy, ILinkGrabber
    {
        // Three sword hits, as in the NES
        private const int WallmasterHealth = 3;

        // How long it drags Link before he is taken away, and how fast
        private const float CarryTime = 1f; // seconds
        private const float CarrySpeed = 1f; // pixels per frame

        private IPlayer heldLink;
        private Vector2 carryDirection;
        private float carryTimeLeft;

        public Wallmaster(Vector2 startPosition)
            : base(startPosition)
        {
            Health = new EnemyHealth(WallmasterHealth);
            Sprite = EnemySpriteFactory.Instance.CreateWallmasterSprite();
        }

        public bool IsHoldingLink => heldLink != null;
        public bool HasCarriedLinkOff => IsHoldingLink && carryTimeLeft <= 0;

        public void Grab(IPlayer link)
        {
            if (heldLink != null)
                return;

            heldLink = link;
            carryTimeLeft = CarryTime;
            carryDirection = TowardsNearestWall();
        }

        public void Release()
        {
            heldLink = null;
        }

        // Holding Link, it drags him with it; otherwise it wanders like other enemies
        protected override void UpdateBehavior(GameTime gameTime)
        {
            if (heldLink == null)
            {
                base.UpdateBehavior(gameTime);
                return;
            }

            // It drags him as far as the edge of the floor, and no further (not out into a doorway)
            carryTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 next = Position + carryDirection * CarrySpeed;
            if (Movement.Allows(next))
                Position = next;
            UpdateCollider();
            Sprite.Update(gameTime);
            heldLink.PlaceAt(Position);
        }

        // Straight out to whichever wall of the room is closest
        private Vector2 TowardsNearestWall()
        {
            Rectangle floor = RoomLayout.FloorBounds;
            Point middle = Collider.GetCollider().Center;
            int left = middle.X - floor.Left, right = floor.Right - middle.X;
            int up = middle.Y - floor.Top, down = floor.Bottom - middle.Y;
            int nearest = System.Math.Min(System.Math.Min(left, right), System.Math.Min(up, down));

            if (nearest == left) return new Vector2(-1, 0);
            if (nearest == right) return new Vector2(1, 0);
            return nearest == up ? new Vector2(0, -1) : new Vector2(0, 1);
        }

        // Dragging Link into a wall, it stays there until he is taken away
        public override void OnBlocked(Vector2 push)
        {
            if (heldLink == null)
                base.OnBlocked(push);
            else
            {
                Position += push;
                UpdateCollider();
            }
        }
    }
}
