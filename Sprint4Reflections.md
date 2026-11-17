How the team performed: This sprint the team turned the 18 rooms from Sprint 3 into a first level that can be played from start to finish.
Early in the sprint background music and muting were added, then the HUD (hearts, rupees, keys, bombs, the B and A items and the dungeon map) and an inventory screen for picking the B item,
both through pull requests. The rest came in the last two days: game states (playing, paused, inventory, room transition, game over and win), walking between rooms with the scrolling
transition, locked doors and keys, shutters, cracked walls that bombs open, the stairs down to the bow, and the NES sound effects for everything. Every point in the Sprint 3 feedback was
fixed: enemies take several hits and are knocked back, they drop items, bombs hurt Link, Blade Traps charge when Link lines up with them, blocks can be pushed, the bow passage is seen from
the side, Wallmasters grab Link and take him back to the entrance, and the Old Man talks and his fires shoot when he is attacked. The refactors from the instructor's sheet were done too
(CollisionManager split up, the Goriya states folded in, Enemy renamed to WanderingEnemy, enemy subfolders, the sword as a projectile, Game1 using IController and the game states), the magic
numbers and strings were named, and the level was checked against the NES so that keys and items appear when rooms are cleared, like the original. One analyzer warning came in with the HUD
and sat in the build for six days before a full rebuild found it; it was fixed, and the build has 0 warnings.

what worked was Building on the room files and the CollisionManager from Sprint 3 again: most new features were a new line in the room files or one new interface (ILockedDoor, IShutterDoor,
IBombableWall, IPushableBlock, ILinkGrabber), so the rooms and collisions did not need rewriting. Playing through the whole dungeon after each big change, and scripting play-throughs of it,
found bugs that testing one feature at a time would have missed, like getting trapped under the stairs after climbing out of the bow passage, and a bombed wall not showing on the map.
The code reviews also paid off: one pointed out that Link could still swing his sword while a Wallmaster was dragging him, which is now fixed.

Next plan is to Actually start the sprint's work in the first week, since most of this sprint was done in the last two days again, keep a task board and burndown chart again, and send
every change through a pull request with a full rebuild before merging, so warnings don't sit in the build. For Sprint 5 we want to add a title screen and the overworld, load the game's
settings from a file, fix the differences from the NES that are left (finding the bow and boomerang before using them, the map and compass on the HUD, Aquamentus aiming at Link), and add
gamepad controls.
