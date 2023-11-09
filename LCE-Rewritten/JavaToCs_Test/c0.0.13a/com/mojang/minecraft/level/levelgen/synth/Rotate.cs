namespace com.mojang.minecraft.level.levelgen.synth
{
   public class Rotate : Synth {
      private Synth synth;
      private double sin;
      private double cos;

      public Rotate(Synth synth, double angle) {
         this.synth = synth;
         this.sin = Math.Sin(angle);
         this.cos = Math.Cos(angle);
      }

      public override double getValue(double x, double y) {
         return this.synth.getValue(x * this.cos + y * this.sin, y * this.cos - x * this.sin);
      }
}
}


