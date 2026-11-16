namespace Interfaces
{
    internal interface IController
    {

        // Reads the input device and runs the commands for whatever was pressed
        public void Update();

        // Reads the input device without running anything, so whatever is already held down when this controller
        // starts being used (like the key that switched the game to it) doesn't count as a new press
        public void IgnoreHeldInput();

    }
}
