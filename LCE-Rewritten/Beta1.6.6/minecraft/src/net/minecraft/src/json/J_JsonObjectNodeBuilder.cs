namespace net.minecraft.src.json
{
	public sealed class J_JsonObjectNodeBuilder : J_JsonNodeBuilder {
		private readonly List<J_JsonFieldBuilder> field_27238_a = new List<J_JsonFieldBuilder>();


		public J_JsonObjectNodeBuilder func_27237_a(J_JsonFieldBuilder var1) {
			this.field_27238_a.Add(var1);
			return this;
		}

		public J_JsonRootNode func_27235_a() {
			return J_JsonNodeFactories.func_27312_a(new Dictionary<string, J_JsonObjectNodeBuilder>());
		}

		public J_JsonNode func_27234_b() {
			return this.func_27235_a();
		}

		internal static List<J_JsonFieldBuilder> func_27236_a(J_JsonObjectNodeBuilder var0) {
			return var0.field_27238_a;
		}
	}
}


