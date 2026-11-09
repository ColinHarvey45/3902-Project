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
