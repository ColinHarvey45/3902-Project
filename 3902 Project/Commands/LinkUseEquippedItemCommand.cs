using Interfaces;

namespace Commands
{
    // Uses the item in Link's B slot, which is chosen on the pause screen
    internal sealed class LinkUseEquippedItemCommand(IPlayer player) : ICommand
    {

        public void Execute()
        {
            player.UseItem(player.EquippedItem);
        }

    }
}
