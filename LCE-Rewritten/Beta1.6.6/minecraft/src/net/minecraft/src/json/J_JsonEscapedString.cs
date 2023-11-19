namespace net.minecraft.src.json
{
	sealed class J_JsonEscapedString {
		private readonly String field_27031_a;

		public J_JsonEscapedString(String var1) {
			this.field_27031_a = var1.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\b", "\\b").Replace("\f", "\\f").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
		}

		public String toString() {
			return this.field_27031_a;
		}
	}
}

