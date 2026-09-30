using CSE_3902_Project;
using Interfaces;

namespace Commands
{
    internal class PreviousEnemyCommand(Game1 game) : ICommand
    {

        public void Execute()
        {
            game.PreviousEnemy();
        }

    }
}
