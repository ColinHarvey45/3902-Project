# CSE 3902 Project

A remake of the first dungeon of *The Legend of Zelda* (NES), built in C# with MonoGame.

GitHub: https://github.com/ColinHarvey45/3902-Project.git

## What Sprint 3 does

All 18 rooms of the first dungeon (Level 1, "the Eagle") are built from room files, plus a test room with one of every kind of object in the dungeon. Link can walk around any room, swing his sword and use his items, and everything collides. Link can't walk through walls, blocks or the Old Man. Enemies hurt him, and his sword, arrows, boomerang and bombs kill them. Arrows, boomerangs and fireballs stop at walls, and Link picks up items by walking onto them. Click the mouse (or press O / P) to jump to any room.

## Building and running

Open `3902 Project.slnx` in Visual Studio and run it, or run `dotnet run --project "3902 Project"` from this folder. The game opens in a 1024 x 704 window, the size of one dungeon room, in the test room.

The room files in `3902 Project/Levels/Dungeon1/` are copied next to the game when it is built. If a room file has a mistake, the game stops with a message naming the file, the line and what is wrong.

## Controls

### Link
| Key | Action |
|---|---|
| W A S D or arrow keys | Walk up, left, down, right. Link only walks in four directions; if two are held, up/down wins over left/right |
| Z or N | Swing the sword. Link can't move or use items until the swing finishes |
| 1 | Fire an arrow |
| 2 | Throw the boomerang. Only one can be out at a time; it comes back to Link |
| 3 | Drop a bomb on the tile in front of Link. It explodes after about a second |
| E | Hurt Link, for testing: he flashes red for a second and is knocked back a little |

The number keys on the numpad work too.

### Rooms
| Input | Action |
|---|---|
| Right mouse button, or P | Go to the next room |
| Left mouse button, or O | Go to the previous room |

Rooms go in the order of the list below and wrap around at both ends. Clicks only count inside the game window, and holding a button down changes the room only once.

### Other
| Key | Action |
|---|---|
| R | Reload every room from its file and put Link back at the start of the test room |
| Q or Escape | Quit |

## The rooms

The test room comes first, then the dungeon's rooms, roughly in the order you would explore them. Their layouts, doors, enemies and items were read off the Level 1 map at NESMaps (see Credits).

| # | File | What's in it |
|---|---|---|
| 0 | `Room00_Test.csv` | **Test room:** one of every block, door, item, enemy and NPC in the dungeon (see below) |
| 1 | `Room01_Entrance.csv` | The entrance: 12 statues and some sand |
| 2 | `Room02_KeeseWest.csv` | West of the entrance: 3 Keese and a key |
| 3 | `Room03_StalfosEast.csv` | East of the entrance: 5 Stalfos |
| 4 | `Room04_StalfosNorth.csv` | North of the entrance: 3 Stalfos |
| 5 | `Room05_StalfosKey.csv` | 5 Stalfos and a key |
| 6 | `Room06_KeeseNorthWest.csv` | 6 Keese |
| 7 | `Room07_Compass.csv` | 8 Keese and the compass |
| 8 | `Room08_MovableBlock.csv` | 3 Gels, around the block that can be pushed in the original game |
| 9 | `Room09_OldMan.csv` | The Old Man between two fires |
| 10 | `Room10_Map.csv` | 5 Gels and the dungeon map |
| 11 | `Room11_Boomerang.csv` | 3 Goriyas and the boomerang |
| 12 | `Room12_Wallmasters.csv` | 8 Wallmasters and a key |
| 13 | `Room13_WaterMaze.csv` | A maze of water with 3 Stalfos |
| 14 | `Room14_GoriyaKey.csv` | 3 Goriyas and a key, around a ring of water |
| 15 | `Room15_BladeTraps.csv` | 4 Blade Traps, and the stairs down to the bow |
| 16 | `Room16_BowPassage.csv` | The underground passage below the stairs: the bow and 4 Keese |
| 17 | `Room17_Aquamentus.csv` | Aquamentus, the boss, and a heart container |
| 18 | `Room18_Triforce.csv` | The triforce piece |

### The test room

