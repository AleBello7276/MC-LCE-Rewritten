namespace net.minecraft.src.json
{
	class EnumJsonNodeTypeMappingHelper {

		public static readonly int[] field_27341_a = new int[EnumJsonNodeType.GetValues(typeof(EnumJsonNodeType)).Length];


		static EnumJsonNodeTypeMappingHelper(){
			try {
				field_27341_a[EnumJsonNodeType.ARRAY.GetHashCode()] = 1;
			} catch (IndexOutOfRangeException var7) {
			}

			try {
				field_27341_a[EnumJsonNodeType.OBJECT.GetHashCode()] = 2;
			} catch (IndexOutOfRangeException var6) {
			}

			try {
				field_27341_a[EnumJsonNodeType.STRING.GetHashCode()] = 3;
			} catch (IndexOutOfRangeException var5) {
			}

			try {
				field_27341_a[EnumJsonNodeType.NUMBER.GetHashCode()] = 4;
			} catch (IndexOutOfRangeException var4) {
			}

			try {
				field_27341_a[EnumJsonNodeType.FALSE.GetHashCode()] = 5;
			} catch (IndexOutOfRangeException var3) {
			}

			try {
				field_27341_a[EnumJsonNodeType.TRUE.GetHashCode()] = 6;
			} catch (IndexOutOfRangeException var2) {
			}

			try {
				field_27341_a[EnumJsonNodeType.NULL.GetHashCode()] = 7;
			} catch (IndexOutOfRangeException var1) {
			}

		}
	}

}
