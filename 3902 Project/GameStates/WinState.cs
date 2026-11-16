using Commands;
using Input;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameStates
{
    // Link has the triforce: everything stops with the room on screen and a message over it, and the start key
    // (or the reset key) plays the level again from the beginning
    internal sealed class WinState : IGameState
    {
        private static readonly string[] Message = ["YOU GOT THE TRIFORCE!", "PRESS ENTER TO PLAY AGAIN", "OR Q TO QUIT"];

        private readonly GameStateMachine game;
        private readonly KeyboardController keyboard = new();

        public WinState(GameStateMachine game)
        {
            this.game = game;
            keyboard.RegisterCommand(Keys.Enter, new ResetCommand(game));
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
