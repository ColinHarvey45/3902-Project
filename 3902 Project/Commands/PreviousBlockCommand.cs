using CSE_3902_Project;
using Interfaces;

namespace Commands
{
    internal sealed class PreviousBlockCommand(Game1 game) : ICommand
    {

        public void Execute()
        {
            game.PreviousBlock();
        }

    }
}
