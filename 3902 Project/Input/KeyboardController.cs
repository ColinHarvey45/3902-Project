using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Enemies;
using Environment;

namespace Input
{
    internal class KeyboardController : IController
    {

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private readonly Keys right;
        private readonly Keys altRight;
        private readonly Keys left;
        private readonly Keys altLeft;
        private readonly Keys down;
        private readonly Keys altDown;
        private readonly Keys up;
        private readonly Keys altUp;
        private readonly Keys nextEnemy;
        private readonly Keys prevEnemy;
        private readonly Keys nextBlock;
        private readonly Keys prevBlock;
        private readonly Keys quit;
        private readonly Keys altQuit;
        private readonly Keys reset;
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

            nextEnemy = Keys.P;
            prevEnemy = Keys.O;

            nextBlock = Keys.Y;
            prevBlock = Keys.T;

            quit = Keys.Q;
            altQuit = Keys.Escape;
            reset = Keys.R;

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

        public int ChangeEnemy(int currentIndex, Enemy[] enemyArray)
        {

            if (currentKeyboardState.IsKeyDown(nextEnemy) && previousKeyboardState.IsKeyUp(nextEnemy))
            {

                enemyArray[currentIndex].SetVisibility(false);

                currentIndex++;
                if (currentIndex >= enemyArray.Length) currentIndex = 0;

                enemyArray[currentIndex].SetVisibility(true);
            }
            else if (currentKeyboardState.IsKeyDown(prevEnemy) && previousKeyboardState.IsKeyUp(prevEnemy))
            {

                enemyArray[currentIndex].SetVisibility(false);

                currentIndex--;
                if (currentIndex < 0) currentIndex = enemyArray.Length - 1;


                enemyArray[currentIndex].SetVisibility(true);
            }

            return currentIndex;
        }

        public int ChangeBlock(int currentIndex, Block[] blockArray)
        {

            if (currentKeyboardState.IsKeyDown(nextBlock) && previousKeyboardState.IsKeyUp(nextBlock))
            {

                blockArray[currentIndex].SetVisibility(false);

                currentIndex++;
                if (currentIndex >= blockArray.Length) currentIndex = 0;

                blockArray[currentIndex].SetVisibility(true);
            }
            else if (currentKeyboardState.IsKeyDown(prevBlock) && previousKeyboardState.IsKeyUp(prevBlock))
            {

                blockArray[currentIndex].SetVisibility(false);

                currentIndex--;
                if (currentIndex < 0) currentIndex = blockArray.Length - 1;


                blockArray[currentIndex].SetVisibility(true);
            }

            return currentIndex;
        }

        public bool QuitPressed()
        {
            return currentKeyboardState.IsKeyDown(quit) || currentKeyboardState.IsKeyDown(altQuit);
        }

        public bool ResetPressed()
        {
            return currentKeyboardState.IsKeyDown(reset) && previousKeyboardState.IsKeyUp(reset);
        }

        public void Update() { currentKeyboardState = Keyboard.GetState(); }
        public void PostUpdate() { previousKeyboardState = currentKeyboardState; }


        public Vector2 MousePos() { return Vector2.Zero; }

    }
}

