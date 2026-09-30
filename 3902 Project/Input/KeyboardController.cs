using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Input
{
    internal class KeyboardController : IController
    {

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private readonly Dictionary<Keys, ICommand> commands = [];
        private readonly Keys right;
        private readonly Keys altRight;
        private readonly Keys left;
        private readonly Keys altLeft;
        private readonly Keys down;
        private readonly Keys altDown;
        private readonly Keys up;
        private readonly Keys altUp;
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

        // The command runs once each time the key is pressed, not every frame it is held
        public void RegisterCommand(Keys key, ICommand command)
        {
            commands[key] = command;
        }

        public Vector2 UpdateMovement()
        {

                movementDirection = Vector2.Zero;

                if (currentKeyboardState.IsKeyDown(right) || currentKeyboardState.IsKeyDown(altRight))
                    movementDirection.X = 1;
                else if (currentKeyboardState.IsKeyDown(left) || currentKeyboardState.IsKeyDown(altLeft))
                    movementDirection.X = -1;

                if (currentKeyboardState.IsKeyDown(down) || currentKeyboardState.IsKeyDown(altDown))
                 movementDirection.Y = 1;
                else if (currentKeyboardState.IsKeyDown(up) || currentKeyboardState.IsKeyDown(altUp))
                    movementDirection.Y = -1;

            return movementDirection;
        }

        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            foreach (Keys key in currentKeyboardState.GetPressedKeys())
            {
                if (previousKeyboardState.IsKeyUp(key) && commands.TryGetValue(key, out ICommand command))
                {
                    command.Execute();
                }
            }
        }


        public Vector2 MousePos() { return Vector2.Zero; }

    }
}

