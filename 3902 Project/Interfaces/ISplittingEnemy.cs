using System.Collections.Generic;

namespace Interfaces
{
    // An enemy that can split in two when hit, like the Zol into two Gels
    internal interface ISplittingEnemy : IDamageable
    {

        // What it split into, which takes its place in the room once it is gone; empty unless it has split
        public IReadOnlyList<IEnemy> Pieces { get; }

    }
}
