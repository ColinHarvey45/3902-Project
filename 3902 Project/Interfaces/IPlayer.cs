using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;

namespace Interfaces
{
    // Everything the player character can be told to do; commands only talk to Link through this
    internal interface IPlayer
    {

        public void Move(Direction direction);
        public void SwordAttack();
        public void UseItem(SecondaryItem item);
        public void TakeDamage();

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
