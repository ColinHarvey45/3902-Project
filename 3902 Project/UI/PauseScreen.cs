using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Rooms;
using Sprites;
using System;
using System.Collections.Generic;

namespace UI
{
    // What the game shows while it is paused, laid out like the NES's: the items Link can use on top, the
    // dungeon map as far as he has explored it under that, and the HUD at the bottom.
    // Link picks the item for his B slot here: the arrow keys move the highlight (four blue corners round
    // an item) and the equip key makes the highlighted item the one he uses.
    // Everything is laid out in sheet pixels, like the pictures it is drawn on, and scaled up when drawn
    internal sealed class PauseScreen
    {
        private const int Scale = (int)Sprite.Scale;

        // Each of the two panels is 256 x 88 sheet pixels, which puts the HUD 176 pixels down
        private const int PanelHeight = 88;
        private static readonly Point InventoryPanelAt = new(0, 0);
        private static readonly Point MapPanelAt = new(0, PanelHeight);
        private static readonly Vector2 HudAt = new(0, 2 * PanelHeight * Scale);

        // On the inventory panel: the box for the item Link has equipped, and where each item he can
        // choose sits in the box of items. They are listed in the order the arrow keys go through them
        private static readonly Point EquippedItemAt = new(68, 48);

        private static readonly (SecondaryItem Item, Point At)[] Choices =
        [
            (SecondaryItem.Boomerang, new Point(132, 48)),
            (SecondaryItem.Bomb, new Point(156, 48)),
            (SecondaryItem.Arrow, new Point(184, 48))
        ];

        // The highlight is a 16 x 16 box round the item (which is 8 wide, so it starts 4 pixels to the left),
        // drawn as a 3 x 3 corner at each of its four corners
        private const int CursorSize = 16;
        private const int CursorInset = 4;
        private const int CursorCornerSize = 3;

        // On the map panel: where the map and compass pictures go (shown once Link has found them), and the
        // 8 x 8 squares of the dungeon map, one 8 x 8 tile for each room
        private static readonly Point MapItemAt = new(48, 24);
        private static readonly Point CompassAt = new(46, 66);
        private static readonly Rectangle DungeonMap = new(128, 8, 64, 64);
        private const int RoomTileSize = 8;

        // The panel has a checkerboard where the dungeon map goes, which is covered with the colour of the map's paper
        private static readonly Color PaperColor = new(252, 152, 56);

        // The room Link is in is marked with a small pale blue square
        private static readonly Rectangle RoomMarker = new(2, 2, 4, 4);
        private static readonly Color RoomMarkerColor = new(196, 212, 252);

        private readonly IPlayer link;
        private readonly RoomManager rooms;
        private readonly Hud hud;

        private readonly ISprite inventoryPanel = HudSpriteFactory.Instance.CreateInventoryPanelSprite();
        private readonly ISprite mapPanel = HudSpriteFactory.Instance.CreateMapPanelSprite();
        private readonly Dictionary<SecondaryItem, ISprite> itemIcons = [];
        private readonly ISprite mapIcon = ItemSpriteFactory.Instance.CreateMapSprite();
        private readonly ISprite compassIcon = ItemSpriteFactory.Instance.CreateCompassSprite();

        // The four corners of the highlight
        private readonly ISprite cursorTopLeft = HudSpriteFactory.Instance.CreateCursorCornerSprite(top: true, left: true);
        private readonly ISprite cursorTopRight = HudSpriteFactory.Instance.CreateCursorCornerSprite(top: true, left: false);
        private readonly ISprite cursorBottomLeft = HudSpriteFactory.Instance.CreateCursorCornerSprite(top: false, left: true);
        private readonly ISprite cursorBottomRight = HudSpriteFactory.Instance.CreateCursorCornerSprite(top: false, left: false);

        // How each room looks on the map depends on which of its walls have a doorway, which never changes,
        // so each room's tile is made once, the first time it is drawn
        private readonly Dictionary<Room, ISprite> roomTiles = [];

        // Which of the choices is highlighted
        private int selected;

        // hud is drawn at the bottom
        public PauseScreen(IPlayer link, RoomManager rooms, Hud hud)
        {
            this.link = link;
            this.rooms = rooms;
            this.hud = hud;

            foreach (SecondaryItem item in Enum.GetValues<SecondaryItem>())
                itemIcons[item] = ItemSpriteFactory.Instance.CreateInventorySprite(item);
        }

        // Called when the game is paused: the highlight starts on the item Link has equipped
        public void Open()
        {
            selected = Math.Max(0, Array.FindIndex(Choices, choice => choice.Item == link.EquippedItem));
        }

