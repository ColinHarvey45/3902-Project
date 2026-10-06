using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    internal interface IProjectile
    {

        public bool IsFinished { get; }

        public void Update(GameTime gameTime);
        public void Draw(Texture2D texture);

    }
}
