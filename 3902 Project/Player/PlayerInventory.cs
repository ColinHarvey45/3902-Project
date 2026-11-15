using System;

namespace Player
{
    // What Link is carrying, shown in the HUD; items add to it when he picks them up
    internal sealed class PlayerInventory
    {
        // The HUD has room for two digits
        public const int MaxCount = 99;

        private const int StartingBombs = 8;

        public int Rupees { get; private set; }
        public int Keys { get; private set; }
        public int Bombs { get; private set; } = StartingBombs;

        // The dungeon's map and compass, which the pause screen shows once Link has found them
        public bool HasMap { get; private set; }
        public bool HasCompass { get; private set; }

        public void AddRupees(int amount)
        {
            Rupees = Math.Min(MaxCount, Rupees + amount);
        }

        public void AddKeys(int amount)
        {
            Keys = Math.Min(MaxCount, Keys + amount);
        }

        public void AddBombs(int amount)
        {
            Bombs = Math.Min(MaxCount, Bombs + amount);
        }

        public void CollectMap()
        {
            HasMap = true;
        }

        public void CollectCompass()
        {
            HasCompass = true;
        }

        // Uses up a bomb if he has one; false if he has none left
        public bool TryUseBomb()
        {
            if (Bombs == 0)
                return false;

            Bombs--;
            return true;
        }
    }
}
