namespace net.minecraft.src.json
{
	public sealed class J_JsonStringNodeBuilder : J_JsonNodeBuilder {
		private readonly String field_27244_a;

		public J_JsonStringNodeBuilder(String var1) {
			this.field_27244_a = var1;
		}

		public J_JsonStringNode func_27243_a() {
			return J_JsonNodeFactories.func_27316_a(this.field_27244_a);
		}

		public J_JsonNode func_27234_b() {
			return this.func_27243_a();
		}
	}
}

