using Interfaces;
using UI;

namespace Commands
{
    // Makes the highlighted item on the pause screen the one Link uses
    internal sealed class PauseEquipItemCommand(PauseScreen pauseScreen) : ICommand
    {

        public void Execute()
        {
            pauseScreen.EquipSelected();
        }

    }
}
