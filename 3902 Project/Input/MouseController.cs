using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Enemies;
using Environment;


namespace Input
{
    internal class MouseController : IController
    {
        private MouseState mouseState;
        private Vector2 mousePos;

        public MouseController()
        {
            mousePos = new Vector2(mouseState.X, mouseState.Y);
        }

        public Vector2 MousePos()
        {
            mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                mousePos = new Vector2(mouseState.X, mouseState.Y);

            }

            return mousePos;

        }

        // Mouse not used for movement
        public Vector2 UpdateMovement() { return Vector2.Zero; }
        public void Update() { }
        public void PostUpdate() { }
        public int ChangeEnemy(int enemyIndex, Enemy[] enemyArray) {  return enemyIndex; }
        public int ChangeBlock(int blockIndex, Block[] blockArray) {  return blockIndex; }
        public bool QuitPressed() { return false; }
        public bool ResetPressed() { return false; }

    }
}
