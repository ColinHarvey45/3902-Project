using Interfaces;

namespace Commands
{
    internal sealed class LinkTakeDamageCommand(IPlayer player) : ICommand
    {

        public void Execute()
        {
            player.TakeDamage();
        }

    }
}
