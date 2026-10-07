using Interfaces;

namespace Commands
{
    internal sealed class LinkSwordAttackCommand(IPlayer player) : ICommand
    {

        public void Execute()
        {
            player.SwordAttack();
        }

    }
}
