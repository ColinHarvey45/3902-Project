using Microsoft.Xna.Framework;
using System;


namespace Interfaces
{
    internal interface IController
    {

        public void Update();
        public Vector2 MousePos();

        public Vector2 UpdateMovement();

    }
}
