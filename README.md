# CSE 3902 Project

A remake of the first dungeon of *The Legend of Zelda* (NES), built in C# with MonoGame.

GitHub: https://github.com/ColinHarvey45/3902-Project.git

## What Sprint 4 does

The first dungeon (Level 1, "the Eagle") can be played from start to finish, as in the NES. The game starts in the entrance. Walking out through a doorway scrolls the screen to the room next to it, keys open locked doors, bombs blow holes in cracked walls, shutters close behind Link until the room is cleared, and the stairs lead down to the passage with the bow. Link has hearts, and the HUD shows them along with his rupees, keys, bombs, items and the dungeon map. Enemies take several hits, are knocked back, and sometimes leave a rupee, a heart or bombs; clearing some rooms makes a key or an item appear. Running out of hearts is game over, and Enter starts the level again; picking up the triforce wins. Every action has its NES sound effect, over background music. The game can be paused (Space), and the inventory opened (F), at any time. The test room, with one of every object, is still one mouse click (or O) away from the entrance.

## Building and running

Open `3902 Project.slnx` in Visual Studio and run it, or run `dotnet run --project "3902 Project"` from this folder. The game opens in a 1024 x 928 window: the HUD (224 pixels high) above one dungeon room (1024 x 704). Link starts just inside the entrance, with 3 hearts and 8 bombs.

The room files in `3902 Project/Levels/Dungeon1/` are copied next to the game when it is built. If a room file has a mistake, the game stops with a message naming the file, the line and what is wrong.

## Controls

### Link
| Key | Action |
|---|---|
| W A S D or arrow keys | Walk up, left, down, right. Link only walks in four directions; if two are held, up/down wins over left/right. If he only just catches the corner of a block, he slides round it |
| Z or N | Swing the sword. Link can't move or use items until the swing finishes |
| 1 | Fire an arrow |
| 2 | Throw the boomerang. Only one can be out at a time; it comes back to Link |
| 3 | Drop a bomb on the tile in front of Link. It explodes after about a second, and hurts Link too if he is in the blast. Each one uses up a bomb |
| X or B | Use the item in Link's B slot (bombs to start with; chosen on the inventory screen) |
| E | Hurt Link, for testing: he loses a heart, flashes for a second and is knocked back a little |

The number keys on the numpad work too. While a Wallmaster is holding Link, he can't walk, swing his sword or use items.

### The game
| Key | Action |
|---|---|
| Space | Pause: everything stops, with "PAUSED" over the room. Space again carries on |
| F | Open the inventory screen. The arrow keys move the cursor over Link's items, G puts the one under the cursor in his B slot, and F goes back to the game |
| M | Mute or unmute the music. The sound effects still play |
| Enter | On the game over or win screen: play the level again |
| R | Start the level again at any time: every room is reloaded from its file, and Link is back at the entrance with 3 hearts |
| Q or Escape | Quit |

### Jumping between rooms (for testing)
| Input | Action |
|---|---|
| Right mouse button, or P | Go to the next room |
| Left mouse button, or O | Go to the previous room |

Rooms go in the order of the list below and wrap around at both ends, so the test room is just before the entrance. Clicks only count inside the game window, and holding a button down changes the room only once.

## The rooms

The test room comes first, then the dungeon's rooms, roughly in the order you would explore them. Their layouts, doors, enemies, items and keys were read off the Level 1 map at NESMaps (see Credits), and each room's `Map` line puts it in the same place on the dungeon map, which is how the game knows which room is through each door.

