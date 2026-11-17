# Code Analysis

We use the **.NET code-quality analyzers (Roslyn)** to check our code. This document lists every warning they reported in each sprint and what we did about each one: fixed it, or suppressed it with a written reason.

## Setup

- **Tool:** the .NET code analyzers that come with the .NET SDK (we used SDK 9.0.318).
- **Rule set:** Microsoft's *recommended* rules, turned on with this line in `3902 Project/3902 Project.csproj`:
  ```xml
  <AnalysisLevel>latest-recommended</AnalysisLevel>
  ```
- **How to see the warnings:** do a full rebuild (`dotnet build "3902 Project.slnx" --no-incremental`, or Build → Rebuild Solution in Visual Studio). Warnings appear in the build output, or in the Error List in Visual Studio. A normal build is not enough: see *Lesson learned* under Sprint 3.
- **Dates run:** 2026-10-06, on the code at the end of Sprint 2; 2026-10-26, on every Sprint 3 commit; 2026-11-16, on every Sprint 4 commit.

## Sprint 2

### Summary

The first build with the analyzers on reported **106 warnings from 4 rules**. We fixed 104 and suppressed 2. **The build now has 0 warnings.**

| Rule | What it means | Count | What we did |
|---|---|---|---|
| CA1852 | A class can be marked `sealed` because nothing inherits from it | 82 | Fixed |
| CA1805 | A field or property is set to the value it would have anyway (e.g. `= 0f`, `= false`, `= null`) | 22 | Fixed |
| CA1707 | A name contains underscores | 1 | Suppressed |
| CA1859 | A field could use a more specific type to run faster | 1 | Suppressed |

### Fixed

#### CA1852: type can be sealed (82)
**Fix:** we added `sealed` to every class that nothing inherits from. It tells readers the class is not designed to be extended, and lets .NET call its methods slightly faster. If a later sprint needs to inherit from one of these classes, we just remove `sealed` from that class.

Base classes that other classes do inherit from (`Enemy`, `HoppingEnemy`, `Block`, `Item`) were not flagged and stay unsealed.

| Folder | How many | Classes sealed |
|---|---|---|
| project root | 1 | ShowcaseList |
| `Commands/` | 14 | LinkFireArrowCommand, LinkMoveCommand, LinkPlaceBombCommand, LinkSwordAttackCommand, LinkTakeDamageCommand, LinkThrowBoomerangCommand, NextBlockCommand, NextEnemyCommand, NextItemCommand, PreviousBlockCommand, PreviousEnemyCommand, PreviousItemCommand, QuitCommand, ResetCommand |
| `Enemies/` | 19 | Aquamentus, AquamentusAttackingState, AquamentusWalkingState, BladeTrap, BladeTrapChargingState, BladeTrapReturningState, BladeTrapWaitingState, Gel, Goriya, GoriyaThrowingState, GoriyaWalkingState, HopperHoppingState, HopperRestingState, Keese, KeeseFlyingState, KeeseRestingState, Stalfos, Wallmaster, Zol |
| `Environment/` | 16 | BlackTile, BlueGap, BombedWallOpening, DiamondLockedDoor, DragonStatue, FireBlock, FishStatue, FloorTile, KeyholeLockedDoor, Ladder, OpenDoor, SandTile, SquareBlock, Stairs, Wall, WhiteBrick |
| `Input/` | 1 | KeyboardController |
| `Items/` | 12 | BombItem, BoomerangItem, Bow, Clock, Compass, Fairy, Heart, HeartContainer, Key, Map, Rupee, TriforcePiece |
| `Movement/` | 1 | RandomMovement |
| `Npcs/` | 1 | OldMan |
| `Player/` | 6 | Link, LinkAttackingState, LinkDamagedState, LinkHealthyState, LinkStandingState, LinkWalkingState |
| `Projectiles/` | 4 | Arrow, Bomb, Boomerang, Fireball |
| `Sprites/` | 7 | BlockSpriteFactory, EnemySpriteFactory, ItemSpriteFactory, LinkSpriteFactory, NpcSpriteFactory, ProjectileSpriteFactory, Sprite |

