using Interfaces;

namespace Sprites
{
    // How a door piece looks: the whole piece, and its arch, the outer half of the piece (away from the room),
    // drawn at the same place. The room draws the arch again over Link (see IDoor)
    internal readonly record struct DoorSprites(ISprite Whole, ISprite Arch);
}
