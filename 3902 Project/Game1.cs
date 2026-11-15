using Commands;
using Input;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Player;
using Projectiles;
using Rooms;
using Sprites;
using System;
using System.IO;
using Audio;
using UI;

namespace CSE_3902_Project
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private IPlayer link;
        private RoomManager rooms;
        private KeyboardController keyboard;
        private MouseController mouse;
        private Hud hud;
        private PauseScreen pauseScreen;

        // The game and the pause screen each have their own keys; only one set is live at a time
        private KeyboardController pauseKeyboard;
        private bool isPaused;

        // The HUD above one dungeon room: the room is 16 x 11 tiles of 16px, drawn at 4x scale
        private const int ScreenWidth = 1024;
        private const int ScreenHeight = Hud.Height + 704;

        private const int DungeonLevel = 1;

        // The middle of the bottom row of the floor, just inside the door Link comes in by
        private static readonly Vector2 LinkStartPosition = RoomLayout.TilePosition(5.5f, 6);

        // One CSV file per room, copied next to the game when it is built; see Rooms/RoomLoader.cs
        private static readonly string RoomFolder = Path.Combine(AppContext.BaseDirectory, "Levels", "Dungeon1");

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
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
            AudioManager.Instance.LoadAllSounds(Content);

            Solid.Load(GraphicsDevice);

            keyboard = new KeyboardController();
            pauseKeyboard = new KeyboardController();
            mouse = new MouseController(GraphicsDevice.Viewport.Bounds);
            RegisterCommands();

            ResetGame();
            AudioManager.Instance.PlayMusic();
        }

        private void RegisterCommands()
        {
            ICommand quit = new QuitCommand(this);
            keyboard.RegisterCommand(Keys.Q, quit);
            keyboard.RegisterCommand(Keys.Escape, quit);
            keyboard.RegisterCommand(Keys.R, new ResetCommand(this));
            keyboard.RegisterCommand(Keys.M, new ToggleMuteCommand());

            // The pause key works in both places, so it can also take the game out of pause
            ICommand togglePause = new TogglePauseCommand(this);
            keyboard.RegisterCommand(Keys.F, togglePause);
            pauseKeyboard.RegisterCommand(Keys.F, togglePause);
        }

        // Link is rebuilt on every reset, so his keys are rebound to the new Link each time
        private void RegisterLinkCommands()
        {
            // Registered in priority order: if several are held, the first one listed wins
            RegisterMoveKeys(Keys.W, Keys.Up, Direction.Up);
            RegisterMoveKeys(Keys.S, Keys.Down, Direction.Down);
            RegisterMoveKeys(Keys.A, Keys.Left, Direction.Left);
            RegisterMoveKeys(Keys.D, Keys.Right, Direction.Right);

            ICommand swordAttack = new LinkSwordAttackCommand(link);
            keyboard.RegisterCommand(Keys.Z, swordAttack);
            keyboard.RegisterCommand(Keys.N, swordAttack);

            ICommand fireArrow = new LinkFireArrowCommand(link);
            keyboard.RegisterCommand(Keys.D1, fireArrow);
            keyboard.RegisterCommand(Keys.NumPad1, fireArrow);

            ICommand throwBoomerang = new LinkThrowBoomerangCommand(link);
            keyboard.RegisterCommand(Keys.D2, throwBoomerang);
            keyboard.RegisterCommand(Keys.NumPad2, throwBoomerang);

            ICommand placeBomb = new LinkPlaceBombCommand(link);
            keyboard.RegisterCommand(Keys.D3, placeBomb);
            keyboard.RegisterCommand(Keys.NumPad3, placeBomb);

            // Uses the item in the B slot, which is chosen on the pause screen
            ICommand useEquippedItem = new LinkUseEquippedItemCommand(link);
            keyboard.RegisterCommand(Keys.X, useEquippedItem);
            keyboard.RegisterCommand(Keys.B, useEquippedItem);

            keyboard.RegisterCommand(Keys.E, new LinkTakeDamageCommand(link));
        }

        // The pause screen is rebuilt on every reset too. The arrow keys move the highlight between the items,
        // and G makes the highlighted one the item Link uses
        private void RegisterPauseCommands()
        {
            pauseKeyboard.RegisterCommand(Keys.Up, new PauseMoveSelectionCommand(pauseScreen, Direction.Up));
            pauseKeyboard.RegisterCommand(Keys.Down, new PauseMoveSelectionCommand(pauseScreen, Direction.Down));
            pauseKeyboard.RegisterCommand(Keys.Left, new PauseMoveSelectionCommand(pauseScreen, Direction.Left));
            pauseKeyboard.RegisterCommand(Keys.Right, new PauseMoveSelectionCommand(pauseScreen, Direction.Right));
            pauseKeyboard.RegisterCommand(Keys.G, new PauseEquipItemCommand(pauseScreen));
        }
        
        // The rooms are rebuilt on every reset too, so these are rebound to the new rooms each time.
        // Right click or P goes to the next room, left click or O to the previous one
        private void RegisterRoomCommands()
        {
            ICommand nextRoom = new NextRoomCommand(rooms);
            ICommand previousRoom = new PreviousRoomCommand(rooms);

            keyboard.RegisterCommand(Keys.P, nextRoom);
            keyboard.RegisterCommand(Keys.O, previousRoom);
            mouse.RegisterRightClick(nextRoom);
            mouse.RegisterLeftClick(previousRoom);
        }

        private void RegisterMoveKeys(Keys letterKey, Keys arrowKey, Direction direction)
        {
            ICommand move = new LinkMoveCommand(link, direction);
            keyboard.RegisterHeldCommand(letterKey, move);
            keyboard.RegisterHeldCommand(arrowKey, move);
        }

        // Reloads every room from its file, as it was at the start, and puts Link back in the first one;
        // also used by the reset key
        public void ResetGame()
        {
            ProjectileManager projectiles = new();

            link = new Link(LinkStartPosition, projectiles);
            RegisterLinkCommands();

            Room[] dungeon = new RoomLoader(projectiles).LoadAll(RoomFolder);
            rooms = new RoomManager(link, dungeon, projectiles);
            RegisterRoomCommands();

            hud = new Hud(link, rooms, DungeonLevel);

            pauseScreen = new PauseScreen(link, rooms, hud);
            RegisterPauseCommands();
            isPaused = false;
        }

        // Pauses the game, or carries on if it is already paused; the pause key does this
        public void TogglePause()
        {
            isPaused = !isPaused;

            if (isPaused)
                pauseScreen.Open();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // While the game is paused only the pause screen's keys work: the game's keys and the mouse are
            // switched off (they still watch, so what was held when the pause ended isn't taken as a new press)
            bool wasPaused = isPaused;
            keyboard.IsEnabled = !wasPaused;
            pauseKeyboard.IsEnabled = wasPaused;
            mouse.IsEnabled = !wasPaused;

            // Runs the commands for any keys pressed or held, and mouse buttons clicked, this frame
            keyboard.Update();
            pauseKeyboard.Update();
            mouse.Update();

            // Nothing moves or attacks while the game is paused
            if (!wasPaused)
                rooms.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            if (isPaused)
            {
                pauseScreen.Draw(spriteBatch);
            }
            else
            {
                rooms.Draw(spriteBatch);
                hud.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
