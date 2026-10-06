using Animation;
using Commands;
using Enemies;
using Input;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.ObjectModel;
using Environment;

namespace CSE_3902_Project
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Link link;
        private Texture2D linkTexture; // moved to a field so Draw() can use it too
        private Texture2D enemyTexture;
        private Texture2D dungeonTexture;
        private Texture2D npcTexture;
        private Enemy[] enemies;
        private Block[] blocks;
        private int enemyIndex = 0;
        private int blockIndex = 0;
        private KeyboardController keyboard;

        // One dungeon room: 16 x 11 tiles of 16px, drawn at 4x scale
        private const int ScreenWidth = 1024;
        private const int ScreenHeight = 704;

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

            linkTexture = Content.Load<Texture2D>("TLOZLink-transparent2");
            enemyTexture = Content.Load<Texture2D>("TLOZDungeonEnemies-transparent");
            dungeonTexture = Content.Load<Texture2D>("TLOZDungeon-transparent");
            npcTexture = Content.Load<Texture2D>("TLOZNPCs-transparent");

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

            ICommand quit = new QuitCommand(this);
            keyboard.RegisterCommand(Keys.Q, quit);
            keyboard.RegisterCommand(Keys.Escape, quit);
            keyboard.RegisterCommand(Keys.R, new ResetCommand(this));
        }

        // Link is rebuilt on every reset, so his keys are rebound to the new Link each time
        private void RegisterLinkCommands()
        {
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

        // Puts every game object back in its starting state; also used by the reset key
        public void ResetGame()
        {
            link = new Link(linkTexture, spriteBatch, new Vector2(100, 100), keyboard, GraphicsDevice.Viewport.Bounds);
            RegisterLinkCommands();

            Zol zol = new Zol(enemyTexture, spriteBatch, new Vector2(400, 200));
            Stalfos stalfos = new Stalfos(enemyTexture, spriteBatch, new Vector2(400, 200));
            Gel gel = new Gel(enemyTexture, spriteBatch, new Vector2(400, 200));
            Keese keese = new Keese(enemyTexture, spriteBatch, new Vector2(400, 200));
            Goriya goriya = new Goriya(enemyTexture, spriteBatch, new Vector2(400, 200));

            FireBlock fire = new FireBlock(npcTexture, spriteBatch, new Vector2(800, 200));
            Stairs stairs = new Stairs(dungeonTexture, spriteBatch, new Vector2(800, 200));
            SquareBlock squareBlock = new SquareBlock(dungeonTexture, spriteBatch, new Vector2(800, 200));
            FishStatue fishStatue = new FishStatue(dungeonTexture, spriteBatch, new Vector2(800, 200));
            DragonStatue dragonStatue = new DragonStatue(dungeonTexture, spriteBatch, new Vector2(800, 200));
            BlueGap blueGap = new BlueGap(dungeonTexture, spriteBatch, new Vector2(800, 200));

            WhiteBrick whiteBrick = new WhiteBrick(dungeonTexture, spriteBatch, new Vector2(800, 200));
            Ladder ladder = new Ladder(dungeonTexture, spriteBatch, new Vector2(800, 200));
            
            Wall wall = new Wall(dungeonTexture, spriteBatch, new Vector2(800, 200));
            OpenDoor openDoor = new OpenDoor(dungeonTexture, spriteBatch, new Vector2(800, 200));
            BombedWallOpening bombedWallOpening = new BombedWallOpening(dungeonTexture, spriteBatch, new Vector2(800, 200));
            KeyholeLockedDoor keyholeLockedDoor = new KeyholeLockedDoor(dungeonTexture, spriteBatch, new Vector2(800, 200));
            DiamondLockedDoor diamondLockedDoor = new DiamondLockedDoor(dungeonTexture, spriteBatch, new Vector2(800, 200));
            
            enemies = [stalfos, zol, gel, keese, goriya];
            blocks = [fire, stairs, squareBlock, fishStatue, dragonStatue, blueGap,
                whiteBrick, ladder, wall, openDoor, bombedWallOpening, keyholeLockedDoor, diamondLockedDoor];
            enemyIndex = 0;
            blockIndex = 0;

            foreach (Enemy enemy in enemies)
            {
                // Hide everything initially
                enemy.SetVisibility(false); 
            }
            
            foreach (Block block in blocks)
            {
                // Hide everything initially
                block.SetVisibility(false);
            }



            enemies[enemyIndex].SetVisibility(true);
            blocks[blockIndex].SetVisibility(true);
        }

        public void NextEnemy()
        {
            ShowEnemy(enemyIndex + 1);
        }

        public void PreviousEnemy()
        {
            ShowEnemy(enemyIndex - 1);
        }

        public void NextBlock()
        {
            ShowBlock(blockIndex + 1);
        }

        public void PreviousBlock()
        {
            ShowBlock(blockIndex - 1);
        }

        // Hides the current enemy and shows the one at index, wrapping around at either end of the list
        private void ShowEnemy(int index)
        {
            enemies[enemyIndex].SetVisibility(false);
            enemyIndex = (index + enemies.Length) % enemies.Length;
            enemies[enemyIndex].SetVisibility(true);
        }

        // Hides the current block and shows the one at index, wrapping around at either end of the list
        private void ShowBlock(int index)
        {
            blocks[blockIndex].SetVisibility(false);
            blockIndex = (index + blocks.Length) % blocks.Length;
            blocks[blockIndex].SetVisibility(true);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // Runs the command for any key pressed this frame
            keyboard.Update();

            link.Update(gameTime);
            enemies[enemyIndex].Update(gameTime);
            blocks[blockIndex].Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            link.Draw(linkTexture);
            enemies[enemyIndex].Draw();
            blocks[blockIndex].Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