- **Doors:** a keyhole locked door at the top, a bombed wall opening on the left, a diamond locked door on the right, and an open door at the bottom, where Link starts. (A plain wall piece, for a wall with no door, is used in the dungeon rooms.)
- **Blocks (top row):** square block, fish statue, dragon statue, blue gap, black tile, sand tile, stairs, white brick, ladder, and two fires with the Old Man between them.
- **Enemies (middle):** Stalfos, Zol, Gel, Keese, Goriya, Wallmaster, Blade Trap and Aquamentus.
- **Items (second row from the bottom):** heart, heart container, rupee, triforce piece, key, map, compass, bow, boomerang, bomb, clock and fairy.

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

### Room files

Each room is one CSV file in `3902 Project/Levels/Dungeon1/`, loaded in file-name order by `Rooms/RoomLoader.cs`. Every line adds something to the room:

```
Door,Up,KeyholeLockedDoor      the door (or plain wall) in the gap in one wall
Floor,..QQ....QQ..             one row of the 12 x 7 floor (7 of these), one symbol per tile
Enemy,Stalfos,6,4              a kind of enemy (its class name) and the floor tile it starts on
Npc,OldMan,5.5,2
Item,Key,4,5
Block,FireBlock,9,0
```

Floor symbols: `.` floor, `Q` square block, `F` fish statue, `D` dragon statue, `G` blue gap (water), `B` black tile, `S` sand, `T` stairs, `W` white brick, `L` ladder. Tiles count from the top-left of the floor; halves like `5.5` are allowed. Lines starting with `#` are comments. The underground passage has no ordinary walls, so it uses `Tiles` lines, which describe the whole 16 x 11 screen instead. The full description is at the top of `RoomLoader.cs`.

## Collisions

All collisions are handled in `Collisions/CollisionManager.cs`, once a frame after everything has moved.

| What touches what | What happens |
|---|---|
| Link and an enemy | Link is hurt: he flashes red for about a second, during which nothing can hurt him again, and is knocked back a little |
| Link and a fireball, or a Goriya's boomerang | Link is hurt the same way; the fireball disappears and the boomerang flies back to its Goriya |
| Link's sword and an enemy | The enemy is hurt; every enemy dies in one hit (Blade Traps can't be hurt) |
| An arrow and an enemy | The first enemy it reaches is hurt, and the arrow disappears |
| Link's boomerang and an enemy | The enemy is hurt, and the boomerang flies back to Link |
| A bomb's explosion and enemies | Every enemy in the blast (two tiles square) is hurt |
| Link and a wall, a solid block or a closed door | Link is stopped. Solid: square blocks, statues, water (blue gap), white bricks, fire, and plain wall, locked and diamond doors |
| Link and an open doorway | Link can walk into the gap, and stops at the edge of the screen |
| Link and the Old Man | Link is stopped |
| Link and an item | Link picks it up, and it disappears from the room |
| An enemy and a wall or solid block | Most enemies step back and walk another way. Keese fly over blocks; a Blade Trap stops and slides back home; Aquamentus turns around |
| An arrow or a fireball and a wall or solid block | It disappears |
| A boomerang and a wall or solid block | It turns back early |

Link walks over floor, sand, black tiles, stairs, ladders, open doors and bombed openings. Enemies pass through each other, as in the original game.

## Differences from the original game

These are deliberate for now, or planned for later sprints:

- **Doors don't open, and you can't walk into the next room.** Locked doors can't be unlocked with a key, diamond (shutter) doors stay shut, and bombs don't open walls. Walking out through an open doorway stops at the edge of the screen. Stairs don't lead anywhere. Use the mouse (or O / P) to change rooms.
- **No hearts, inventory or HUD yet.** Link can't die, and items disappear when picked up without doing anything. Items 1, 2 and 3 can always be used, as many times as you like.
- **Every enemy dies in one hit**, and simply disappears; there is no death animation yet.
- **Knockback pushes Link a few pixels the opposite way to how he is facing**, rather than away from whatever hit him.
- **The Old Man doesn't say anything.** His message ("EASTMOST PENNINSULA IS THE SECRET.") needs text drawing, which comes later.
- **The movable block in room 8 can't be pushed.**
- **The Blade Trap and Aquamentus don't react to Link.** In the real game the Blade Trap charges when Link lines up with it and Aquamentus aims at Link. Here the Blade Trap charges on a timer in a random direction, and Aquamentus always fires to the left.
- **Stalfos, Goriya and Wallmasters walk freely** instead of tile by tile, and Wallmasters wander the floor instead of reaching out of the walls to grab Link.
- **Link doesn't change pose when he uses an item.**
- **Rooms change instantly**, without the scrolling transition, which comes in Sprint 4.

