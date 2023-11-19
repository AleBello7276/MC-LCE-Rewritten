namespace net.minecraft.src.world.chunk
{
	public class ChunkPosition {
		public readonly int x;
		public readonly int y;
		public readonly int z;

		public ChunkPosition(int var1, int var2, int var3) {
			this.x = var1;
			this.y = var2;
			this.z = var3;
		}

		public bool equals(Object var1) {
			if(!(var1 is ChunkPosition)) {
				return false;
			} else {
				ChunkPosition var2 = (ChunkPosition)var1;
				return var2.x == this.x && var2.y == this.y && var2.z == this.z;
			}
		}

		public int hashCode() {
			return this.x * 8976890 + this.y * 981131 + this.z;
		}
	}

}
