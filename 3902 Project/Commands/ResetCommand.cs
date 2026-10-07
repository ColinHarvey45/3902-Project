using CSE_3902_Project;
using Interfaces;

namespace Commands
{
    internal sealed class ResetCommand(Game1 game) : ICommand
    {

        public void Execute()
        {
            game.ResetGame();
        }

    }
}