| # | File | What's in it |
|---|---|---|
| 0 | `Room00_Test.csv` | **Test room:** one of every block, door, item, enemy and NPC in the dungeon (see below) |
| 1 | `Room01_Entrance.csv` | The entrance: 12 statues and some sand, and a locked door at the top |
| 2 | `Room02_KeeseWest.csv` | West of the entrance: 3 Keese. A key appears once they are dead |
| 3 | `Room03_StalfosEast.csv` | East of the entrance: 5 Stalfos, one of them carrying a key |
| 4 | `Room04_StalfosNorth.csv` | North of the entrance: 3 Stalfos |
| 5 | `Room05_StalfosKey.csv` | 5 Stalfos. A key appears once they are dead. The top wall is cracked |
| 6 | `Room06_KeeseNorthWest.csv` | 6 Keese. Coming in from room 5, the shutter shuts behind Link until they are dead |
| 7 | `Room07_Compass.csv` | 8 Keese and the compass. Reached by bombing the cracked wall in room 11 |
| 8 | `Room08_MovableBlock.csv` | 3 Gels, and a block that can be pushed once they are dead, which opens the shutter to the Old Man |
| 9 | `Room09_OldMan.csv` | The Old Man between two fires, and his message |
| 10 | `Room10_Map.csv` | 5 Gels and the dungeon map. The bottom wall is cracked |
| 11 | `Room11_Boomerang.csv` | 3 Goriyas. The boomerang appears once they are dead. The bottom wall is cracked |
| 12 | `Room12_Wallmasters.csv` | 8 Wallmasters, hiding in the walls, and a key |
| 13 | `Room13_WaterMaze.csv` | A maze of water with 3 Stalfos, one of them carrying a key |
| 14 | `Room14_GoriyaKey.csv` | 3 Goriyas around a ring of water. A key appears once they are dead |
| 15 | `Room15_BladeTraps.csv` | 4 Blade Traps, and the stairs down to the bow behind a diamond of blocks; the leftmost block can be pushed up |
| 16 | `Room16_BowPassage.csv` | The underground passage below the stairs, seen from the side: the bow and 4 Keese |
| 17 | `Room17_Aquamentus.csv` | Aquamentus, the boss. When he dies the heart container appears and the shutter to the triforce opens |
| 18 | `Room18_Triforce.csv` | The triforce piece |

### The way through

There are just enough keys, as in the NES. From the entrance, get the keys in rooms 2 and 3, then go up through rooms 4 and 5 (a key) and left into room 6. Go up into room 8, clear it and push the block, and the Old Man is on the left. Go right into room 10 for the map and up into room 13 (a key), then up into room 14 (a key) and left into room 15: push the leftmost block up, and the stairs lead down to the bow. Back in room 10, go right into room 11 for the boomerang (bomb its bottom wall for the compass in room 7), then right into room 12 (a key), up to Aquamentus in room 17, and right to the triforce.

### The test room

- **Doors:** a keyhole locked door at the top, a bombed wall opening on the left, a shutter (diamond door) on the right, and an open door at the bottom. (A plain wall piece, for a wall with no door, and a cracked wall, are used in the dungeon rooms.)
- **Blocks (top row):** square block, fish statue, dragon statue, blue gap, black tile, sand tile, stairs, white brick, ladder, and two fires with the Old Man between them.
- **Enemies (middle):** Stalfos, Zol, Gel, Keese, Goriya, Wallmaster, Blade Trap and Aquamentus.
- **Items (second row from the bottom):** heart, heart container, rupee, triforce piece, key, map, compass, bow, boomerang, bomb, clock and fairy.

| Character | Sword hits to kill | What it does |
|---|---|---|
| Stalfos | 2 | Walks in a random direction, turning every couple of seconds |
| Zol | 2 | Creeps along in slow hops with pauses in between. The sword splits it into two Gels |
| Gel | 1 | Darts about in quick hops with short pauses |
| Keese | 1 | Takes off, flutters in all eight directions while speeding up and slowing down, then lands for a moment |
| Goriya | 3 | Walks like a Stalfos, facing the way it walks; sometimes stops to throw a boomerang and waits for it to come back |
| Wallmaster | 3 | Hides in the walls. When Link walks along a wall, one comes out a little ahead of him, creeps along the wall past him and goes back in; only one is out at a time. If it catches Link, it drags him towards the wall, and he is taken back to the entrance |
| Blade Trap | can't be hurt | Waits, shoots out when Link lines up with it across or down the room, then slides slowly back |
| Aquamentus | 6 | The boss: paces back and forth, roars, and every few seconds opens its mouth to breathe three fireballs |
| Old Man | - | Stands still while his message ("EASTMOST PENNINSULA IS THE SECRET.") types out letter by letter. Attacking him makes the fires beside him shoot fireballs at Link |

### Room files

Each room is one CSV file in `3902 Project/Levels/Dungeon1/`, loaded in file-name order by `Rooms/RoomLoader.cs`. Every line adds something to the room:

