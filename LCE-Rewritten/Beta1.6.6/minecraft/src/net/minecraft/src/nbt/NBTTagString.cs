using System.IO;
namespace net.minecraft.src.nbt
{
	
	public class NBTTagString : NBTBase {
		public String stringValue;

		public NBTTagString() {
		}

		public NBTTagString(String var1) {
			stringValue = var1;
            if (var1 == null)
            {
                throw new ArgumentException("Empty string not allowed");
            }
		}

		public override void writeTagContents(BinaryWriter var1) {
			var1.Write(this.stringValue);
		}

		public override void readTagContents(BinaryReader var1) {
			this.stringValue = var1.ReadString();
		}

		public override byte getType() {
			return (byte)8;
		}

		public String toString() {
			return "" + this.stringValue;
		}
	}

}