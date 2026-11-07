using Audio;
using Interfaces;

namespace Commands
{
    internal sealed class ToggleMuteCommand : ICommand
    {
        public void Execute()
        {
            AudioManager.Instance.ToggleMute();
        }
    }
}