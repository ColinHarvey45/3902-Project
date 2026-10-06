using Interfaces;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Input
{
    internal class KeyboardController : IController
    {

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        // Run once each time their key goes down
        private readonly Dictionary<Keys, ICommand> pressCommands = [];

        // Run every frame their key is held, in the order they were registered
        private readonly List<KeyValuePair<Keys, ICommand>> heldCommands = [];

        public void RegisterCommand(Keys key, ICommand command)
        {
            pressCommands[key] = command;
        }

        // Registering a key again replaces its command but keeps its place in the order
        public void RegisterHeldCommand(Keys key, ICommand command)
        {
            KeyValuePair<Keys, ICommand> binding = new(key, command);
            int index = heldCommands.FindIndex(existing => existing.Key == key);

            if (index >= 0)
                heldCommands[index] = binding;
            else
                heldCommands.Add(binding);
        }

        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            foreach (Keys key in currentKeyboardState.GetPressedKeys())
            {
                if (previousKeyboardState.IsKeyUp(key) && pressCommands.TryGetValue(key, out ICommand command))
                {
                    command.Execute();
                }
            }

            foreach (KeyValuePair<Keys, ICommand> binding in heldCommands)
            {
                if (currentKeyboardState.IsKeyDown(binding.Key))
                {
                    binding.Value.Execute();
                }
            }
        }

    }
}
