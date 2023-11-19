namespace net.minecraft.src.json
{
	internal class J_JsonObjectNodeSelector : J_LeafFunctor {
		public bool func_27070_a(J_JsonNode var1) {
			return EnumJsonNodeType.OBJECT == var1.func_27218_a();
		}

		public override String func_27060_a() {
			return "A short form object";
		}

		public IDictionary<string, J_JsonNode> func_27071_b(J_JsonNode var1) {
			return var1.func_27214_c();
		}

		public String toString() {
			return "an object";
		}

		public override Object func_27063_c(Object var1) {
			return this.func_27071_b((J_JsonNode)var1);
		}

		public override bool func_27058_a(Object var1) {
			return this.func_27070_a((J_JsonNode)var1);
		}
	}

}


