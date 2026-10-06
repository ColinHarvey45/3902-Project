using Interfaces;

namespace Commands
{
    internal class LinkTakeDamageCommand(IPlayer player) : ICommand
    {

        public void Execute()
        {
            player.TakeDamage();
        }

    }
}
