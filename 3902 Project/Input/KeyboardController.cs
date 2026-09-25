using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Input
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

        movementDirection = Vector2.Zero;

        if (keyboardState.IsKeyDown(right) || keyboardState.IsKeyDown(altRight))
            movementDirection.X = 1;
        else if (keyboardState.IsKeyDown(left) || keyboardState.IsKeyDown(altLeft))
            movementDirection.X = -1;

        if (keyboardState.IsKeyDown(down) || keyboardState.IsKeyDown(altDown))
         movementDirection.Y = 1;
        else if (keyboardState.IsKeyDown(up) || keyboardState.IsKeyDown(altUp))
            movementDirection.Y = -1;

    return movementDirection;
}

        public void Update() { }
        public Vector2 MousePos() { return Vector2.Zero; }

    }
}

