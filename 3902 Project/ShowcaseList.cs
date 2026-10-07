namespace CSE_3902_Project
{
    // A list of objects the player flips through one at a time; going past either end wraps around
    internal sealed class ShowcaseList<T>(T[] objects)
    {

        private int index;

        public T Current => objects[index];

        public void Next()
        {
            index = (index + 1) % objects.Length;
        }

        public void Previous()
        {
            index = (index - 1 + objects.Length) % objects.Length;
        }

    }
}
