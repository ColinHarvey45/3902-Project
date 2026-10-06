using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;

namespace Interfaces
{
    // What Link is doing right now (standing, walking, swinging his sword); each state decides
    // how Link reacts to input and which sprite he is drawn with
    internal interface ILinkState
    {

        public void Move(Direction direction);
        public void Stop();
        public void SwordAttack();
        public void UseItem(SecondaryItem item);

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch, Color tint);

    }
}
