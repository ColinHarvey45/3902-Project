using Enemies.States;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectiles;
using System;
using System.Collections.Generic;

namespace Enemies
{
    // The dungeon 1 boss: paces back and forth, and every so often opens its mouth to breathe
    // a spread of three fireballs. Its state decides whether its mouth is open
    internal class Aquamentus : IEnemy
    {
        private const float PaceSpeed = 0.5f; // pixels per frame
        private const float PaceDistance = 48f; // how far it walks either side of where it started
        private const float FireballSpeed = 150f; // pixels per second

        // Where the fireballs come out, relative to its top-left corner (it faces left)
        private static readonly Vector2 MouthOffset = new(0, 32);

        // In the real game the middle fireball aims at Link; until Sprint 3 they fly left in a fan
        private static readonly Vector2[] FireballDirections =
        [
            Vector2.Normalize(new Vector2(-1, -0.4f)),
            new Vector2(-1, 0),
            Vector2.Normalize(new Vector2(-1, 0.4f))
        ];

        private readonly float homeX;
        private readonly Rectangle screenBounds;
        private readonly List<IProjectile> fireballs = [];
        private float paceDirection = -1f;
        private IEnemyState state;

        public Vector2 Position { get; private set; }
        public ISprite Sprite { get; set; }

        public Aquamentus(Vector2 startPosition, Rectangle screenBounds)
        {
            Position = startPosition;
            homeX = startPosition.X;
            this.screenBounds = screenBounds;
            state = new AquamentusWalkingState(this);
        }

        public void ChangeState(IEnemyState newState)
        {
            state = newState;
        }

        public void Update(GameTime gameTime)
        {
            state.Update(gameTime);

            foreach (IProjectile fireball in fireballs)
                fireball.Update(gameTime);
            fireballs.RemoveAll(fireball => fireball.IsFinished);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Position);

            foreach (IProjectile fireball in fireballs)
                fireball.Draw(spriteBatch);
        }

        // Called by both states: it keeps walking whether or not its mouth is open
        public void Pace(GameTime gameTime)
        {
            Position += new Vector2(paceDirection * PaceSpeed, 0);

            if (Math.Abs(Position.X - homeX) >= PaceDistance)
                paceDirection = -paceDirection;

            Sprite.Update(gameTime);
        }

        // Called by the attacking state when its mouth opens
        public void BreatheFireballs()
        {
            Vector2 mouth = Position + MouthOffset;

            foreach (Vector2 direction in FireballDirections)
                fireballs.Add(new Fireball(mouth, direction * FireballSpeed, screenBounds));
        }
    }
}
