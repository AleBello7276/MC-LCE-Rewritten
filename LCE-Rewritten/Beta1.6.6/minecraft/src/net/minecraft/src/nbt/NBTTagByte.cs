using System.IO;

namespace net.minecraft.src.nbt
{
	public class NBTTagByte : NBTBase {
		public byte byteValue;

		public NBTTagByte() {
		}

		public NBTTagByte(byte var1) {
			this.byteValue = var1;
		}

		public override void writeTagContents(BinaryWriter var1) {
			var1.Write(this.byteValue);
		}

		public override void readTagContents(BinaryReader var1)  {
			this.byteValue = var1.ReadByte();
		}

		public override byte getType() {
			return (byte)1;
		}

		public String toString() {
			return "" + this.byteValue;
		}
	}

}


