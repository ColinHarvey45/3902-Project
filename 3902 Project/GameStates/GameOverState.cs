using Audio;
using Commands;
using Input;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rooms;
using UI;

namespace GameStates
{
    // Link has run out of hearts. The room stays on screen for a moment, turning red as in the NES, then the screen
    // goes black with GAME OVER, and the start key (or the reset key) plays the level again from the beginning
    internal sealed class GameOverState : IGameState
    {
        private const float DyingSeconds = 1.5f;
        private static readonly Color DyingTint = Color.Red * 0.5f;
        private static readonly string[] Message = ["GAME OVER", "PRESS ENTER TO PLAY AGAIN"];

        private readonly GameStateMachine game;
        private readonly KeyboardController keyboard = new();
        private float elapsed;

        public GameOverState(GameStateMachine game)
        {
            this.game = game;
            keyboard.RegisterCommand(Keys.Enter, new ResetCommand(game));
        }

        public void Enter()
        {
            keyboard.IgnoreHeldInput();
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.Play(Sound.LinkDie);
        }

        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            keyboard.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (elapsed < DyingSeconds)
            {
                game.DrawDungeon(spriteBatch);
                Solid.Fill(spriteBatch, RoomLayout.Bounds, DyingTint);
                return;
            }

            game.Hud.Draw(spriteBatch);
            game.Text.DrawMessage(spriteBatch, Message, Color.White);
        }
    }
}
