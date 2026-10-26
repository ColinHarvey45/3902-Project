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

        // One dungeon room: 16 x 11 tiles of 16px, drawn at 4x scale
        private const int ScreenWidth = 1024;
        private const int ScreenHeight = 704;

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
            NpcSpriteFactory.Instance.LoadAllTextures(Content);

            keyboard = new KeyboardController();
            mouse = new MouseController(GraphicsDevice.Viewport.Bounds);
            RegisterCommands();

            ResetGame();
        }

        private void RegisterCommands()
        {
            ICommand quit = new QuitCommand(this);
            keyboard.RegisterCommand(Keys.Q, quit);
            keyboard.RegisterCommand(Keys.Escape, quit);
            keyboard.RegisterCommand(Keys.R, new ResetCommand(this));
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

            keyboard.RegisterCommand(Keys.E, new LinkTakeDamageCommand(link));
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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // Runs the commands for any keys pressed or held, and mouse buttons clicked, this frame
            keyboard.Update();
            mouse.Update();

            rooms.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            rooms.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
