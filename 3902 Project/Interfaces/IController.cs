using Microsoft.Xna.Framework;
using System;
using Enemies;
using Environment;


namespace Interfaces
{
    internal interface IController
    {

        public void Update();
        public void PostUpdate();
        public int ChangeEnemy(int enemyIndex, Enemy[] enemyArray);
        public int ChangeBlock(int blockIndex, Block[] blockArray);
        public Vector2 MousePos();

        public Vector2 UpdateMovement();

        public bool QuitPressed();
        public bool ResetPressed();

    }
}
