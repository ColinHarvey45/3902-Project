using Commands;
using Input;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Player;
using UI;

namespace GameStates
{
    // The inventory screen, where Link chooses the item for his B slot while the game stands still: the arrow keys
    // move the highlight, G equips the highlighted item, and the inventory key goes back to playing
    internal sealed class ItemSelectionState : IGameState
    {
        private readonly PauseScreen pauseScreen;
        private readonly KeyboardController keyboard = new();

        public ItemSelectionState(GameStateMachine game, PauseScreen pauseScreen)
        {
            this.pauseScreen = pauseScreen;

            keyboard.RegisterCommand(Keys.F, new ResumeCommand(game));
            keyboard.RegisterCommand(Keys.Up, new PauseMoveSelectionCommand(pauseScreen, Direction.Up));
            keyboard.RegisterCommand(Keys.Down, new PauseMoveSelectionCommand(pauseScreen, Direction.Down));
            keyboard.RegisterCommand(Keys.Left, new PauseMoveSelectionCommand(pauseScreen, Direction.Left));
            keyboard.RegisterCommand(Keys.Right, new PauseMoveSelectionCommand(pauseScreen, Direction.Right));
            keyboard.RegisterCommand(Keys.G, new PauseEquipItemCommand(pauseScreen));
        }

        // The highlight starts on the item Link has equipped
        public void Enter()
        {
            pauseScreen.Open();
            keyboard.IgnoreHeldInput();
        }

        public void Update(GameTime gameTime)
        {
            keyboard.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pauseScreen.Draw(spriteBatch);
        }
    }
}
