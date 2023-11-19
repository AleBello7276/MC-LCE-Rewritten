using System.IO;

namespace net.minecraft.src.nbt
{
	public class NBTTagDouble : NBTBase {
		public double doubleValue;

		public NBTTagDouble() {
		}

		public NBTTagDouble(double var1) {
			this.doubleValue = var1;
		}

		public override void writeTagContents(BinaryWriter var1)  {
			var1.Write(this.doubleValue);
		}

		public override void readTagContents(BinaryReader var1)  {
			this.doubleValue = var1.ReadDouble();
		}

		public override byte getType() {
			return (byte)6;
		}

		public String toString() {
			return "" + this.doubleValue;
		}
	}
}


