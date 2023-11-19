using System.IO;

namespace net.minecraft.src.nbt
{
	public class NBTTagFloat : NBTBase {
		public float floatValue;

		public NBTTagFloat() {
		}

		public NBTTagFloat(float var1) {
			this.floatValue = var1;
		}

		public override void writeTagContents(BinaryWriter var1) {
			var1.Write(this.floatValue);
		}

		public override void readTagContents(BinaryReader var1) {
			this.floatValue = var1.ReadSingle();
		}

		public override byte getType() {
			return (byte)5;
		}

		public String toString() {
			return "" + this.floatValue;
		}
	}

}

