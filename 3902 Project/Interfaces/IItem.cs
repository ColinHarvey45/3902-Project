using Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    // Something Link can pick up (hearts, rupees, keys, ...) by touching it
    internal interface IItem
    {

        public ICollider Collider { get; }

        // Moves it so its middle is at middle, e.g. onto the enemy carrying it (see Room)
        public void CenterOn(Vector2 middle);

        // What it does for Link when he picks it up (the room then takes it away)
        public void OnPickedUp(IPlayer player);

        // The sound it makes as he picks it up
        public Sound PickUpSound { get; }

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
