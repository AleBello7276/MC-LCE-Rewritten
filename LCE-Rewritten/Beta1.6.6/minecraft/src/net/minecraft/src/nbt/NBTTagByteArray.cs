
using System.IO;
namespace net.minecraft.src.nbt
{
	public class NBTTagByteArray : NBTBase {
		public byte[] byteArray;

		public NBTTagByteArray() {
		}

		public NBTTagByteArray(byte[] var1) {
			this.byteArray = var1;
		}

		public override void writeTagContents(BinaryWriter var1)  {
			var1.Write(this.byteArray.Length);
			var1.Write(this.byteArray);
		}

		public override void readTagContents(BinaryReader var1)  {
			int var2 = var1.ReadInt32();
			this.byteArray = new byte[var2];
			var1.Read(this.byteArray);
		}

		public override byte getType() {
			return (byte)7;
		}

		public String toString() {
			return "[" + this.byteArray.Length + " bytes]";
		}
	}

}

