using Audio;
using Colliders;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Rooms;
using Sprites;

namespace Environment
{
    // Looks like any other square block, but slides one tile when Link pushes against it for half a second, once
    // only, as in the NES; the secret sound plays when it stops. onlyWay, if given, is the one way it can be
    // pushed, like the block in room 15, which only goes up. In room 15 it opens the way to the stairs, and in
    // room 8 it opens the shutter door (see Room). As in the NES, it can't be pushed until the room's enemies
    // are dead (see MovementCollisions)
    internal sealed class PushableBlock : IPushableBlock
    {
        private const int FramesToPush = 30; // half a second
        private const float SlideSpeed = 2f; // pixels per frame, so the tile takes half a second

        private readonly ISprite sprite = BlockSpriteFactory.Instance.CreateSquareBlockSprite();
        private Vector2 position;

        // How long Link has been pushing it, and which way; it has to be the same way every frame
        private Direction pushDirection;
        private int framesPushed;
        private bool pushedSinceLastUpdate;

        private bool hasStartedSliding;
        private Vector2 slideLeft;

        private readonly Direction? onlyWay;

        public PushableBlock(Vector2 position, Direction? onlyWay = null)
        {
            this.position = position;
            this.onlyWay = onlyWay;
            Collider = new BoxCollider(new Rectangle((int)position.X, (int)position.Y, RoomLayout.TileSize, RoomLayout.TileSize));
        }

        public ICollider Collider { get; }
        public bool IsSolid => true;
        public bool IsClimbable => false;
        public bool HasMoved => hasStartedSliding && slideLeft == Vector2.Zero;

        public bool PushedBy(Direction direction)
        {
            if (hasStartedSliding || (onlyWay != null && direction != onlyWay))
                return false;

            if (framesPushed == 0 || direction != pushDirection)
            {
                pushDirection = direction;
                framesPushed = 0;
            }

            pushedSinceLastUpdate = true;
            framesPushed++;
            return framesPushed >= FramesToPush;
        }

        public Rectangle Destination(Direction direction)
        {
            Rectangle tile = Collider.GetCollider();
            tile.Offset((direction.ToVector() * RoomLayout.TileSize).ToPoint());
            return tile;
        }

        public void Slide(Direction direction)
        {
            hasStartedSliding = true;
            slideLeft = direction.ToVector() * RoomLayout.TileSize;
        }

        // Link has to keep pushing: a frame without a push starts the count again
        public void Update(GameTime gameTime)
        {
            if (!pushedSinceLastUpdate)
                framesPushed = 0;
            pushedSinceLastUpdate = false;

            if (slideLeft == Vector2.Zero)
                return;

            Vector2 step = slideLeft.Length() <= SlideSpeed ? slideLeft : Vector2.Normalize(slideLeft) * SlideSpeed;
            position += step;
            slideLeft -= step;
            Collider.Update(position);

            if (slideLeft == Vector2.Zero)
                AudioManager.Instance.Play(Sound.Secret);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }
    }
}
