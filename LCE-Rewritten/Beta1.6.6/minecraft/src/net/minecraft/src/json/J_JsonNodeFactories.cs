namespace net.minecraft.src.json
{
	public static class J_JsonNodeFactories {
		public static J_JsonNode func_27310_a() {
			return J_JsonConstants.field_27228_a;
		}

		public static J_JsonNode func_27313_b() {
			return J_JsonConstants.field_27227_b;
		}

		public static J_JsonNode func_27314_c() {
			return J_JsonConstants.field_27230_c;
		}

		public static J_JsonStringNode func_27316_a(String var0) {
			return new J_JsonStringNode(var0);
		}

		public static J_JsonNode func_27311_b(String var0) {
			return new J_JsonNumberNode(var0);
		}

		public static J_JsonRootNode func_27309_a(IEnumerable<J_JsonNode> var0) {
			return new J_JsonArray(var0);
		}

		public static J_JsonRootNode func_27315_a(J_JsonNode[] j_JsonNodes, params J_JsonNode[] var0) {
			return func_27309_a(var0);
		}

		public static J_JsonRootNode func_27312_a(Dictionary<string, J_JsonObjectNodeBuilder> var0) {
			return new J_JsonObject(var0);
		}

        
    }
}


