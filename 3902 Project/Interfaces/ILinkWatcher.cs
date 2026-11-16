using Microsoft.Xna.Framework;

namespace Interfaces
{
    // Something in a room that reacts to where Link is, like a Blade Trap waiting for him to line up with it
    internal interface ILinkWatcher
    {

        // Told every frame, just before the room updates, where Link's hitbox is
        public void Watch(Rectangle link);

    }
}
