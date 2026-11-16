using Commands;
using Input;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Player;
using Rooms;

namespace GameStates
{
    // Link playing through the dungeon: everything moves, and his keys (and the mouse's room switching) work.
    // The game moves on to another state when he walks out through a doorway, runs out of hearts or picks up the triforce
    internal sealed class PlayingState : IGameState
    {
        private readonly GameStateMachine game;
        private readonly KeyboardController keyboard = new();
        private readonly MouseController mouse;
        private readonly IController[] controllers;

        // window is where the mouse can click
        public PlayingState(GameStateMachine game, Rectangle window)
        {
            this.game = game;
            mouse = new MouseController(window);
            controllers = [keyboard, mouse];

            RegisterLinkCommands(game.Link);
            RegisterRoomCommands(game.Rooms);

            keyboard.RegisterCommand(Keys.Space, new PauseCommand(game));
            keyboard.RegisterCommand(Keys.F, new OpenItemSelectionCommand(game));
        }

        public void Enter()
        {
            foreach (IController controller in controllers)
                controller.IgnoreHeldInput();
        }

        public void Update(GameTime gameTime)
        {
            foreach (IController controller in controllers)
                controller.Update();

            // A key may have switched to another state (paused, say), and then nothing more happens here this frame
            if (game.Current != this)
                return;

            game.Rooms.Update(gameTime);

            if (game.Link.Health == 0)
                game.GameOver();
            else if (game.Link.Inventory.HasTriforce)
                game.Win();
            else if (game.Rooms.DoorwayLinkLeftBy is Direction side)
                game.ScrollToNextRoom(side);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            game.DrawDungeon(spriteBatch);
        }

        private void RegisterLinkCommands(IPlayer link)
        {
            // Registered in priority order: if several are held, the first one listed wins
            RegisterMoveKeys(link, Keys.W, Keys.Up, Direction.Up);
            RegisterMoveKeys(link, Keys.S, Keys.Down, Direction.Down);
            RegisterMoveKeys(link, Keys.A, Keys.Left, Direction.Left);
            RegisterMoveKeys(link, Keys.D, Keys.Right, Direction.Right);

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

            // Uses the item in the B slot, which is chosen on the inventory screen
            ICommand useEquippedItem = new LinkUseEquippedItemCommand(link);
            keyboard.RegisterCommand(Keys.X, useEquippedItem);
            keyboard.RegisterCommand(Keys.B, useEquippedItem);

            keyboard.RegisterCommand(Keys.E, new LinkTakeDamageCommand(link));
        }

        private void RegisterMoveKeys(IPlayer link, Keys letterKey, Keys arrowKey, Direction direction)
        {
            ICommand move = new LinkMoveCommand(link, direction);
            keyboard.RegisterHeldCommand(letterKey, move);
            keyboard.RegisterHeldCommand(arrowKey, move);
        }

        // Jumps straight to another room, in file order: right click or P goes to the next room, left click or O
        // to the previous one. This is how to get to the test room, which is just before the entrance
        private void RegisterRoomCommands(RoomManager rooms)
        {
            ICommand nextRoom = new NextRoomCommand(rooms);
            ICommand previousRoom = new PreviousRoomCommand(rooms);

            keyboard.RegisterCommand(Keys.P, nextRoom);
            keyboard.RegisterCommand(Keys.O, previousRoom);
            mouse.RegisterRightClick(nextRoom);
            mouse.RegisterLeftClick(previousRoom);
        }
    }
}
