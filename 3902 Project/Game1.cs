using Commands;
using Enemies;
using Environment;
using Input;
using Interfaces;
using Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Player;
using Sprites;

namespace CSE_3902_Project
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private IPlayer link;
        private ShowcaseList<IEnemy> enemies;
        private ShowcaseList<IBlock> blocks;
        private ShowcaseList<IItem> items;
        private KeyboardController keyboard;

        // One dungeon room: 16 x 11 tiles of 16px, drawn at 4x scale
        private const int ScreenWidth = 1024;
        private const int ScreenHeight = 704;

        private static readonly Vector2 LinkStartPosition = new(100, 100);
        private static readonly Vector2 EnemyShowcasePosition = new(400, 200);
        private static readonly Vector2 BlockShowcasePosition = new(800, 200);
        private static readonly Vector2 ItemShowcasePosition = new(600, 450);

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

            keyboard = new KeyboardController();
            RegisterCommands();

            ResetGame();
        }

        private void RegisterCommands()
        {
            keyboard.RegisterCommand(Keys.P, new NextEnemyCommand(this));
            keyboard.RegisterCommand(Keys.O, new PreviousEnemyCommand(this));

            keyboard.RegisterCommand(Keys.Y, new NextBlockCommand(this));
            keyboard.RegisterCommand(Keys.T, new PreviousBlockCommand(this));

            keyboard.RegisterCommand(Keys.I, new NextItemCommand(this));
            keyboard.RegisterCommand(Keys.U, new PreviousItemCommand(this));

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

        private void RegisterMoveKeys(Keys letterKey, Keys arrowKey, Direction direction)
        {
            ICommand move = new LinkMoveCommand(link, direction);
            keyboard.RegisterHeldCommand(letterKey, move);
            keyboard.RegisterHeldCommand(arrowKey, move);
        }

        // Puts every game object back in its starting state; also used by the reset key
        public void ResetGame()
        {
            link = new Link(LinkStartPosition, GraphicsDevice.Viewport.Bounds);
            RegisterLinkCommands();

            enemies = new ShowcaseList<IEnemy>(
            [
                new Stalfos(EnemyShowcasePosition),
                new Zol(EnemyShowcasePosition),
                new Gel(EnemyShowcasePosition),
                new Keese(EnemyShowcasePosition),
                new Goriya(EnemyShowcasePosition)
            ]);

            blocks = new ShowcaseList<IBlock>(
            [
                new FireBlock(BlockShowcasePosition),
                new Stairs(BlockShowcasePosition),
                new SquareBlock(BlockShowcasePosition),
                new FishStatue(BlockShowcasePosition),
                new DragonStatue(BlockShowcasePosition),
                new BlueGap(BlockShowcasePosition),
                new WhiteBrick(BlockShowcasePosition),
                new Ladder(BlockShowcasePosition),
                new Wall(BlockShowcasePosition),
                new OpenDoor(BlockShowcasePosition),
                new BombedWallOpening(BlockShowcasePosition),
                new KeyholeLockedDoor(BlockShowcasePosition),
                new DiamondLockedDoor(BlockShowcasePosition)
            ]);

            items = new ShowcaseList<IItem>(
            [
                new Heart(ItemShowcasePosition),
                new HeartContainer(ItemShowcasePosition),
                new Rupee(ItemShowcasePosition),
                new TriforcePiece(ItemShowcasePosition),
                new Fairy(ItemShowcasePosition),
                new Key(ItemShowcasePosition),
                new Map(ItemShowcasePosition),
                new Compass(ItemShowcasePosition),
                new Bow(ItemShowcasePosition),
                new BoomerangItem(ItemShowcasePosition),
                new BombItem(ItemShowcasePosition),
                new Clock(ItemShowcasePosition)
            ]);
        }

        public void NextEnemy() => enemies.Next();
        public void PreviousEnemy() => enemies.Previous();

        public void NextBlock() => blocks.Next();
        public void PreviousBlock() => blocks.Previous();

        public void NextItem() => items.Next();
        public void PreviousItem() => items.Previous();

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // Runs the commands for any keys pressed or held this frame
            keyboard.Update();

            link.Update(gameTime);
            enemies.Current.Update(gameTime);
            blocks.Current.Update(gameTime);
            items.Current.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Back to front: room pieces, items, enemies, then Link on top
            blocks.Current.Draw(spriteBatch);
            items.Current.Draw(spriteBatch);
            enemies.Current.Draw(spriteBatch);
            link.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
