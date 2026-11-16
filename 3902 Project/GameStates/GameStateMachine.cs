using System;
using System.IO;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Projectiles;
using Rooms;
using UI;

namespace GameStates
{
    // Which state the game is in (playing, paused, choosing an item, scrolling between rooms, game over or won),
    // and what those states share: Link, the dungeon's rooms, the HUD and the text writer.
    // Reset builds the dungeon afresh, which is how the game starts, and how it starts again after Link dies or wins.
    // window is the game window, where the mouse can click
    internal sealed class GameStateMachine(Rectangle window)
    {
        private const int DungeonLevel = 1;

        // The room Link starts in; the test room and the rest are a click away (see PlayingState)
        private const string StartRoom = "Room01_Entrance";

        // One CSV file per room, copied next to the game when it is built; see Rooms/RoomLoader.cs
        private static readonly string RoomFolder = Path.Combine(AppContext.BaseDirectory, "Levels", "Dungeon1");

        // The states that last as long as the dungeon does; the others are made each time the game switches to them
        private IGameState playing;
        private IGameState paused;
        private IGameState itemSelection;

        public IGameState Current { get; private set; }

        public IPlayer Link { get; private set; }
        public RoomManager Rooms { get; private set; }
        public Hud Hud { get; private set; }
        public TextRenderer Text { get; } = new();

        // Every room as it is in its file, Link at the entrance with full hearts and nothing picked up, and playing
        public void Reset()
        {
            ProjectileManager projectiles = new();

            Link = new Link(RoomLayout.EntryPosition(Direction.Down), projectiles);
            Rooms = new RoomManager(Link, new RoomLoader(projectiles).LoadAll(RoomFolder), projectiles, StartRoom);
            Hud = new Hud(Link, Rooms, DungeonLevel);

            playing = new PlayingState(this, window);
            paused = new PausedState(this);
            itemSelection = new ItemSelectionState(this, new PauseScreen(Link, Rooms, Hud));

            ChangeTo(playing);
        }

        public void Play() => ChangeTo(playing);
        public void Pause() => ChangeTo(paused);
        public void OpenItemSelection() => ChangeTo(itemSelection);

        // Link has walked out of the room through the doorway in the given wall
        public void ScrollToNextRoom(Direction side) => ChangeTo(new RoomTransitionState(this, side));

        public void GameOver() => ChangeTo(new GameOverState(this));
        public void Win() => ChangeTo(new WinState(this));

        public void Update(GameTime gameTime)
        {
            Current.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Current.Draw(spriteBatch);
            spriteBatch.End();
        }

        // The game as it stands: the room with everything in it, and the HUD above
        public void DrawDungeon(SpriteBatch spriteBatch)
        {
            Rooms.Draw(spriteBatch);
            Hud.Draw(spriteBatch);
        }

        private void ChangeTo(IGameState state)
        {
            Current = state;
            state.Enter();
        }
    }
}