```
Map,3,4                        where the room is on the dungeon map (column, row)
Door,Up,KeyholeLockedDoor      the door (or plain wall) in the gap in one wall
Floor,..QQ....QQ..             one row of the 12 x 7 floor (7 of these), one symbol per tile
Enemy,Stalfos,6,4              a kind of enemy (its class name) and the floor tile it starts on
Enemy,Stalfos,6.5,4,Key        an enemy carrying an item, which it drops when it dies
Reward,Key,6.25,1              an item that appears once every enemy in the room is dead
Npc,OldMan,5.5,2
Item,Key,4,5
Block,FireBlock,9,0
Warp,6,3,Room16_BowPassage,1,-1   stepping on that tile takes Link to that tile of another room (the stairs)
SideView                       the room is seen from the side (the underground passage)
```

Door kinds: `OpenDoor`, `KeyholeLockedDoor` (opened with a key), `DiamondLockedDoor` (a shutter, opened by clearing the room), `BombableWall` (a cracked wall, opened with a bomb), `BombedWallOpening` and `Wall`. Floor symbols: `.` floor, `Q` square block, `P` pushable block, `^` `v` `<` `>` a block that can only be pushed that way, `F` fish statue, `D` dragon statue, `G` blue gap (water), `B` black tile, `S` sand, `T` stairs, `W` white brick, `L` ladder. Tiles count from the top-left of the floor; halves like `5.5` are allowed. Lines starting with `#` are comments. The underground passage has no ordinary walls, so it uses `Tiles` lines, which describe the whole 16 x 11 screen instead. The full description is at the top of `RoomLoader.cs`.

## Game states

`GameStates/GameStateMachine.cs` holds Link, the rooms and the HUD, and hands every update and draw to the state the game is in. Each state has its own keys, so the keys only do what makes sense in that state.

| State | When | What happens |
|---|---|---|
| Playing | Most of the time | The game runs |
| Room transition | Link walks out through a doorway | Both rooms slide across the screen for a second, and Link comes in just inside the door on the other side |
| Paused | Space | Everything stops, with "PAUSED" over the room, until Space is pressed again |
| Item selection | F | The inventory screen (see Controls) |
| Game over | Link runs out of hearts | The room turns red, then "GAME OVER"; Enter starts the level again |
| Win | Link picks up the triforce | The fanfare plays, and "YOU GOT THE TRIFORCE!"; Enter plays again, Q quits |

## Fighting, hearts and items

- **Link** starts with 3 hearts. Each hit takes one away, and he flashes for about a second, during which nothing can hurt him again. At one heart the low-health beep plays.
- **Damage:** the sword takes 1 from an enemy, an arrow 2, and a bomb 4 (from every enemy in the blast). The boomerang stuns most enemies for 2 seconds (not Aquamentus), but kills Gels and Keese. The health of each enemy is in the table above.
- **An enemy that survives a hit** flashes and is knocked back a tile, unless a wall or block is in the way. **An enemy that dies** leaves a puff of smoke, and sometimes an item: a rupee (25% of the time), a heart (15%) or bombs (10%).

| Item | What picking it up does |
|---|---|
| Heart | Gives back one heart |
| Heart container | One more heart, and fills them all |
| Rupee | One more rupee |
| Bombs | Four more bombs |
| Key | One more key |
| Map, compass | Shown on the inventory screen |
| Triforce piece | Wins the game |
| Bow, boomerang, clock, fairy | Nothing yet (see Differences) |

## Sound

Every sound is a recording from the NES game (see Credits): the sword, arrows and the boomerang, bombs being dropped and exploding, Link being hurt and dying, the low-health beep, enemies being hit and dying, Aquamentus's roar and hits, picking up items, hearts and rupees, a key appearing, doors unlocking and shutters closing, the "secret" sound when a block is pushed or a wall is blown open, the stairs, the Old Man's message, and the triforce fanfare. The background music plays quietly under them, and M mutes only the music. `audio/AudioManager.cs` loads and plays them all.

## Collisions

`Collisions/CollisionManager.cs` runs once a frame, after everything has moved, and calls three classes that each handle one kind of collision: `MovementCollisions` (what stops or moves Link, enemies and projectiles), `CombatCollisions` (weapons, enemies and Link getting hurt) and `PickupCollisions` (items, and opening locked doors and cracked walls).

