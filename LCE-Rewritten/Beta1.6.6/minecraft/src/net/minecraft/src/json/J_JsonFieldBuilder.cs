namespace net.minecraft.src.json
{	
	public sealed class J_JsonFieldBuilder {
		private J_JsonNodeBuilder field_27306_a;
		private J_JsonNodeBuilder field_27305_b;

		public static J_JsonFieldBuilder func_27301_a() {
			return new J_JsonFieldBuilder();
		}

		public J_JsonFieldBuilder func_27304_a(J_JsonNodeBuilder var1) {
			this.field_27306_a = var1;
			return this;
		}

		public J_JsonFieldBuilder func_27300_b(J_JsonNodeBuilder var1) {
			this.field_27305_b = var1;
			return this;
		}

		public J_JsonStringNode func_27303_b() {
			return (J_JsonStringNode)this.field_27306_a.func_27234_b();
		}

		public J_JsonNode func_27302_c() {
			return this.field_27305_b.func_27234_b();
		}
	}
}
