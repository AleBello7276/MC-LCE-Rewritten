using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace net.minecraft.src.nbt
{
	public class NBTTagCompound : NBTBase {
		
		private Dictionary<string, NBTBase> tagMap = new Dictionary<string, NBTBase>();

		public override void writeTagContents(BinaryWriter var1)  {
			foreach (NBTBase tag in tagMap.Values)
            {
                NBTBase.writeTag(tag, var1);
            }

			var1.Write((byte)0);
		}

		public override void readTagContents(BinaryReader var1)  {
			this.tagMap.Clear();

			while(true) {
				NBTBase var2 = NBTBase.readTag(var1);
				if(var2.getType() == 0) {
					return;
				}

				tagMap[var2.getKey()] = var2;
			}
		}

		public ICollection func_28110_c() {
			return this.tagMap.Values;
		}

		public override byte getType() {
			return (byte)10;
		}

		public void setTag(String var1, NBTBase var2) {
			tagMap[var1] = var2.setKey(var1);
		}

		public void setByte(String var1, byte var2) {
			tagMap[var1] = new NBTTagByte(var2).setKey(var1);
		}

		public void setShort(String var1, short var2) {
			tagMap[var1] = new NBTTagShort(var2).setKey(var1);
		}

		public void setInteger(String var1, int var2) {
			tagMap[var1] = new NBTTagInt(var2).setKey(var1);
		}

		public void setLong(String var1, long var2) {
			tagMap[var1] = new NBTTagLong(var2).setKey(var1);
		}

		public void setFloat(String var1, float var2) {
			tagMap[var1] = new NBTTagFloat(var2).setKey(var1);
		}

		public void setDouble(String var1, double var2) {
			tagMap[var1] = new NBTTagDouble(var2).setKey(var1);
		}

		public void setString(String var1, String var2) {
			tagMap[var1] = new NBTTagString(var2).setKey(var1);
		}

		public void setByteArray(String var1, byte[] var2) {
			tagMap[var1] = new NBTTagByteArray(var2).setKey(var1);
		}

		public void setCompoundTag(String var1, NBTTagCompound var2) {
			tagMap[var1] = var2.setKey(var1);
		}

		public void setBoolean(String var1, bool var2) {
			this.setByte(var1, (byte)(var2 ? 1 : 0));
		}

		public bool hasKey(String var1) {
			return this.tagMap.ContainsKey(var1);
		}

		public byte getByte(String var1) {
			return !this.tagMap.ContainsKey(var1) ? (byte)0 : ((NBTTagByte)tagMap[var1]).byteValue;
		}

		public short getShort(String var1) {
			return !this.tagMap.ContainsKey(var1) ? (short)0 : ((NBTTagShort)tagMap[var1]).shortValue;
		}

		public int getInteger(String var1) {
			return !this.tagMap.ContainsKey(var1) ?  0 : ((NBTTagInt)tagMap[var1]).intValue;
		}

		public long getLong(String var1) {
			return !this.tagMap.ContainsKey(var1) ? 0L : ((NBTTagLong)tagMap[var1]).longValue;
		}

		public float getFloat(String var1) {
			return !this.tagMap.ContainsKey(var1) ? 0.0F : ((NBTTagFloat)tagMap[var1]).floatValue;
		}

		public double getDouble(String var1) {
			return !this.tagMap.ContainsKey(var1) ? 0.0D : ((NBTTagDouble)tagMap[var1]).doubleValue;
		}

		public String getString(String var1) {
			return !this.tagMap.ContainsKey(var1) ? "" : ((NBTTagString)tagMap[var1]).stringValue;
		}

		public byte[] getByteArray(String var1) {
			return !this.tagMap.ContainsKey(var1) ? new byte[0] : ((NBTTagByteArray)tagMap[var1]).byteArray;
		}

		public NBTTagCompound getCompoundTag(String var1) {
			return !this.tagMap.ContainsKey(var1) ? new NBTTagCompound() : (NBTTagCompound)tagMap[var1];
        }
		

		public NBTTagList getTagList(String var1) {
			return !this.tagMap.ContainsKey(var1) ? new NBTTagList() : (NBTTagList)tagMap[var1];
		}

		public bool getBoolean(String var1) {
			return this.getByte(var1) != 0;
		}

		public String toString() {
			return "" + this.tagMap.Count+ " entries";
		}
	}

}