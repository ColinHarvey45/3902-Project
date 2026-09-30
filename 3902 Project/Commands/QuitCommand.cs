using CSE_3902_Project;
using Interfaces;

namespace Commands
{
    internal class QuitCommand(Game1 game) : ICommand
    {

        public void Execute()
        {
            game.Exit();
        }

    }
}
