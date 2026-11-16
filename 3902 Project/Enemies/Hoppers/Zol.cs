using System.Collections.Generic;
using Audio;
using Interfaces;
using Microsoft.Xna.Framework;
using Rooms;
using Sprites;
using Enemies;

namespace Enemies.Hoppers
{
    // Like a Gel but bigger and slower: creeps about a tile at a time with longer pauses.
    // A blow that doesn't kill it outright (the sword's) splits it into two Gels, as in the NES
    internal sealed class Zol : HoppingEnemy, ISplittingEnemy
    {
        private const float ZolHopSpeed = 1f; // pixels per frame
        private const float ZolHopTime = 1f;
        private const float ZolMaxRestTime = 1.5f;
        private const int ZolHealth = 2;

        // The Gels land a quarter of a tile either side of where it was
        private static readonly Vector2 GelSpacing = new(RoomLayout.TileSize / 4f, 0);

        public Zol(Vector2 startPosition)
            : base(startPosition, ZolHopSpeed, ZolHopTime, ZolMaxRestTime)
        {
            Health = new EnemyHealth(ZolHealth);
            Sprite = EnemySpriteFactory.Instance.CreateZolSprite();
        }

        public IReadOnlyList<IEnemy> Pieces { get; private set; } = [];

        public override void TakeDamage(int damage, Vector2 hitFrom)
        {
            if (IsDead || IsInvincible || damage >= Health.HitPoints)
            {
                base.TakeDamage(damage, hitFrom);
                return;
            }

            Pieces = [new Gel(Position - GelSpacing, fromSplit: true), new Gel(Position + GelSpacing, fromSplit: true)];
            Health.Vanish();
            AudioManager.Instance.Play(Sound.EnemyHit);
        }
    }
}
