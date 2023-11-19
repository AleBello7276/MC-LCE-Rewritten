using System;
using System.Collections.Generic;
using System.IO;

namespace net.minecraft.src.nbt
{
	public class NBTTagList : NBTBase {
		private List<NBTBase> tagList = new List<NBTBase>();
		private byte tagType;

		public override void writeTagContents(BinaryWriter var1)  {
			if(this.tagList.Count > 0) {
				this.tagType = ((NBTBase)this.tagList[0]).getType();
			} else {
				this.tagType = 1;
			}

			var1.Write(this.tagType);
			var1.Write(this.tagList.Count);

			for(int var2 = 0; var2 < this.tagList.Count; ++var2) {
				((NBTBase)this.tagList[var2]).writeTagContents(var1);
			}

		}

		public override void readTagContents(BinaryReader var1) {
			this.tagType = var1.ReadByte();
			int var2 = var1.ReadInt32();
			tagList = new List<NBTBase>();

			for(int var3 = 0; var3 < var2; ++var3) {
				NBTBase var4 = NBTBase.createTagOfType(this.tagType);
				var4.readTagContents(var1);
				this.tagList.Add(var4);
			}

		}

		public override byte getType() {
			return (byte)9;
		}

		public String toString() {
			return "" + this.tagList.Count + " entries of type " + NBTBase.getTagName(this.tagType);
		}

		public void setTag(NBTBase var1) {
			this.tagType = var1.getType();
			this.tagList.Add(var1);
		}

		public NBTBase tagAt(int var1) {
			return (NBTBase)this.tagList[var1];
		}

		public int tagCount() {
			return this.tagList.Count;
		}
	}

}



