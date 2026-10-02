How the team performed: This sprint the team built out core gameplay systems in parallel. 
Link now supports 4-directional movement, idle/walk animation, sword-swing attacks, and bow/arrow projectiles, with keybinds (U/I) to cycle equipped weapons, 
hitboxes were deliberately deferred to focus on correct animation first.Separately, five enemy types (Zol, Stalfos, Gel, Keese, Goriya) and six environment/block objects 
(FireBlock, Stairs, SquareBlock, FishStatue, DragonStatue, BlueGap) were added, along with a command-pattern input system (ICommand, NextEnemyCommand, PreviousEnemyCommand,
etc.) wired into Game1 as a debug-cycling menu for reviewing assets in isolation. A Reset/Quit command set was also added for testing convenience.

what worked was Splitting work by subsystem (player, enemies, environment, input commands) let multiple people build independently without blocking each other,
and the debug command menu (cycle enemies/blocks with O/P and T/Y) made it easy to visually verify each new asset without needing full gameplay wired up yet.

Next plan is to Start wiring real state/logic into EnemyState (health, attack behavior) and begin connecting hitboxes now that animations and the enemy roster exist.
