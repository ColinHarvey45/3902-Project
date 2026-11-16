using System.Collections.Generic;
using Audio;

namespace CSE_3902_Project
{
    // The name of every file the game loads from Content (see Content/Content.mgcb), kept in one place
    internal static class ContentNames
    {
        // --- Sprite sheets ---
        public const string LinkSheet = "TLOZLink-transparent2";
        public const string DungeonEnemiesSheet = "TLOZDungeonEnemies-transparent";
        public const string BossesSheet = "TLOZBosses-transparent";
        public const string EnemyDeathSheet = "TLOZEnemyDeath";
        public const string DungeonSheet = "TLOZDungeon-transparent";
        public const string NpcsSheet = "TLOZNPCs-transparent";
        public const string ItemsSheet = "TLOZItems";
        public const string HudAndPauseSheet = "TLOZHUDandPause-transparent";
        public const string FontSheet = "TLOZFont-transparent";

        // --- Sound ---
        public const string Music = "BackgroundMusic";

        // Each sound effect's file, in Content/Sounds (see Sounds/CREDITS.txt)
        private const string SoundFolder = "Sounds/";
        public static readonly IReadOnlyDictionary<Sound, string> SoundEffects = new Dictionary<Sound, string>
        {
            [Sound.SwordSlash] = SoundFolder + "LOZ_Sword_Slash",
            [Sound.ArrowBoomerang] = SoundFolder + "LOZ_Arrow_Boomerang",
            [Sound.BombDrop] = SoundFolder + "LOZ_Bomb_Drop",
            [Sound.BombBlow] = SoundFolder + "LOZ_Bomb_Blow",
            [Sound.LinkHurt] = SoundFolder + "LOZ_Link_Hurt",
            [Sound.LinkDie] = SoundFolder + "LOZ_Link_Die",
            [Sound.LowHealth] = SoundFolder + "LOZ_LowHealth",
            [Sound.EnemyHit] = SoundFolder + "LOZ_Enemy_Hit",
            [Sound.EnemyDie] = SoundFolder + "LOZ_Enemy_Die",
            [Sound.BossHit] = SoundFolder + "LOZ_Boss_Hit",
            [Sound.BossScream] = SoundFolder + "LOZ_Boss_Scream1",
            [Sound.GetItem] = SoundFolder + "LOZ_Get_Item",
            [Sound.GetHeart] = SoundFolder + "LOZ_Get_Heart",
            [Sound.GetRupee] = SoundFolder + "LOZ_Get_Rupee",
            [Sound.Fanfare] = SoundFolder + "LOZ_Fanfare",
            [Sound.KeyAppear] = SoundFolder + "LOZ_Key_Appear",
            [Sound.DoorUnlock] = SoundFolder + "LOZ_Door_Unlock",
            [Sound.Stairs] = SoundFolder + "LOZ_Stairs",
            [Sound.Secret] = SoundFolder + "LOZ_Secret",
            [Sound.Text] = SoundFolder + "LOZ_Text"
        };
    }
}
