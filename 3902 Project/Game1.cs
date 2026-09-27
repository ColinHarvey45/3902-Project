using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Animation;
using Interfaces;
using Enemies;
// using YourControllersNamespace; // wherever KeyboardController actually lives
using Input;

namespace CSE_3902_Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Link link;
        private Zol zol;
        private Texture2D linkTexture; // moved to a field so Draw() can use it too
        private Texture2D enemyTexture;
        private Enemy[] enemies;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            linkTexture = Content.Load<Texture2D>("TLOZLink-transparent2");
            enemyTexture = Content.Load<Texture2D>("TLOZDungeonEnemies-transparent");

            IController controller = new KeyboardController();

            link = new Link(linkTexture, spriteBatch, new Vector2(100, 100), controller);
            zol = new Zol(enemyTexture, spriteBatch, new Vector2(400, 200));

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            link.Update(gameTime);
            zol.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            link.Draw(linkTexture);
            zol.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