        // Left and up go to the item before the highlighted one, right and down to the one after it;
        // going past either end wraps around
        public void MoveSelection(Direction direction)
        {
            int step = direction is Direction.Left or Direction.Up ? -1 : 1;
            selected = (selected + step + Choices.Length) % Choices.Length;
        }

        // The highlighted item goes in Link's B slot
        public void EquipSelected()
        {
            link.EquippedItem = Choices[selected].Item;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            inventoryPanel.Draw(spriteBatch, Screen(InventoryPanelAt, Point.Zero));
            mapPanel.Draw(spriteBatch, Screen(MapPanelAt, Point.Zero));
            Solid.Fill(spriteBatch, Scaled(MapPanelAt, DungeonMap), PaperColor);

            DrawItems(spriteBatch);
            DrawMapAndCompass(spriteBatch);
            DrawDungeonMap(spriteBatch);

            hud.Draw(spriteBatch, HudAt);
        }

        // The equipped item in its own box, then every item Link can choose, with the highlight on one of them
        private void DrawItems(SpriteBatch spriteBatch)
        {
            itemIcons[link.EquippedItem].Draw(spriteBatch, Screen(InventoryPanelAt, EquippedItemAt));

            foreach ((SecondaryItem item, Point at) in Choices)
                itemIcons[item].Draw(spriteBatch, Screen(InventoryPanelAt, at));

            Point itemAt = Choices[selected].At;
            Point corner = new(itemAt.X - CursorInset, itemAt.Y);
            int far = CursorSize - CursorCornerSize;

            cursorTopLeft.Draw(spriteBatch, Screen(InventoryPanelAt, corner));
            cursorTopRight.Draw(spriteBatch, Screen(InventoryPanelAt, new Point(corner.X + far, corner.Y)));
            cursorBottomLeft.Draw(spriteBatch, Screen(InventoryPanelAt, new Point(corner.X, corner.Y + far)));
            cursorBottomRight.Draw(spriteBatch, Screen(InventoryPanelAt, new Point(corner.X + far, corner.Y + far)));
        }

        private void DrawMapAndCompass(SpriteBatch spriteBatch)
        {
            if (link.Inventory.HasMap)
                mapIcon.Draw(spriteBatch, Screen(MapPanelAt, MapItemAt));

            if (link.Inventory.HasCompass)
                compassIcon.Draw(spriteBatch, Screen(MapPanelAt, CompassAt));
        }

        // Only the rooms Link has been in are on the map, each showing where its doorways are, and the one he is in is marked
        private void DrawDungeonMap(SpriteBatch spriteBatch)
        {
            foreach (Room room in rooms.AllRooms)
            {
                if (room.Visited && room.MapPosition is Point cell)
                    TileFor(room).Draw(spriteBatch, Screen(MapPanelAt, TileAt(cell)));
            }

            if (rooms.CurrentRoom.MapPosition is Point here)
            {
                Point tile = TileAt(here);
                Rectangle marker = new(tile.X + RoomMarker.X, tile.Y + RoomMarker.Y, RoomMarker.Width, RoomMarker.Height);
                Solid.Fill(spriteBatch, Scaled(MapPanelAt, marker), RoomMarkerColor);
            }
        }

        private ISprite TileFor(Room room)
        {
            if (!roomTiles.TryGetValue(room, out ISprite tile))
            {
                tile = HudSpriteFactory.Instance.CreateDungeonRoomSprite(
                    room.HasDoorway(Direction.Up), room.HasDoorway(Direction.Down),
                    room.HasDoorway(Direction.Left), room.HasDoorway(Direction.Right));
                roomTiles[room] = tile;
            }

            return tile;
        }

        // Where a room's tile goes on the map panel
        private static Point TileAt(Point cell)
        {
            return new Point(DungeonMap.X + cell.X * RoomTileSize, DungeonMap.Y + cell.Y * RoomTileSize);
        }

        // Where a point on a panel, in sheet pixels, is on screen
        private static Vector2 Screen(Point panelAt, Point sheetPixels)
        {
            return new Vector2((panelAt.X + sheetPixels.X) * Scale, (panelAt.Y + sheetPixels.Y) * Scale);
        }

        // Where an area of a panel, in sheet pixels, is on screen
        private static Rectangle Scaled(Point panelAt, Rectangle area)
        {
            return new Rectangle((panelAt.X + area.X) * Scale, (panelAt.Y + area.Y) * Scale, area.Width * Scale, area.Height * Scale);
        }
    }
}
