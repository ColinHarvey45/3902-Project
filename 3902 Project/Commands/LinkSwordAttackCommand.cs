using Interfaces;

namespace Commands
{
    internal class LinkSwordAttackCommand(IPlayer player) : ICommand
    {

        public void Execute()
        {
            player.SwordAttack();
        }

    }
}
