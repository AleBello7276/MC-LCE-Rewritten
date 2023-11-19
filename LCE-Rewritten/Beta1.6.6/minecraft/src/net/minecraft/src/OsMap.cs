namespace net.minecraft.src
{
	class OsMap {
		public static readonly int[] field_1193_a = new int[EnumOS1.GetValues(typeof(EnumOS1)).Length];

		static OsMap(){
			try {
				field_1193_a[EnumOS1.linux.GetHashCode()] = 1;
			} catch (IndexOutOfRangeException var4) {
			}

			try {
				field_1193_a[EnumOS1.solaris.GetHashCode()] = 2;
			} catch (IndexOutOfRangeException var3) {
			}

			try {
				field_1193_a[EnumOS1.windows.GetHashCode()] = 3;
			} catch (IndexOutOfRangeException var2) {
			}

			try {
				field_1193_a[EnumOS1.macos.GetHashCode()] = 4;
			} catch (IndexOutOfRangeException var1) {
			}

		}
	}
}


