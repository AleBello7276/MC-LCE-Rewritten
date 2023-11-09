namespace com.mojang.minecraft.level.levelgen.synth
{
   public class ImprovedNoise : Synth {
      private int[] p;
      public double scale;
      private Random random;
      public ImprovedNoise() {
         random = new Random();
      }

      public ImprovedNoise(Random random) {
         this.p = new int[512];

         int i;
         for(i = 0; i < 256; this.p[i] = i++) {
         }

         for(i = 0; i < 256; ++i) {
            int j = new Random().Next(256 - i) + i;
            int tmp = this.p[i];
            this.p[i] = this.p[j];
            this.p[j] = tmp;
            this.p[i + 256] = this.p[i];
         }

      }

      public double noise(double x, double y, double z) {
         int X = (int)Math.Floor(x) & 255;
         int Y = (int)Math.Floor(y) & 255;
         int Z = (int)Math.Floor(z) & 255;
         x -= Math.Floor(x);
         y -= Math.Floor(y);
         z -= Math.Floor(z);
         double u = this.fade(x);
         double v = this.fade(y);
         double w = this.fade(z);
         int A = this.p[X] + Y;
         int AA = this.p[A] + Z;
         int AB = this.p[A + 1] + Z;
         int B = this.p[X + 1] + Y;
         int BA = this.p[B] + Z;
         int BB = this.p[B + 1] + Z;
         return this.lerp(w, this.lerp(v, this.lerp(u, this.grad(this.p[AA], x, y, z), this.grad(this.p[BA], x - 1.0D, y, z)), this.lerp(u, this.grad(this.p[AB], x, y - 1.0D, z), this.grad(this.p[BB], x - 1.0D, y - 1.0D, z))), this.lerp(v, this.lerp(u, this.grad(this.p[AA + 1], x, y, z - 1.0D), this.grad(this.p[BA + 1], x - 1.0D, y, z - 1.0D)), this.lerp(u, this.grad(this.p[AB + 1], x, y - 1.0D, z - 1.0D), this.grad(this.p[BB + 1], x - 1.0D, y - 1.0D, z - 1.0D))));
      }

      public double fade(double t) {
         return t * t * t * (t * (t * 6.0D - 15.0D) + 10.0D);
      }

      public double lerp(double t, double a, double b) {
         return a + t * (b - a);
      }

      public double grad(int hash, double x, double y, double z) {
         int h = hash & 15;
         double u = h < 8 ? x : y;
         double v = h < 4 ? y : (h != 12 && h != 14 ? z : x);
         return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
      }

      public override double getValue(double x, double y) {
         return this.noise(x, y, 0.0D);
      }
   }

}


