
using com.mojang.minecraft;



namespace com.mojang.minecraft.level
{
   public class DirtyChunkSorter : IComparer<Chunk> {
      private Player player;

      public DirtyChunkSorter(Player player) {
         this.player = player;
      }

      public int Compare(Chunk c0, Chunk c1) {
         bool i0 = c0.visible;
         bool i1 = c1.visible;
         if (i0 && !i1) {
            return -1;
         } else if (i1 && !i0) {
            return 1;
         } else {
            return c0.distanceToSqr(this.player) < c1.distanceToSqr(this.player) ? -1 : 1;
         }
      }
    }
}


