using GameStates;
using Interfaces;

namespace Commands
{
    // Carries on playing after the game was paused, or after choosing an item on the inventory screen
    internal sealed class ResumeCommand(GameStateMachine game) : ICommand
    {

        public void Execute()
        {
            game.Play();
        }

    }
}
