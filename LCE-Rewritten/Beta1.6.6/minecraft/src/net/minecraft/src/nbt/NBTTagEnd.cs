using System.IO;

namespace net.minecraft.src.nbt
{
	public class NBTTagEnd : NBTBase {
		public override void readTagContents(BinaryReader var1)  {
		}

		public override void writeTagContents(BinaryWriter var1) {
		}

		public override byte getType() {
			return (byte)0;
		}

		public String toString() {
			return "END";
		}
	}

}

