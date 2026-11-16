using Microsoft.Xna.Framework;

namespace Rooms
{
    // A way to another room that isn't through a doorway, like the stairs down to the underground passage and the
    // ladder back up. When Link steps onto Area he is taken to the room called Destination and put at Arrival
    internal sealed record Warp(Rectangle Area, string Destination, Vector2 Arrival);
}
