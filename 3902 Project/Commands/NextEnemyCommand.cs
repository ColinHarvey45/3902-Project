using CSE_3902_Project;
using Interfaces;

namespace Commands
{
    internal sealed class NextEnemyCommand(Game1 game) : ICommand
    {

        public void Execute()
        {
            game.NextEnemy();
        }

    }
}
