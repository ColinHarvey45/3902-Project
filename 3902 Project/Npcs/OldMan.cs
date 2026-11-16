using System.Linq;
using Audio;
using Colliders;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms;
using Sprites;
using UI;

namespace Npcs
{
    // Stands in place between the fires and, as Link comes in, writes out his hint across the top of the room a
    // letter at a time, as in the NES. Attacking him sets off the fires (see FireBlock)
    internal sealed class OldMan(Vector2 position) : INpc
    {
        // As the NES has it, spelling and all
        private static readonly string[] Message = ["EASTMOST PENNINSULA", "IS THE SECRET."];
        private static readonly int MessageLength = Message.Sum(line => line.Length);

        private const float LetterTime = 0.08f; // seconds

        // The first line a quarter of a tile below the top of the floor, the second a tile under it
        private static readonly float FirstLineY = RoomLayout.FloorOrigin.Y + RoomLayout.TileSize / 4f;
        private const float LineSpacing = RoomLayout.TileSize;

        private readonly ISprite sprite = NpcSpriteFactory.Instance.CreateOldManSprite();
        private readonly TextRenderer text = new();
        private int lettersShown;
        private float letterTimer;

        // He is one tile big, and Link can't walk through him
        public ICollider Collider { get; } =
            new BoxCollider(new Rectangle((int)position.X, (int)position.Y, RoomLayout.TileSize, RoomLayout.TileSize));

        // Only called while Link is in his room, so the message starts as Link comes in
        public void Update(GameTime gameTime)
        {
            if (lettersShown == MessageLength)
                return;

            letterTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (letterTimer < LetterTime)
                return;

            letterTimer = 0f;
            lettersShown++;
            AudioManager.Instance.Play(Sound.Text);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);

            // Each line stays where it will be when finished, so the letters don't shift as they appear
            int lettersLeft = lettersShown;
            for (int i = 0; i < Message.Length && lettersLeft > 0; i++)
            {
                string line = Message[i];
                int shown = System.Math.Min(lettersLeft, line.Length);
                float left = RoomLayout.Bounds.Center.X - TextRenderer.Width(line) / 2f;

                text.Draw(spriteBatch, line[..shown], new Vector2(left, FirstLineY + i * LineSpacing), Color.White);
                lettersLeft -= shown;
            }
        }

    }
}
