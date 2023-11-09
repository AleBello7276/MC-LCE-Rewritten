namespace com.mojang.minecraft.level.levelgen.synth
{
   public class PerlinNoise : Synth {
      private ImprovedNoise[] noiseLevels;
      private int levels;
      private Random random;
      public PerlinNoise(int levels) {
        random = new Random();
        this.levels = levels;
      }

      public PerlinNoise(Random random, int levels) {
         this.levels = levels;
         this.noiseLevels = new ImprovedNoise[levels];

         for(int i = 0; i < levels; ++i) {
            this.noiseLevels[i] = new ImprovedNoise(random);
         }

      }

      public override double getValue(double x, double y) {
         double value = 0.0D;
         double pow = 1.0D;

         for(int i = 0; i < this.levels; ++i) {
            value += this.noiseLevels[i].getValue(x / pow, y / pow) * pow;
            pow *= 2.0D;
         }

         return value;
      }
   }

}



