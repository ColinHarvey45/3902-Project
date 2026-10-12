# CSE 3902 Project

A remake of the first dungeon of *The Legend of Zelda* (NES), built in C# with MonoGame.

GitHub: https://github.com/ColinHarvey45/3902-Project.git

## What the game does so far

Link starts in a dungeon room, with its walls, floor and doors, and can walk around and use his sword and items. For now there is one room: a test room with one of every block, door, item, enemy and NPC in the dungeon, so each can be seen moving and animating. Nothing collides with anything yet; collisions, the rest of the dungeon's rooms and switching rooms with the mouse come later in Sprint 3.

## Building and running

The game opens in a 1024 x 704 window, the size of one dungeon room.

## Controls

### Link
| Key | Action |
|---|---|
| W A S D or arrow keys | Walk up, left, down, right. Link only walks in four directions; if two are held, up/down wins over left/right |
| Z or N | Swing the sword. Link can't move or use items until the swing finishes |
| 1 | Fire an arrow |
| 2 | Throw the boomerang. Only one can be out at a time; it comes back to Link |
| 3 | Drop a bomb on the tile in front of Link. It explodes after about a second |
| E | Damage Link: he flashes red for a second and can't be damaged again until it stops |

The number keys on the numpad work too.

### Other
| Key | Action |
|---|---|
| R | Reset everything to how it was when the game started |
| Q or Escape | Quit |

## What's in the test room

**Doors:** one in the gap in each wall: a keyhole locked door at the top, a bombed wall opening on the left, a diamond locked door on the right, and an open door at the bottom, where Link starts. Each door is drawn the right way round for the wall it is in. (A plain wall piece, for a wall with no door, also exists.)

**Blocks (top row):** square block, fish statue, dragon statue, blue gap, black tile, sand tile, stairs, white brick, ladder, and two fires with the Old Man between them. The fires flicker; the rest are still. The floor itself is made of plain floor tiles.

**Items (second row from the bottom):** heart, heart container, rupee, triforce piece, key, map, compass, bow, boomerang, bomb, clock and fairy. The heart, rupee and triforce piece flash; the fairy flutters around the room.

**Enemies and NPCs (middle of the room):**
| Character | What it does |
|---|---|
| Stalfos | Walks in a random direction, turning every couple of seconds |
| Zol | Creeps along in slow hops with pauses in between |
| Gel | Darts about in quick hops with short pauses |
| Keese | Takes off, flutters in all eight directions while speeding up and slowing down, then lands for a moment |
| Goriya | Walks like a Stalfos, facing the way it walks; sometimes stops to throw a boomerang and waits for it to come back |
| Wallmaster | A hand that wanders, opening and closing |
| Blade Trap | Waits, shoots out in a straight line, then slides slowly back to where it started |
| Aquamentus | The boss: paces back and forth and every few seconds opens its mouth to breathe three fireballs |
| Old Man | Stands still |

Wandering enemies and the fairy turn around at the edge of the floor instead of walking onto the walls.

## Known bugs and limitations

- **Nothing collides yet.** Arrows, boomerangs and fireballs pass through everything, enemies can't hurt Link (use **E** to see Link take damage), and Link's sword doesn't hit anything.
- **Link can walk through the walls and off the screen.** The walls are drawn but aren't solid until collisions are added.
- **There is only one room**, the test room. The dungeon's real rooms, loaded from files, come later in Sprint 3.
- **The Blade Trap and Aquamentus don't react to Link.** In the real game the Blade Trap charges when Link lines up with it and Aquamentus aims at Link; here the Blade Trap charges on a timer in a random direction and Aquamentus always fires to the left. Both need Link's position, which they get once collisions are added.
- **Stalfos, Goriya and Wallmaster walk freely** instead of tile by tile as in the original game.
- **Link is drawn on top of every enemy**, so an enemy that walks under him is hidden until it walks out again. Arrows, boomerangs, bombs and fireballs are drawn on top of Link.

## How the code is organized

| Folder | What's in it |
|---|---|
| `Rooms/` | A room and everything in it, the room manager that holds every room and knows which one Link is in, `RoomLayout` (where the walls, floor tiles and doors are on screen), and the test room |
| `Player/` | Link, and his states in `Player/States/` (standing, walking, attacking, plus healthy and damaged) |
| `Enemies/` | Every enemy, and their states in `Enemies/States/` |
| `Npcs/` | Characters that don't fight (the Old Man) |
| `Items/` | Items Link can pick up |
| `Environment/` | Blocks, tiles, doors and walls |
| `Projectiles/` | Arrows, boomerangs, bombs and fireballs, and `ProjectileManager`, which holds every one in the room whoever fired it (Link, Goriya and Aquamentus all fire into it; changing room clears it) |
| `Sprites/` | The `Sprite` class and one sprite factory per kind of object |
| `Commands/` | One command per action a key can trigger |
| `Input/` | The keyboard controller |
| `Movement/` | Random wandering shared by enemies and the fairy |
| `Interfaces/` | The interfaces everything above implements |

**Design patterns:**
- **Command:** each key is bound to a command object in `Game1`, and `KeyboardController` runs it. Keys can run once per press (most keys) or every frame they're held (movement). Link never reads the keyboard himself.
- **State:** Link, Goriya, Gel, Zol, Keese, the Blade Trap and Aquamentus each hand their behaviour to a state object that switches to another state when it's done. Link has two at once: what he's doing, and whether he's hurt.
- **Factory:** every sprite-sheet rectangle lives in a sprite factory in `Sprites/`. Game objects ask a factory for a sprite and only decide where it is drawn; they never touch sprite-sheet coordinates themselves.

## Tools and processes

- **GitHub** for version control. Some changes went through branches and pull requests; most were committed straight to `master`.
- **.NET code analyzers (Roslyn)** with Microsoft's recommended rules. The build has 0 warnings; every warning the analyzers found, and how it was fixed or why it was suppressed, is in [CodeAnalysis.md](CodeAnalysis.md).
- **Code reviews** for readability and maintainability, kept in the `CodeReviews/` folder.
- **Sprint reflection:** see `Sprint2Reflections`.

## Credits

Sprite sheets ripped by Mister Mike, as credited on the sheets themselves. *The Legend of Zelda* is © Nintendo.