#### CA1805: explicit default value (22)
**Fix:** we removed the redundant initializer. C# already starts numbers at `0`, `bool`s at `false` and references at `null`, so the behaviour is unchanged.

| File | Member | Before (now without the `= ...` part) |
|---|---|---|
| `Enemies/Goriya.cs` | `boomerang` | `private Boomerang boomerang = null;` |
| `Enemies/States/AquamentusAttackingState.cs` | `timer` | `private float timer = 0f;` |
| `Enemies/States/AquamentusWalkingState.cs` | `timer` | `private float timer = 0f;` |
| `Enemies/States/BladeTrapWaitingState.cs` | `timer` | `private float timer = 0f;` |
| `Enemies/States/HopperHoppingState.cs` | `timer` | `private float timer = 0f;` |
| `Enemies/States/HopperRestingState.cs` | `timer` | `private float timer = 0f;` |
| `Enemies/States/KeeseFlyingState.cs` | `timer` | `private float timer = 0f;` |
| `Enemies/States/KeeseRestingState.cs` | `timer` | `private float timer = 0f;` |
| `Movement/RandomMovement.cs` | `timer` | `private float timer = 0f;` |
| `Player/Link.cs` | `boomerangInFlight` | `private bool boomerangInFlight = false;` |
| `Player/Link.cs` | `requestedDirection` | `private Direction? requestedDirection = null;` |
| `Projectiles/Arrow.cs` | `IsFinished` | `public bool IsFinished { get; private set; } = false;` |
| `Projectiles/Bomb.cs` | `fuseTimer` | `private float fuseTimer = 0f;` |
| `Projectiles/Bomb.cs` | `isExploding` | `private bool isExploding = false;` |
| `Projectiles/Boomerang.cs` | `IsFinished` | `public bool IsFinished { get; private set; } = false;` |
| `Projectiles/Boomerang.cs` | `isReturning` | `private bool isReturning = false;` |
| `Projectiles/Boomerang.cs` | `stateTimer` | `private float stateTimer = 0f;` |
| `Projectiles/Fireball.cs` | `IsFinished` | `public bool IsFinished { get; private set; } = false;` |
| `ShowcaseList.cs` | `index` | `private int index = 0;` |
| `Sprites/Sprite.cs` | `IsFinished` | `public bool IsFinished { get; private set; } = false;` |
| `Sprites/Sprite.cs` | `currentFrame` | `private int currentFrame = 0;` |
| `Sprites/Sprite.cs` | `frameTimer` | `private float frameTimer = 0f;` |

### Suppressed

Both suppressions are in `3902 Project/GlobalSuppressions.cs`, each with its reason, and each applies only to the one place it names.

#### CA1707: underscores in the namespace `CSE_3902_Project` (1)
**Why we kept it:** a C# name can't start with a digit, so the underscores are what keep "3902" readable in our namespace. Renaming it would mean editing every file that uses `Game1` without changing any behaviour.

#### CA1859: `Game1.link` could be typed as `Link` instead of `IPlayer` (1)
**Why we kept it:** this is a deliberate design choice. `Game1`, and the commands it creates, only know about the player through the `IPlayer` interface, so Link's class can change without touching them. The speed gain the analyzer suggests is for one call per frame, which is not noticeable in this game.

## Sprint 3

### Summary

Code written in Sprint 3 added **3 warnings, all CA1852**. We fixed all 3. The two Sprint 2 suppressions still apply and nothing new was suppressed. **The build has 0 warnings again**, checked with a full rebuild of every Sprint 3 commit on 2026-10-26.

