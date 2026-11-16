using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    // Blocks never move, but some (like fire) still animate
    internal interface IBlock
    {

        // Block's hitbox
        public ICollider Collider { get; }

        // True if Link can't walk through it, false if he does (backgrounds, stairs, etc.)
        public bool IsSolid { get; }

        // True for a ladder, which Link can climb in a room seen from the side (see SideView)
        public bool IsClimbable { get; }

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
