using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Interfaces;

namespace Animation
{
    internal class Link : ISprite
    {
        private enum Direction { Up, Down, Left, Right }

        private Direction currentDirection = Direction.Down;

        private readonly AnimatedSprite downSprite;
        private readonly AnimatedSprite upSprite;
        private readonly AnimatedSprite horizontalSprite; // shared by Left/Right, flipped as needed

        private readonly IController controller;

        // how long each walk frame is held before swapping - tweak to taste
        private const float WalkFrameSpeed = 0.15f;

        public Link(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition, IController controller)
        {
            this.controller = controller;

            downSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(1, 1), 2, WalkFrameSpeed);
            upSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(71, 1), 2, WalkFrameSpeed);
            horizontalSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(33, 1), 2, WalkFrameSpeed);
        }

        public void Update(GameTime gameTime)
        {
            controller.Update();
            Vector2 movement = controller.UpdateMovement();

            if (movement.Y < 0) currentDirection = Direction.Up;
            else if (movement.Y > 0) currentDirection = Direction.Down;
            else if (movement.X < 0) currentDirection = Direction.Left;
            else if (movement.X > 0) currentDirection = Direction.Right;

            Vector2 currentPos = ActiveSprite.Position;
            downSprite.Position = currentPos;
            upSprite.Position = currentPos;
            horizontalSprite.Position = currentPos;

            horizontalSprite.SetEffects(currentDirection == Direction.Left
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None);

            ActiveSprite.UpdateAnimation(gameTime, movement);
        }

        private AnimatedSprite ActiveSprite => currentDirection switch
        {
            Direction.Up => upSprite,
            Direction.Down => downSprite,
            _ => horizontalSprite
        };

        public void Draw(Texture2D texture)
        {
            ActiveSprite.Draw(texture);
        }
    }
}
