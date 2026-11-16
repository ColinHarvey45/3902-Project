namespace CSE_3902_Project
{
    // A list the player flips through one at a time (e.g. the rooms); going past either end wraps around
    internal sealed class CyclingList<T>(T[] objects)
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

        // Makes item the current one; going to the next or previous one then carries on from there
        public void MoveTo(T item)
        {
            int position = System.Array.IndexOf(objects, item);
            if (position < 0)
                throw new System.ArgumentException("That isn't in the list", nameof(item));

            index = position;
        }

    }
}
