using Microsoft.Xna.Framework;
using System;


namespace CSE_3902_Project
{
    internal interface IController
    {

        public void Update();
        public Vector2 MousePos();

        public Vector2 UpdateMovement();

    }
}
