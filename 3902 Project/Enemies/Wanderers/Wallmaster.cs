using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Rooms;
using Sprites;

namespace Enemies.Wanderers
{
    // A hand that lives in the walls, as in the NES: the room looks empty until Link walks along a wall, when one
    // comes out of the wall a little ahead of him, creeps along it past where he is, and goes back in. Touching
    // Link doesn't hurt him: it catches him and drags him off towards the wall, and he is taken back to the
    // entrance of the dungeon (see RoomManager). Where its line in the room file puts it doesn't matter
    internal sealed class Wallmaster : WanderingEnemy, ILinkGrabber, ILinkWatcher, IHidingEnemy
    {
        // Three sword hits, as in the NES
        private const int WallmasterHealth = 3;

        // How long it drags Link before he is taken away, and how fast
        private const float CarryTime = 1f; // seconds
        private const float CarrySpeed = 1f; // pixels per frame

        // How fast it creeps out of the wall and along it
        private const float CreepSpeed = 2f; // pixels per frame

        // It comes out this many tiles along the wall from Link, and creeps this many tiles along it, past him
        private const int TilesAhead = 2;
        private const int TilesAlong = 4;

        // Link is by a wall when he is less than half a tile from it
        private const int NearWall = RoomLayout.TileSize / 2;

        // How long it waits in the wall, when Link comes into the room and after each time it comes out
        private const float ShortestWait = 1f; // seconds
        private const float LongestWait = 3f;

        private readonly ICollider hitbox;

        private bool inWall = true;
        private float waitLeft;
        private Rectangle link;

        // Where it goes once out of the wall: out, along the wall, and back in
        private readonly List<Vector2> path = [];
        private int nextOnPath;

        private IPlayer heldLink;
        private Vector2 carryDirection;
        private float carryTimeLeft;

        public Wallmaster(Vector2 startPosition)
            : base(startPosition)
        {
            Health = new EnemyHealth(WallmasterHealth);
            Sprite = EnemySpriteFactory.Instance.CreateWallmasterSprite();
            hitbox = Collider;
            GoBackIntoTheWall();
        }

        public bool IsHidden => inWall;
        public bool IsReadyToComeOut => inWall && waitLeft <= 0 && WallBesideLink() != null;
        public bool IsHoldingLink => heldLink != null;
        public bool HasCarriedLinkOff => IsHoldingLink && carryTimeLeft <= 0;

        public void Watch(Rectangle linkHitbox)
        {
            link = linkHitbox;
        }

        // Out of the wall beside Link, on whichever side of him there is room along it (or either, at random)
        public void ComeOut()
        {
            if (WallBesideLink() is not Direction wall)
                return;

            Rectangle floor = RoomLayout.FloorBounds;
            bool alongX = wall is Direction.Up or Direction.Down;
            float lowest = alongX ? floor.Left : floor.Top;
            float highest = (alongX ? floor.Right : floor.Bottom) - RoomLayout.TileSize;
            float linkAt = (alongX ? link.Center.X : link.Center.Y) - RoomLayout.TileSize / 2f;

            int way = RandomGenerator.Next(2) == 0 ? 1 : -1;
            float ahead = linkAt + way * TilesAhead * RoomLayout.TileSize;
            if (ahead < lowest || ahead > highest)
                way = -way;

            float start = MathHelper.Clamp(linkAt + way * TilesAhead * RoomLayout.TileSize, lowest, highest);
            float end = MathHelper.Clamp(start - way * TilesAlong * RoomLayout.TileSize, lowest, highest);

            // The tile in the wall, and the floor tile just inside it, at a point along the wall
            float edge = wall switch
            {
                Direction.Up => floor.Top,
                Direction.Down => floor.Bottom - RoomLayout.TileSize,
                Direction.Left => floor.Left,
                _ => floor.Right - RoomLayout.TileSize
            };
            Vector2 outwards = wall.ToVector() * RoomLayout.TileSize;
            Vector2 OnFloor(float along) => alongX ? new Vector2(along, edge) : new Vector2(edge, along);

            path.Clear();
            path.AddRange([OnFloor(start), OnFloor(end), OnFloor(end) + outwards]);
            nextOnPath = 0;
            Position = OnFloor(start) + outwards;
            inWall = false;
            Collider = hitbox;
            UpdateCollider();
        }

        // Nothing can reach it in the wall
        public override void TakeDamage(int damage, Vector2 hitFrom)
        {
            if (!inWall)
                base.TakeDamage(damage, hitFrom);
        }

        public void Grab(IPlayer player)
        {
            if (heldLink != null)
                return;

            heldLink = player;
            carryTimeLeft = CarryTime;
            carryDirection = TowardsNearestWall();
        }

        // Lets go of Link, if it has him, and goes back into the wall: he has been taken away, or has left the room
        public void Release()
        {
            heldLink = null;
            GoBackIntoTheWall();
        }

        // Holding Link, it drags him with it; out of the wall, it creeps along its path; in the wall, it waits
        protected override void UpdateBehavior(GameTime gameTime)
        {
            if (heldLink != null)
                Carry(gameTime);
            else if (!inWall)
                Creep(gameTime);
            else
                waitLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void Creep(GameTime gameTime)
        {
            Vector2 toNext = path[nextOnPath] - Position;
            if (toNext.Length() <= CreepSpeed)
            {
                Position = path[nextOnPath];
                nextOnPath++;
                if (nextOnPath == path.Count)
                {
                    GoBackIntoTheWall();
                    return;
                }
            }
            else
            {
                Position += Vector2.Normalize(toNext) * CreepSpeed;
            }

            UpdateCollider();
            Sprite.Update(gameTime);
        }

        // It drags him as far as the edge of the floor, and no further (not out into a doorway)
        private void Carry(GameTime gameTime)
        {
            carryTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 next = Position + carryDirection * CarrySpeed;
            if (Movement.Allows(next))
                Position = next;
            UpdateCollider();
            Sprite.Update(gameTime);
            heldLink.PlaceAt(Position);
        }

        private void GoBackIntoTheWall()
        {
            inWall = true;
            Collider = null;
            waitLeft = ShortestWait + (float)RandomGenerator.NextDouble() * (LongestWait - ShortestWait);
        }

        // The wall Link is walking along, if he is close enough to one
        private Direction? WallBesideLink()
        {
            if (link.IsEmpty)
                return null;

            Rectangle floor = RoomLayout.FloorBounds;
            if (link.Top - floor.Top < NearWall) return Direction.Up;
            if (floor.Bottom - link.Bottom < NearWall) return Direction.Down;
            if (link.Left - floor.Left < NearWall) return Direction.Left;
            if (floor.Right - link.Right < NearWall) return Direction.Right;
            return null;
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

        // Walls don't stop it: it comes out of them. Dragging Link, it stops at a block
        public override void OnBlocked(Vector2 push)
        {
            if (heldLink == null)
                return;

            Position += push;
            UpdateCollider();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!inWall)
                base.Draw(spriteBatch);
        }
    }
}
