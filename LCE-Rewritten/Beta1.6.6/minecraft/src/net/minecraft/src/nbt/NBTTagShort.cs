using System.IO;
namespace net.minecraft.src.nbt
{
	public class NBTTagShort : NBTBase {
		public short shortValue;

		public NBTTagShort() {
		}

		public NBTTagShort(short var1) {
			this.shortValue = var1;
		}

		public override void writeTagContents(BinaryWriter var1)  {
			var1.Write(this.shortValue);
		}

		public override void readTagContents(BinaryReader var1) {
			this.shortValue = var1.ReadInt16();
		}

		public override byte getType() {
			return (byte)2;
		}

		public String toString() {
			return "" + this.shortValue;
		}
	}
}



