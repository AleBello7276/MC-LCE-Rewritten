namespace net.minecraft.src
{
	public class EnumOSMappingHelper 
	{
		public static readonly int[] enumOSMappingArray = new int[EnumOS2.GetValues(typeof(EnumOS2)).Length];

		static EnumOSMappingHelper()
		{
			try {
				enumOSMappingArray[EnumOS2.linux.GetHashCode()] = 1;
			} catch (IndexOutOfRangeException ex) {
			}

			try {
				enumOSMappingArray[EnumOS2.solaris.GetHashCode()] = 2;
			} catch (IndexOutOfRangeException ex) {
			}

			try {
				enumOSMappingArray[EnumOS2.windows.GetHashCode()] = 3;
			} catch (IndexOutOfRangeException ex) {
			}

			try {
				enumOSMappingArray[EnumOS2.macos.GetHashCode()] = 4;
			} catch (IndexOutOfRangeException ex) {
			}

		}
	}
}


