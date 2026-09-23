using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace CSE_3902_Project
{
    internal class KeyboardController : IController
    {

        private KeyboardState keyboardState;
        private Keys right;
        private Keys altRight;
        private Keys left;
        private Keys altLeft;
        private Keys down;
        private Keys altDown;
        private Keys up;
        private Keys altUp;
        public Vector2 movementDirection;

        public KeyboardController()
        {

            right = Keys.D;
            left = Keys.A;
            down = Keys.S;
            up = Keys.W;

            altRight = Keys.Right;
            altLeft = Keys.Left;
            altDown = Keys.Down;
            altUp = Keys.Up;

        }

        public KeyboardState GetKeys()
        {

            keyboardState = Keyboard.GetState();

            return keyboardState;

        }

        public Vector2 UpdateMovement()
        {

            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(right))
            {
                movementDirection.X = 1;
            }
            else if (keyboardState.IsKeyDown(left))
            {
                movementDirection.X = -1;
            }
            else if (keyboardState.IsKeyDown(down))
            {
                movementDirection.Y = 1;
            }
            else if (keyboardState.IsKeyDown(up))
            {
                movementDirection.Y = -1;
            }
            else
            {
                movementDirection = Vector2.Zero;
            }

            return movementDirection;

        }

        public void Update() { }
        public Vector2 MousePos() { return Vector2.Zero; }

    }
}
