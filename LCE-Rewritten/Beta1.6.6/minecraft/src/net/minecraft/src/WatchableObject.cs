namespace net.minecraft.src
{
	public class WatchableObject {
		private readonly int objectType;
		private readonly int dataValueId;
		private Object watchedObject;
		private bool isWatching;

		public WatchableObject(int var1, int var2, Object var3) {
			this.dataValueId = var2;
			this.watchedObject = var3;
			this.objectType = var1;
			this.isWatching = true;
		}

		public int getDataValueId() {
			return this.dataValueId;
		}

		public void setObject(Object var1) {
			this.watchedObject = var1;
		}

		public Object getObject() {
			return this.watchedObject;
		}

		public int getObjectType() {
			return this.objectType;
		}

		public void setWatching(bool var1) {
			this.isWatching = var1;
		}
	}

}
