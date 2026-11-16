using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    // A door piece in the gap in one of the room's walls, which Link can walk through once it is open
    internal interface IDoor : IBlock
    {

        // The outer half of the door piece, the arch over the doorway. The room draws it again after Link and his
        // weapons, so they go under it into the doorway, as in the NES, instead of over it
        public void DrawArch(SpriteBatch spriteBatch);

    }
}
