using Microsoft.Xna.Framework;

namespace Interfaces
{
    // Whether Link is healthy or flashing from a hit; kept separate from ILinkState because
    // Link can still walk and attack while he is hurt
    internal interface ILinkHealthState
    {

        // The colour Link is drawn in this frame
        public Color Tint { get; }

        public void TakeDamage();
        public void Update(GameTime gameTime);

    }
}