| Rule | Where | Added | Fixed |
|---|---|---|---|
| CA1852 | `Commands/NextRoomCommand.cs` | `a962550` (Oct 15, switching rooms with O / P) | `3d111a2` (Oct 26) |
| CA1852 | `Commands/PreviousRoomCommand.cs` | `a962550` (Oct 15) | `3d111a2` (Oct 26) |
| CA1852 | `Colliders/BoxCollider.cs` | `349a6d8` (Oct 19, the first colliders) | `3d111a2` (Oct 26) |

So the build had 0 warnings until Oct 15, 2 from Oct 15, 3 from Oct 19, and 0 again from Oct 26.

### Fixed

#### CA1852: type can be sealed (3)
**Fix:** the same as in Sprint 2: we added `sealed` to `NextRoomCommand`, `PreviousRoomCommand` and `BoxCollider`, since nothing inherits from them. Every other class added in Sprint 3 (`Room`, `RoomManager`, `RoomLoader`, `ProjectileManager`, `MouseController`, `EnemyHealth`, `CollisionManager`, ...) was `sealed` or `static` from the start.

### Suppressed

Nothing new. Both Sprint 2 suppressions are still needed: the namespace is still `CSE_3902_Project`, and `Game1` still holds Link as an `IPlayer`.

### Lesson learned

A normal (incremental) build only checks the files it has to recompile, so it can report **0 warnings even when there are some**. On Oct 19 a normal build said 0 while a full rebuild of the same code showed all 3. That is likely why these warnings stayed for a week and a half. From now on we check with a full rebuild before committing.

### Not covered by these rules

Unused `using` lines are not reported by the recommended rules. We removed the ones found in code review (e.g. in `ICollider.cs` and `Boomerang.cs`) by hand.

## Sprint 4

### Summary

Code written in Sprint 4 added **1 warning, CA1305**. We fixed it. Nothing new was suppressed, and one Sprint 2 suppression moved to a different field (see below). **The build has 0 warnings**, checked with a full rebuild of every Sprint 4 commit on 2026-11-16.

| Rule | Where | Added | Fixed |
|---|---|---|---|
| CA1305 | `UI/Hud.cs`, in `DrawNumber` | `8ba7336` (Nov 9, the HUD) | `bb48d92` (Nov 15) |

So the build had 0 warnings until Nov 9, 1 from Nov 9, and 0 again from Nov 15.

### Fixed

#### CA1305: a number turned into text without saying which culture to use (1)
`DrawNumber`, which draws the rupee, key and bomb counts in the HUD, called `int.ToString()`. That uses the computer's region settings, so the analyzer warns that the text could come out differently on a computer set up for another country.

**Fix:** we changed it to `ToString(CultureInfo.InvariantCulture)`, which always gives plain digits, so the HUD draws the counts the same way on every computer. Every class added in Sprint 4 (the game states, `TextRenderer`, `AudioManager`, `ProjectileFactory`, the three collision classes, `PushableBlock`, `BombableWall`, ...) was `sealed` or `static` from the start, so CA1852 didn't come up again.

### Suppressed

Nothing new. Both Sprint 2 suppressions are still needed, but the CA1859 one is now on a different field:

- **CA1707:** the namespace is still `CSE_3902_Project`.
- **CA1859:** `Game1` no longer holds Link at all: Link and the game's flow moved into `GameStates/GameStateMachine.cs` (`c97e0c6`, Nov 15), as the instructor's Sprint 3 feedback asked. `Game1` now holds its keyboard for the keys that work everywhere (quit, restart, mute) as an `IController`, which the instructor also asked for, and the same rule suggests typing it as `KeyboardController` instead. We kept the interface for the same reason as before, and moved the suppression in `GlobalSuppressions.cs` to that field (`Game1.globalKeys`) in the same commit.

### Lesson learned

The warning came in with the HUD's pull request and stayed in the build for six days, because nobody did a full rebuild when the pull request was merged. A full rebuild should be part of reviewing a pull request, not only of committing.

## Going forward
The build should stay at 0 warnings. Check with a full rebuild before committing. When new code adds a warning, fix it, or suppress it in `GlobalSuppressions.cs` with a reason and add it to this document.
