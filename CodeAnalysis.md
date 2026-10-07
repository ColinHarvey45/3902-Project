# Code Analysis (Sprint 2)

For Sprint 2 we used the **.NET code-quality analyzers (Roslyn)** to check our code. This document lists every warning they reported and what we did about each one: fixed it, or suppressed it with a written reason.

## Setup

- **Tool:** the .NET code analyzers that come with the .NET SDK (we used SDK 9.0.318).
- **Rule set:** Microsoft's *recommended* rules, turned on with this line in `3902 Project/3902 Project.csproj`:
  ```xml
  <AnalysisLevel>latest-recommended</AnalysisLevel>
  ```
- **How to see the warnings:** build the solution (`dotnet build "3902 Project.slnx"`, or Build in Visual Studio). Warnings appear in the build output, or in the Error List in Visual Studio.
- **Date run:** 2026-10-06, on the code at the end of Sprint 2.

## Summary

The first build with the analyzers on reported **106 warnings from 4 rules**. We fixed 104 and suppressed 2. **The build now has 0 warnings.**

| Rule | What it means | Count | What we did |
|---|---|---|---|
| CA1852 | A class can be marked `sealed` because nothing inherits from it | 82 | Fixed |
| CA1805 | A field or property is set to the value it would have anyway (e.g. `= 0f`, `= false`, `= null`) | 22 | Fixed |
| CA1707 | A name contains underscores | 1 | Suppressed |
| CA1859 | A field could use a more specific type to run faster | 1 | Suppressed |

## Fixed

### CA1852: type can be sealed (82)
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

### CA1805: explicit default value (22)
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

## Suppressed

Both suppressions are in `3902 Project/GlobalSuppressions.cs`, each with its reason, and each applies only to the one place it names.

### CA1707: underscores in the namespace `CSE_3902_Project` (1)
**Why we kept it:** a C# name can't start with a digit, so the underscores are what keep "3902" readable in our namespace. Renaming it would mean editing every file that uses `Game1` without changing any behaviour.

### CA1859: `Game1.link` could be typed as `Link` instead of `IPlayer` (1)
**Why we kept it:** this is a deliberate design choice. `Game1`, and the commands it creates, only know about the player through the `IPlayer` interface, so Link's class can change without touching them. The speed gain the analyzer suggests is for one call per frame, which is not noticeable in this game.

## Going forward
The build should stay at 0 warnings. When new code adds a warning, fix it, or suppress it in `GlobalSuppressions.cs` with a reason and add it to this document.
