using System;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Rooms;

namespace GameStates
{
    // The screen scrolling from the room Link has walked out of to the one beyond that doorway, as in the NES: both
    // rooms slide along together, and Link with them, until he is just inside the new room's doorway.
    // Nothing else moves, and no keys work, until the scroll is over. A shutter he comes in by is open as he goes
    // through it, and shuts behind him if the room's enemies are still there (see Room)
    internal sealed class RoomTransitionState : IGameState
    {
        private const float ScrollSeconds = 1f;

        // The rooms are cut off at the edges of the room's area, so they slide under the HUD rather than over it
        private static readonly RasterizerState ClipToRoom = new() { ScissorTestEnable = true };

        private readonly GameStateMachine game;
        private readonly Room from;
        private readonly Room to;

        // Where the new room starts, relative to the old one; the screen scrolls this far
        private readonly Vector2 nextRoomOffset;

        // Link goes from where he left the old room to just inside the doorway he comes in by
        private readonly Vector2 linkStart;
        private readonly Vector2 linkEnd;

        // The wall of the new room he comes in through, and whether a shutter there opened to let him in
        private readonly Direction entrySide;
        private bool shutterOpenedForLink;

        private float elapsed;

        // side is the wall of the current room that Link walked out through
        public RoomTransitionState(GameStateMachine game, Direction side)
        {
            this.game = game;
            from = game.Rooms.CurrentRoom;
            to = game.Rooms.NeighbourOf(from, side);
            nextRoomOffset = RoomLayout.NextRoomOffset(side);
            linkStart = game.Link.Position;
            entrySide = side.Opposite();
            linkEnd = RoomLayout.EntryPosition(entrySide);
        }

        // How far through the scroll it is, from 0 to 1
        private float Progress => Math.Min(1f, elapsed / ScrollSeconds);

        public void Enter()
        {
            shutterOpenedForLink = to.OpenShutterFor(entrySide);
        }

        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            game.Link.PlaceAt(Vector2.Lerp(linkStart, linkEnd, Progress));

            if (Progress < 1f)
                return;

            game.Rooms.EnterRoom(to);
            if (shutterOpenedForLink)
                to.ShutBehindLink(entrySide);
            game.Play();
        }

        // The old room moves off the screen the opposite way to Link, and the new one follows it on. Link passes
        // under the arches of both rooms' doorways, which meet where the rooms do
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scrolled = nextRoomOffset * Progress;
            Vector2 fromOffset = -scrolled;
            Vector2 toOffset = nextRoomOffset - scrolled;

            // Moved, clipped drawing needs batches of its own, so the batch the states draw into is ended first
            // and begun again after
            spriteBatch.End();
            spriteBatch.GraphicsDevice.ScissorRectangle = RoomLayout.Bounds;
            DrawClipped(spriteBatch, fromOffset, from.Draw);
            DrawClipped(spriteBatch, toOffset, to.Draw);
            DrawClipped(spriteBatch, Vector2.Zero, game.Link.Draw);
            DrawClipped(spriteBatch, fromOffset, from.DrawArches);
            DrawClipped(spriteBatch, toOffset, to.DrawArches);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            game.Hud.Draw(spriteBatch);
        }

        private static void DrawClipped(SpriteBatch spriteBatch, Vector2 offset, Action<SpriteBatch> draw)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, rasterizerState: ClipToRoom,
                transformMatrix: Matrix.CreateTranslation(offset.X, offset.Y, 0));
            draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
