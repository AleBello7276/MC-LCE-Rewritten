using net.minecraft.src.blocks;

namespace net.minecraft.src.world.chunk
{
	public class ChunkBlockMap {
		private static byte[] field_26003_a = new byte[256];

		public static void func_26002_a(byte[] var0) {
			for(int var1 = 0; var1 < var0.Length; ++var1) {
				var0[var1] = field_26003_a[var0[var1] & 255];
			}

		}

		static ChunkBlockMap()
		{
			try {
				for(int var0 = 0; var0 < 256; ++var0) {
					byte var1 = (byte)var0;
					if(var1 != 0 && Block.blocksList[var1 & 255] == null) {
						var1 = 0;
					}

					field_26003_a[var0] = var1;
				}
			} catch (Exception var2) {
				Console.WriteLine(var2.ToString());
			}

		}
	}

}

