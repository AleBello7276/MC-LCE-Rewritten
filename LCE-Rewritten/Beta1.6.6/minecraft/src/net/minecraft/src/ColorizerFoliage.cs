namespace net.minecraft.src
{
	public class ColorizerFoliage {
		private static int[] foliageBuffer = new int[65536];

		public static void func_28152_a(int[] var0) {
			foliageBuffer = var0;
		}

		public static int getFoliageColor(double var0, double var2) {
			var2 *= var0;
			int var4 = (int)((1.0D - var0) * 255.0D);
			int var5 = (int)((1.0D - var2) * 255.0D);
			return foliageBuffer[var5 << 8 | var4];
		}

		public static int getFoliageColorPine() {
			return 6396257;
		}

		public static int getFoliageColorBirch() {
			return 8431445;
		}
	}
}

