namespace net.minecraft.src.json
{
	sealed class J_JsonNumberNodeBuilder : J_JsonNodeBuilder {
		private readonly J_JsonNode field_27239_a;

		public J_JsonNumberNodeBuilder(String var1) {
			this.field_27239_a = J_JsonNodeFactories.func_27311_b(var1);
		}

		public J_JsonNode func_27234_b() {
			return this.field_27239_a;
		}
	}
}

