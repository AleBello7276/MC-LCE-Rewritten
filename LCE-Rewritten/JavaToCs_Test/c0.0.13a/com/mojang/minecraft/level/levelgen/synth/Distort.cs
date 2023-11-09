namespace com.mojang.minecraft.level.levelgen.synth
{
   public class Distort : Synth {
      private Synth source;
      private Synth distort;

      public Distort(Synth source, Synth distort) {
         this.source = source;
         this.distort = distort;
      }

      public override double getValue(double x, double y) {
         return this.source.getValue(x + this.distort.getValue(x, y), y);
      }
   }

}

