namespace net.minecraft.src.json
{
	sealed class J_JsonConstants : J_JsonNode {
		public static readonly J_JsonConstants field_27228_a = new J_JsonConstants(EnumJsonNodeType.NULL);
		public static readonly J_JsonConstants field_27227_b = new J_JsonConstants(EnumJsonNodeType.TRUE);
		public static readonly J_JsonConstants field_27230_c = new J_JsonConstants(EnumJsonNodeType.FALSE);
		private readonly EnumJsonNodeType field_27229_d;

		private J_JsonConstants(EnumJsonNodeType var1) {
			this.field_27229_d = var1;
		}

		public override EnumJsonNodeType func_27218_a() {
			return this.field_27229_d;
		}

		public override String func_27216_b() {
			throw new InvalidOperationException("Attempt to get text on a JsonNode without text.");
		}

		public override Dictionary<string, J_JsonNode> func_27214_c() {
			throw new InvalidOperationException("Attempt to get fields on a JsonNode without fields.");
		}

		public override List<J_JsonNode> func_27215_d() {
			throw new InvalidOperationException("Attempt to get elements on a JsonNode without elements.");
		}
	}

}

