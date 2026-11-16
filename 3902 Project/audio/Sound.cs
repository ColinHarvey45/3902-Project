namespace Audio
{
    // The game's sound effects, from the NES game (see Content/Sounds/CREDITS.txt). AudioManager knows which
    // file each one is
    internal enum Sound
    {
        // Link
        SwordSlash,
        ArrowBoomerang,
        BombDrop,
        BombBlow,
        LinkHurt,
        LinkDie,
        LowHealth,

        // Enemies
        EnemyHit,
        EnemyDie,
        BossHit,
        BossScream,

        // Picking things up
        GetItem,
        GetHeart,
        GetRupee,
        Fanfare,

        // The dungeon
        DoorUnlock,
        Stairs
    }
}