| What touches what | What happens |
|---|---|
| Link and an enemy | Link is hurt and knocked back a little. A Wallmaster catches him instead |
| Link and a fireball, or a Goriya's boomerang | Link is hurt the same way; the fireball disappears and the boomerang flies back to its Goriya |
| Link and a bomb's explosion | Link is hurt, even by his own bomb |
| Link's sword and an enemy | The enemy is hurt (Blade Traps can't be hurt) |
| An arrow and an enemy | The first enemy it reaches is hurt, and the arrow disappears |
| Link's boomerang and an enemy | The first enemy it reaches is stunned, and the boomerang flies back to Link |
| A bomb's explosion and enemies | Every enemy in the blast (two tiles square) is hurt |
| Any of Link's weapons and the Old Man | The fires beside the Old Man start shooting at Link |
| Link and a wall, a solid block or a closed door | Link is stopped. Solid: square blocks, statues, water (blue gap), white bricks, fire, and plain walls, cracked walls, locked doors and shut shutters |
| Link and a locked door, with a key | The door opens, on both sides, and the key is used up |
| A bomb's explosion and a cracked wall | A hole opens in it, on both sides |
| Link pushing a pushable block | After half a second it slides one tile, once. It only moves once the room's enemies are dead, and some only one way |
| Link and an open doorway | Link walks through, and the screen scrolls to the next room |
| Link and the stairs | Link goes down to the underground passage |
| Link and the Old Man | Link is stopped |
| Link and an item | Link picks it up, and it disappears from the room |
| An enemy and a wall or solid block | Most enemies step back and walk another way. Keese fly over blocks; a Blade Trap stops and slides back home; Aquamentus turns around |
| An arrow or a fireball and a wall or solid block | It disappears |
| A boomerang and a wall or solid block | It turns back early |

Link walks over floor, sand, black tiles, stairs, ladders, open doors and bombed openings. Enemies pass through each other, as in the original game.

## Differences from the original game

These are deliberate, or left for later:

- **Bombs hurt Link**, which they don't in the NES. The instructor asked for this in the Sprint 3 feedback.
- **There is no overworld**, so the entrance's bottom door doesn't lead anywhere.
- **Link can fire arrows and throw the boomerang from the start**, before he has found the bow or the boomerang, and arrows don't cost rupees. Picking up the bow, boomerang, clock or fairy does nothing.
- **The little map in the HUD shows every room from the start**, instead of only once Link has the map, and the compass doesn't show where the triforce is.
- **Aquamentus always breathes his fireballs to the left**, in a spread of three, instead of aiming them at Link.
- **Stalfos and Goriyas walk freely** instead of tile by tile.
- **Knockback pushes Link a few pixels the opposite way to how he is facing**, rather than away from whatever hit him.
- **Link doesn't change pose when he uses an item.**
- **Items left by enemies stay until they are picked up**; in the NES they disappear after a while.

## Known bugs

- **Link keeps his position when you change rooms with the mouse or O / P.** He can land on an enemy (and is hurt) or inside a block (and is pushed out of it). Walking through doors doesn't have this problem.
- **Link is drawn on top of every enemy**, so an enemy that walks under him is hidden until it walks out again, and a Wallmaster holding him is drawn under him. Arrows, boomerangs, bombs and fireballs are drawn on top of Link.
- **In the underground passage, Link's feet are drawn slightly into the bricks.**

## How the code is organized

| Folder | What's in it |
|---|---|
| `GameStates/` | `GameStateMachine` and one class per game state (playing, room transition, paused, item selection, game over, win) |
| `Rooms/` | `Room` (a room and everything in it), `RoomManager` (every room, which one Link is in, and which room is through each door), `RoomLoader` (builds rooms from their files), `RoomLayout` (where the walls, floor tiles and door gaps are on screen) and `SideView` (walking and climbing in the underground passage) |
| `Levels/Dungeon1/` | One CSV file per room |
| `Collisions/` | `CollisionManager`, and `MovementCollisions`, `CombatCollisions` and `PickupCollisions`, which it calls |
| `Colliders/` | `BoxCollider`, the hitbox that follows each object |
| `Player/` | Link, his inventory, and his states in `Player/States/` (standing, walking, attacking, held by a Wallmaster, plus healthy and damaged) |
| `Enemies/` | Every enemy, in `Wanderers/`, `Hoppers/`, `Bosses/` and `Traps/`; their shared base class `WanderingEnemy`, their health (`EnemyHealth`) and death puff; and their states in `Enemies/States/` |
| `Npcs/` | Characters that don't fight (the Old Man) |
| `Items/` | Items Link can pick up, and `ItemDrops`, which picks what a dead enemy leaves |
| `Environment/` | Blocks, tiles, doors and walls |
| `Projectiles/` | The sword swing, arrows, boomerangs, bombs and fireballs; `ProjectileFactory`, which creates them; and `ProjectileManager`, which holds every one in the room whoever fired it |
| `UI/` | The HUD, the inventory screen, and `TextRenderer`, which draws text from the NES font |
| `audio/` | `AudioManager`, which plays the music and sound effects |
| `Sprites/` | The `Sprite` class and one sprite factory per kind of object |
| `Commands/` | One command per action a key or mouse button can trigger |
| `Input/` | The keyboard and mouse controllers |
| `Movement/` | Random wandering shared by enemies and the fairy |
| `Interfaces/` | The interfaces everything above implements |

