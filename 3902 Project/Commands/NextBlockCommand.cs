using CSE_3902_Project;
using Interfaces;

namespace Commands
{
    internal class NextBlockCommand(Game1 game) : ICommand
    {

        public void Execute()
        {
            game.NextBlock();
        }

    }
}
