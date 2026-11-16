using Commands;
using Input;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameStates
{
    // The game stands still with PAUSED over the room until the pause key is pressed again
    internal sealed class PausedState : IGameState
    {
        private static readonly string[] Message = ["PAUSED"];

        private readonly GameStateMachine game;
        private readonly KeyboardController keyboard = new();

        public PausedState(GameStateMachine game)
        {
            this.game = game;
            keyboard.RegisterCommand(Keys.Space, new ResumeCommand(game));
        }

        public void Enter()
        {
            keyboard.IgnoreHeldInput();
        }

        public void Update(GameTime gameTime)
        {
            keyboard.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            game.DrawDungeon(spriteBatch);
            game.Text.DrawMessage(spriteBatch, Message, Color.White);
        }
    }
}
