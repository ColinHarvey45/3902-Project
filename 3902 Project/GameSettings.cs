namespace CSE_3902_Project
{
    // Gameplay numbers that more than one class needs. Numbers only one class uses are named constants in that class
    internal static class GameSettings
    {
        // How fast Link walks, and how fast he steps sideways round a corner he has caught (see CollisionManager)
        public const float LinkWalkSpeed = 3f; // pixels per frame

        // Random chances are given as a percentage, out of this many
        public const int Percent = 100;
    }
}
