namespace net.minecraft.src.world.chunk
{
	public class ChunkCoordIntPair {
		public readonly int chunkXPos;
		public readonly int chunkZPos;

		public ChunkCoordIntPair(int var1, int var2) {
			this.chunkXPos = var1;
			this.chunkZPos = var2;
		}

		public static int chunkXZ2Int(int var0, int var1) {
			return (var0 < 0 ? int.MinValue : 0) | (var0 & short.MaxValue) << 16 | (var1 < 0 ? -short.MinValue : 0) | var1 & short.MaxValue;
		}

		public int hashCode() {
			return chunkXZ2Int(this.chunkXPos, this.chunkZPos);
		}

		public bool equals(Object var1) {
			ChunkCoordIntPair var2 = (ChunkCoordIntPair)var1;
			return var2.chunkXPos == this.chunkXPos && var2.chunkZPos == this.chunkZPos;
		}
	}

}
