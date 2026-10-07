using Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Enemies.States
{
    // Speeds up after taking off, flutters at full speed, then slows down to land
    internal class KeeseFlyingState(Keese keese) : IEnemyState
    {
        private const float FlightTime = 3f;
        private const float RampTime = 1f; // time spent speeding up after take-off, and slowing down before landing
        private const float TopSpeed = 2f; // pixels per frame

        private float timer = 0f;

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            float speedingUp = timer / RampTime;
            float slowingDown = (FlightTime - timer) / RampTime;
            float speed = TopSpeed * MathHelper.Clamp(Math.Min(speedingUp, slowingDown), 0f, 1f);

            keese.Fly(gameTime, speed);

            if (timer >= FlightTime)
                keese.ChangeState(new KeeseRestingState(keese));
        }
    }
}
