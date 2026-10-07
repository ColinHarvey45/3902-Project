# CSE 3902 Project

A remake of the first dungeon of *The Legend of Zelda* (NES), built in C# with MonoGame.

GitHub: https://github.com/ColinHarvey45/3902-Project.git

## What Sprint 2 does

Link walks around an empty room and can use his sword and items. Alongside him, one **block**, one **item** and one **enemy or NPC** are shown at a time. Keys flip through every object in the dungeon so each can be seen moving and animating. Nothing collides with anything yet; that comes in Sprint 3.

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

### Showcase
| Key | Action |
|---|---|
| T / Y | Previous / next block |
| U / I | Previous / next item |
| O / P | Previous / next enemy or NPC |

Each list wraps around at both ends.

### Other
| Key | Action |
|---|---|
| R | Reset everything to how it was when the game started |
| Q or Escape | Quit |

## What's in the showcase

**Blocks (16, T / Y):** fire, stairs, square block, fish statue, dragon statue, blue gap, floor tile, black tile, sand tile, white brick, ladder, wall, open door, bombed wall opening, keyhole locked door, diamond locked door. The fire flickers; the rest are still.

**Items (12, U / I):** heart, heart container, rupee, triforce piece, fairy, key, map, compass, bow, boomerang, bomb, clock. The heart, rupee and triforce piece flash; the fairy flutters around the room.

**Enemies and NPCs (9, O / P):**
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

Wandering enemies and the fairy turn around at the edge of the window instead of leaving it.

## Known bugs and limitations

- **Nothing collides yet.** Arrows, boomerangs and fireballs pass through everything, enemies can't hurt Link (use **E** to see Link take damage), and Link's sword doesn't hit anything.
- **Link can walk off the screen.** There are no room walls yet, so nothing stops him.
- **The black tile looks like empty space**, because it is drawn black on the black background.
- **Arrows and Link's boomerang start from Link's top-left corner**, so they look slightly off-centre, most noticeably when fired up or down.
- **The Blade Trap and Aquamentus don't react to Link.** In the real game the Blade Trap charges when Link lines up with it and Aquamentus aims at Link; here the Blade Trap charges on a timer in a random direction and Aquamentus always fires to the left. Both need Link's position, which they get once collisions are added.
- **Stalfos, Goriya and Wallmaster walk freely** instead of tile by tile as in the original game.
- **Link is drawn on top of everything**, so an enemy that walks under him is hidden until it walks out again.
- **Only the object being shown moves.** Hidden enemies and items pause, and continue where they left off when you flip back to them.

## How the code is organized

| Folder | What's in it |
|---|---|
| `Player/` | Link, and his states in `Player/States/` (standing, walking, attacking, plus healthy and damaged) |
| `Enemies/` | Every enemy, and their states in `Enemies/States/` |
| `Npcs/` | Characters that don't fight (the Old Man) |
| `Items/` | Items Link can pick up |
| `Environment/` | Blocks, tiles, doors and walls |
| `Projectiles/` | Arrows, boomerangs, bombs and fireballs |
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
