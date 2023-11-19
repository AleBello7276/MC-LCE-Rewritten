namespace net.minecraft.src.world.chunk
{
	public class ChunkCoordinates : IComparable {
		public int x;
		public int y;
		public int z;

		public ChunkCoordinates() {
		}

		public ChunkCoordinates(int var1, int var2, int var3) {
			this.x = var1;
			this.y = var2;
			this.z = var3;
		}

		public ChunkCoordinates(ChunkCoordinates var1) {
			this.x = var1.x;
			this.y = var1.y;
			this.z = var1.z;
		}

		public override bool Equals(object? var1) {
			if(!(var1 is ChunkCoordinates)) {
				return false;
			} else {
				ChunkCoordinates var2 = (ChunkCoordinates)var1;
				return this.x == var2.x && this.y == var2.y && this.z == var2.z;
			}
		}

		public override int GetHashCode() {
			return this.x + this.z << 8 + this.y << 16;
		}

		public int compareChunkCoordinate(ChunkCoordinates var1) {
			return this.y == var1.y ? (this.z == var1.z ? this.x - var1.x : this.z - var1.z) : this.y - var1.y;
		}

		public double getSqDistanceTo(int var1, int var2, int var3) {
			int var4 = this.x - var1;
			int var5 = this.y - var2;
			int var6 = this.z - var3;
			return Math.Sqrt((double)(var4 * var4 + var5 * var5 + var6 * var6));
		}

		public int CompareTo(object? var1) {
			return this.compareChunkCoordinate((ChunkCoordinates)var1);
		}
	}

}
