How the team performed: This sprint the team turned the Sprint 2 asset showcase into an actual dungeon.
Rooms are now drawn with their walls, floor and doors, and are loaded from CSV files, one per room: all 18 rooms of Level 1 (read off the NESMaps Level 1 map, with their
real doors, enemies and items) plus a test room with one of every object in the dungeon, and the mouse (or O/P) jumps between them. Collisions were built up over the sprint:
colliders on Link, the enemies and projectiles, enemy health with a hurt flash and death, block collision, and finally one CollisionManager that handles every collision
(walls, blocks, closed doors, the Old Man, picking up items, arrows and fireballs stopping at walls, boomerangs turning back, and bombs hurting every enemy in the blast).
Three analyzer warnings also slipped in for over a week, because a normal build kept reporting 0 warnings; a full rebuild found them and they were fixed.

what worked was Splitting work by subsystem again (room system, room layouts and spawns, colliders and combat, block collision) let people build in parallel,
and reviewing code before relying on it paid off: the pull request review of the attack/collision branch found a duplicated fireball check and several smaller issues that were
then fixed, and the review of the room files suggested moving the rooms into CSV files, which is how they are stored now. Checking each room against the NESMaps map tile by tile
also made the dungeon layout match the original game.

Next plan is to Start the sprint's work in the first week instead of the last few days, send every change through a pull request with a review, and build the next features on
the CollisionManager and the room files: a HUD with hearts and an inventory, doors that open with keys and bombs, walking between rooms with the scrolling transition, and sound.
Furthermore, for Sprint 4 a full level one fully playable and the exact same as the OG game will be ready.
