using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    // Link climbs up and down it in the underground passage (see SideView)
    internal sealed class Ladder(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateLadderSprite())
    {
        public override bool IsSolid => false;
        public override bool IsClimbable => true;
    }
}