`ContentNames.cs` names every sprite sheet and sound file, and `GameSettings.cs` holds the gameplay numbers more than one class uses. Numbers only one class uses are named constants in that class.

**Design patterns:**
- **State:** the game hands each frame to its current state (playing, paused, game over, ...), which switches to another when it should. Link, Gel, Zol, Keese, the Blade Trap and Aquamentus each hand their behaviour to a state object too. Link has two at once: what he's doing, and whether he's hurt.
- **Command:** each key and mouse button is bound to a command object, and `KeyboardController` or `MouseController` runs it. Each game state binds its own keys, and `Game1` binds the few that work everywhere (quit, restart, mute). Keys can run once per press (most keys) or every frame they're held (movement). Link and the rooms never read the keyboard or mouse themselves.
- **Factory:** every sprite-sheet rectangle lives in a sprite factory in `Sprites/`. Link, the enemies and the Old Man's fires create what they fire through `ProjectileFactory`, so they only depend on `IProjectile`.
- **Data-driven rooms:** `RoomLoader` turns each kind of object named in a room file into an object through a table of constructors, so adding a new kind of enemy or block to the files means adding one line to a table, not a new `switch` case.
- **Collisions are detected in one place and handled by the objects:** the collision classes only find what is touching and tell the objects involved (`TakeDamage`, `OnBlocked`, `Hit`, `Grab`, `Push`); each object decides what that means for it.
- **Interfaces for what objects can do:** the collisions and rooms ask what an object can do rather than what it is, e.g. `ILockedDoor`, `IShutterDoor`, `IBombableWall`, `IPushableBlock`, `ILinkGrabber` and `IHidingEnemy`.

## Sprint 5 ideas

What we looked into for the next sprint:

- **A title screen and start menu.** The NES title screen is already on a sprite sheet we have (`TLOZTitleScreen-transparent.png`), and a title state fits into the game state machine.
- **The overworld**, from `TLOZOverworld-transparent.png`, so the entrance's bottom door leads somewhere. Overworld screens could use the same room files with a new kind of `Tiles` line.
- **Settings from a file** (Link's speed, damage, drop chances), so the game can be tuned without recompiling. The numbers are already named in one place each, so this means loading them instead.
- **Fixing the differences above:** finding the bow and boomerang before using them, the map and compass on the HUD, Aquamentus aiming at Link, and drops disappearing.
- **A gamepad controller**, as another `IController`, so the game can be played with a controller as well as the keyboard.

## Tools and processes

- **GitHub** for version control. The HUD and the inventory screen came in through pull requests (#4 and #5); most other changes were committed straight to `master`.
- **GitHub Projects** for the task board and burndown chart in earlier sprints. We didn't keep a board in Sprint 4.
- **.NET code analyzers (Roslyn)** with Microsoft's recommended rules. The build has 0 warnings; every warning the analyzers found, and how it was fixed or why it was suppressed, is in [CodeAnalysis.md](CodeAnalysis.md).
- **Code reviews** for readability and maintainability, kept in the `CodeReviews/` folder.
- **Sprint reflections:** [Sprint2Reflections.md](Sprint2Reflections.md), [Sprint3Reflections.md](Sprint3Reflections.md) and [Sprint4Reflections.md](Sprint4Reflections.md).

## Credits

Sprite sheets ripped by Mister Mike, as credited on the sheets themselves. The rooms' layouts, doors, enemies, items and keys were read off the *Level 1 Eagle (Quest 1)* map by Rick N. Bruns at [NESMaps](https://nesmaps.com/maps/Zelda/ZeldaLevel1Q1.html). The sound effects are recordings from the NES game supplied by HelpTheWretched at [ZeldaSounds.com](https://noproblo.dayjo.org/zeldasounds/loz/) (see `3902 Project/Content/Sounds/CREDITS.txt`). The background music was added by Sean Kong. *The Legend of Zelda* is © Nintendo.
