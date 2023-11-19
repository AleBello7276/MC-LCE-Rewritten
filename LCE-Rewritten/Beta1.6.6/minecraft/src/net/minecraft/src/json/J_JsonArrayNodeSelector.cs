namespace net.minecraft.src.json
{
	internal sealed class J_JsonArrayNodeSelector : J_LeafFunctor {
		public bool func_27074_a(J_JsonNode var1) {
			return EnumJsonNodeType.ARRAY == var1.func_27218_a();
		}

		public  override String func_27060_a() {
			return "A short form array";
		}

		public List<J_JsonNode> func_27075_b(J_JsonNode var1) {
			return (List<J_JsonNode>)var1.func_27215_d();
		}

		public String toString() {
			return "an array";
		}

		public override Object func_27063_c(Object var1) {
			return this.func_27075_b((J_JsonNode)var1);
		}

		public override bool func_27058_a(Object var1) {
			return this.func_27074_a((J_JsonNode)var1);
		}
	}

}