## Known bugs

- **Link keeps his position when you change rooms with the mouse or O / P.** He can land on an enemy (and is hurt) or inside a block (and is pushed out of it).
- **Link is drawn on top of every enemy**, so an enemy that walks under him is hidden until it walks out again. Arrows, boomerangs, bombs and fireballs are drawn on top of Link.

## How the code is organized

| Folder | What's in it |
|---|---|
| `Rooms/` | `Room` (a room and everything in it), `RoomManager` (every room, and which one Link is in), `RoomLoader` (builds rooms from their files) and `RoomLayout` (where the walls, floor tiles and door gaps are on screen) |
| `Levels/Dungeon1/` | One CSV file per room |
| `Collisions/` | `CollisionManager`, which handles every collision |
| `Colliders/` | `BoxCollider`, the hitbox that follows each object |
| `Player/` | Link, and his states in `Player/States/` (standing, walking, attacking, plus healthy and damaged) |
| `Enemies/` | Every enemy, their shared health (`EnemyHealth`), and their states in `Enemies/States/` |
| `Npcs/` | Characters that don't fight (the Old Man) |
| `Items/` | Items Link can pick up |
| `Environment/` | Blocks, tiles, doors and walls |
| `Projectiles/` | Arrows, boomerangs, bombs and fireballs, and `ProjectileManager`, which holds every one in the room whoever fired it (changing room clears it) |
| `Sprites/` | The `Sprite` class and one sprite factory per kind of object |
| `Commands/` | One command per action a key or mouse button can trigger |
| `Input/` | The keyboard and mouse controllers |
| `Movement/` | Random wandering shared by enemies and the fairy |
| `Interfaces/` | The interfaces everything above implements |

**Design patterns:**
- **Command:** each key and mouse button is bound to a command object in `Game1`, and `KeyboardController` or `MouseController` runs it. Keys can run once per press (most keys) or every frame they're held (movement). Link and the rooms never read the keyboard or mouse themselves.
- **State:** Link, Goriya, Gel, Zol, Keese, the Blade Trap and Aquamentus each hand their behaviour to a state object that switches to another state when it's done. Link has two at once: what he's doing, and whether he's hurt (his healthy state decides that a hit hurts him and knocks him back; his damaged state ignores hits).
- **Factory:** every sprite-sheet rectangle lives in a sprite factory in `Sprites/`. Game objects ask a factory for a sprite and only decide where it is drawn; they never touch sprite-sheet coordinates themselves.
- **Data-driven rooms:** `RoomLoader` turns each kind of object named in a room file into an object through a table of constructors, so adding a new kind of enemy or block to the files means adding one line to a table, not a new `switch` case.
- **Collisions are detected in one place and handled by the objects:** `CollisionManager` only finds what is touching and tells the objects involved (`TakeDamage`, `OnBlocked`, `Hit`, `Push`); each object decides what that means for it.

## Tools and processes

- **GitHub** for version control. Pull requests were used for some changes (PR #3 was reviewed on GitHub before being merged); most changes were committed straight to `master`.
- **GitHub Projects** for the task board and burndown chart.
- **.NET code analyzers (Roslyn)** with Microsoft's recommended rules. The build has 0 warnings; every warning the analyzers found, and how it was fixed or why it was suppressed, is in [CodeAnalysis.md](CodeAnalysis.md).
- **Code reviews** for readability and maintainability, kept in the `CodeReviews/` folder.
- **Sprint reflections:** [Sprint2Reflections.md](Sprint2Reflections.md) and [Sprint3Reflections.md](Sprint3Reflections.md).

## Credits

Sprite sheets ripped by Mister Mike, as credited on the sheets themselves. The rooms' layouts, doors, enemies and items were read off the *Level 1 Eagle (Quest 1)* map by Rick N. Bruns at [NESMaps](https://nesmaps.com/maps/Zelda/ZeldaLevel1Q1.html). *The Legend of Zelda* is © Nintendo.
