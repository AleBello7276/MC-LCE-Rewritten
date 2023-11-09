using com.mojang.minecraft;

namespace com.mojang.minecraft.level
{
   public class DistanceSorter : IComparer<Chunk> {
      private Player player;

      public DistanceSorter(Player player) {
         this.player = player;
      }

      public int Compare(Chunk c0, Chunk c1) {
         return c0.distanceToSqr(this.player) < c1.distanceToSqr(this.player) ? -1 : 1;
      }
   }
}


