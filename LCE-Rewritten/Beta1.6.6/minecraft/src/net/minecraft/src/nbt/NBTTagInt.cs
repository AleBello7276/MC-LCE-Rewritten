using System.IO;

namespace net.minecraft.src.nbt
{
	public class NBTTagInt : NBTBase {
		public int intValue;

		public NBTTagInt() {
		}

		public NBTTagInt(int var1) {
			this.intValue = var1;
		}

		public override void writeTagContents(BinaryWriter var1) {
			var1.Write(this.intValue);
		}

		public override void readTagContents(BinaryReader var1)  {
			this.intValue = var1.ReadInt32();
		}

		public override byte getType() {
			return (byte)3;
		}

		public String toString() {
			return "" + this.intValue;
		}
	}

}

