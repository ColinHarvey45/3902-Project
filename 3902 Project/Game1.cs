using Audio;
using Commands;
using GameStates;
using Input;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rooms;
using Sprites;
using UI;

namespace CSE_3902_Project
{
    // Loads the pictures and sounds, then hands each frame to the game's current state (see GameStates/),
    // which decides what moves, what is drawn and which keys work
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameStateMachine states;

        // The keys that work whatever state the game is in: quit, reset and mute
        private IController globalKeys;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // The HUD above one dungeon room
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = RoomLayout.Bounds.Width;
            graphics.PreferredBackBufferHeight = Hud.Height + RoomLayout.Bounds.Height;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LinkSpriteFactory.Instance.LoadAllTextures(Content);
            ProjectileSpriteFactory.Instance.LoadAllTextures(Content);
            EnemySpriteFactory.Instance.LoadAllTextures(Content);
            BlockSpriteFactory.Instance.LoadAllTextures(Content);
            ItemSpriteFactory.Instance.LoadAllTextures(Content);
            HudSpriteFactory.Instance.LoadAllTextures(Content);
            NpcSpriteFactory.Instance.LoadAllTextures(Content);
            FontSpriteFactory.Instance.LoadAllTextures(Content);
            AudioManager.Instance.LoadAllSounds(Content);

            Solid.Load(GraphicsDevice);

            states = new GameStateMachine(GraphicsDevice.Viewport.Bounds);
            states.Reset();
            globalKeys = CreateGlobalKeys();
        }

        private KeyboardController CreateGlobalKeys()
        {
            KeyboardController keys = new();

            ICommand quit = new QuitCommand(this);
            keys.RegisterCommand(Keys.Q, quit);
            keys.RegisterCommand(Keys.Escape, quit);
            keys.RegisterCommand(Keys.R, new ResetCommand(states));
            keys.RegisterCommand(Keys.M, new ToggleMuteCommand());

            return keys;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            globalKeys.Update();
            states.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            states.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
