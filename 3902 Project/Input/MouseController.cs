using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    // Runs a command once each time the left or right mouse button is clicked inside the game window
    internal sealed class MouseController(Rectangle window) : IController
    {

        private MouseState currentMouseState;
        private MouseState previousMouseState;
        private ICommand leftClickCommand;
        private ICommand rightClickCommand;

        public void RegisterLeftClick(ICommand command)
        {
            leftClickCommand = command;
        }

        public void RegisterRightClick(ICommand command)
        {
            rightClickCommand = command;
        }

        public void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (!window.Contains(currentMouseState.Position))
                return;

            if (WasClicked(currentMouseState.LeftButton, previousMouseState.LeftButton))
                leftClickCommand?.Execute();

            if (WasClicked(currentMouseState.RightButton, previousMouseState.RightButton))
                rightClickCommand?.Execute();
        }

        // True only on the frame the button goes down, so holding it doesn't repeat the command
        private static bool WasClicked(ButtonState now, ButtonState before)
        {
            return now == ButtonState.Pressed && before == ButtonState.Released;
        }

    }
}
