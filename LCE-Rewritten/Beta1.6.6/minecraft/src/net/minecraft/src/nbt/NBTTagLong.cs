using System.IO;
namespace net.minecraft.src.nbt
{
	public class NBTTagLong : NBTBase {
		public long longValue;

		public NBTTagLong() {
		}

		public NBTTagLong(long var1) {
			this.longValue = var1;
		}

		public override void writeTagContents(BinaryWriter var1){
			var1.Write(this.longValue);
		}

		public override void readTagContents(BinaryReader var1) {
			this.longValue = var1.ReadInt64();
		}

		public override byte getType() {
			return (byte)4;
		}

		public String toString() {
			return "" + this.longValue;
		}
	}

}


