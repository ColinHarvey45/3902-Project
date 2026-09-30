using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Interfaces;

namespace Animation
{
    internal class Link : ISprite
    {
        private enum Direction { Up, Down, Left, Right }
        private enum EquippedWeapon { None, Sword, Bow }

        private static readonly EquippedWeapon[] WeaponOrder = { EquippedWeapon.None, EquippedWeapon.Sword, EquippedWeapon.Bow };
        private int weaponIndex = 1; // starts on Sword
        private EquippedWeapon CurrentWeapon => WeaponOrder[weaponIndex];

        private Direction currentDirection = Direction.Down;
        private bool isAttacking = false;

        // --- Movement sprites ---
        private readonly AnimatedSprite downSprite;
        private readonly AnimatedSprite upSprite;
        private readonly AnimatedSprite horizontalSprite;

        // --- Sword swing sprites ---
        private readonly SwordSwing swordDown;
        private readonly SwordSwing swordUp;
        private readonly SwordSwing swordHorizontal;

        // --- Arrows in flight ---
        private readonly Texture2D texture;
        private readonly SpriteBatch spriteBatch;
        private readonly List<Arrow> arrows = new List<Arrow>();
        private const int ScreenWidth = 1280;  // adjust to match your actual resolution, or pass it in
        private const int ScreenHeight = 720;

        private readonly IController controller;
        private KeyboardState previousKeyboardState;

        private const float WalkFrameSpeed = 0.15f;
        private const float SwingFrameSpeed = 0.06f;
        private const float MoveSpeed = 3f;

        public Link(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition, IController controller)
        {
            this.texture = texture;
            this.spriteBatch = spriteBatch;
            this.controller = controller;

            downSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(1, 1), 2, WalkFrameSpeed);
            upSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(71, 1), 2, WalkFrameSpeed);
            horizontalSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(33, 1), 2, WalkFrameSpeed);

            swordDown = new SwordSwing(texture, spriteBatch, startPosition,
                new[] { new Rectangle(1, 37, 16, 15), new Rectangle(18, 37, 16, 27), new Rectangle(35, 37, 15, 23), new Rectangle(53, 37, 13, 19) },
                new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero },
                SwingFrameSpeed);

            swordUp = new SwordSwing(texture, spriteBatch, startPosition,
                new[] { new Rectangle(1, 99, 16, 16), new Rectangle(18, 87, 16, 28), new Rectangle(37, 88, 12, 27), new Rectangle(54, 96, 12, 19) },
                new[] { Vector2.Zero, new Vector2(0, -12), new Vector2(0, -11), new Vector2(0, -3) },
                SwingFrameSpeed);

            swordHorizontal = new SwordSwing(texture, spriteBatch, startPosition,
                new[] { new Rectangle(1, 68, 15, 15), new Rectangle(18, 68, 27, 15), new Rectangle(46, 68, 23, 15), new Rectangle(70, 67, 19, 16) },
                new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero },
                SwingFrameSpeed);
        }

        public void Update(GameTime gameTime)
        {
            controller.Update();
            Vector2 movement = controller.UpdateMovement() * MoveSpeed;

            KeyboardState keyboardState = Keyboard.GetState();
            bool attackPressed = keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space);
            bool nextWeaponPressed = keyboardState.IsKeyDown(Keys.I) && previousKeyboardState.IsKeyUp(Keys.I);
            bool prevWeaponPressed = keyboardState.IsKeyDown(Keys.U) && previousKeyboardState.IsKeyUp(Keys.U);
            previousKeyboardState = keyboardState;

            if (!isAttacking)
            {
                if (nextWeaponPressed)
                    weaponIndex = (weaponIndex + 1) % WeaponOrder.Length;
                else if (prevWeaponPressed)
                    weaponIndex = (weaponIndex - 1 + WeaponOrder.Length) % WeaponOrder.Length;
            }

            if (isAttacking)
            {
                ActiveSword.Update(gameTime);
                if (!ActiveSword.IsPlaying)
                    isAttacking = false;
            }
            else
            {
                if (movement.Y < 0) currentDirection = Direction.Up;
                else if (movement.Y > 0) currentDirection = Direction.Down;
                else if (movement.X < 0) currentDirection = Direction.Left;
                else if (movement.X > 0) currentDirection = Direction.Right;

                if (attackPressed && CurrentWeapon == EquippedWeapon.Sword)
                {
                    isAttacking = true;
                    ActiveSword.Play();
                }
                else
                {
                    ActiveSprite.UpdateAnimation(gameTime, movement);

                    if (attackPressed && CurrentWeapon == EquippedWeapon.Bow)
                    {
                        arrows.Add(new Arrow(texture, spriteBatch, ActiveSprite.Position, DirectionVector));
                    }
                }
            }

            foreach (var arrow in arrows)
                arrow.Update(gameTime, ScreenWidth, ScreenHeight);
            arrows.RemoveAll(a => !a.Active);

            Vector2 currentPos = isAttacking ? ActiveSword.Position : ActiveSprite.Position;
            downSprite.Position = currentPos;
            upSprite.Position = currentPos;
            horizontalSprite.Position = currentPos;
            swordDown.Position = currentPos;
            swordUp.Position = currentPos;
            swordHorizontal.Position = currentPos;

            bool facingLeft = currentDirection == Direction.Left;
            horizontalSprite.SetEffects(facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            swordHorizontal.SetEffects(facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }

        private Vector2 DirectionVector => currentDirection switch
        {
            Direction.Up => new Vector2(0, -1),
            Direction.Down => new Vector2(0, 1),
            Direction.Left => new Vector2(-1, 0),
            _ => new Vector2(1, 0)
        };

        private AnimatedSprite ActiveSprite => currentDirection switch
        {
            Direction.Up => upSprite,
            Direction.Down => downSprite,
            _ => horizontalSprite
        };

        private SwordSwing ActiveSword => currentDirection switch
        {
            Direction.Up => swordUp,
            Direction.Down => swordDown,
            _ => swordHorizontal
        };

        public void Draw(Texture2D texture)
        {
            if (isAttacking)
                ActiveSword.Draw(texture);
            else
                ActiveSprite.Draw(texture);

            foreach (var arrow in arrows)
                arrow.Draw(texture);
        }
    }
}