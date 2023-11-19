using net.minecraft.src.nbt;

namespace net.minecraft.src
{
	public abstract class MapDataBase {
		public readonly String field_28168_a;
		private bool dirty;

		public MapDataBase(String var1) {
			this.field_28168_a = var1;
		}

		public abstract void readFromNBT(NBTTagCompound var1);

		public abstract void writeToNBT(NBTTagCompound var1);

		public void markDirty() {
			this.setDirty(true);
		}

		public void setDirty(bool var1) {
			this.dirty = var1;
		}

		public bool isDirty() {
			return this.dirty;
		}
	}

}



