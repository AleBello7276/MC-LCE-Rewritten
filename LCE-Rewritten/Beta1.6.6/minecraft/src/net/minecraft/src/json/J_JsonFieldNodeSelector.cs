using System.Collections.Generic;

namespace net.minecraft.src.json
{
	sealed class J_JsonFieldNodeSelector : J_LeafFunctor {
		private readonly J_JsonStringNode field_27066_a;

		public J_JsonFieldNodeSelector(J_JsonStringNode var1) {
			this.field_27066_a = var1;
		}

		public bool func_27065_a(Dictionary<string, J_JsonNode> var1) {
			return var1.ContainsKey(this.field_27066_a.ToString());
		}

		public override String func_27060_a() {
			return "\"" + this.field_27066_a.func_27216_b() + "\"";
		}

		public J_JsonNode func_27064_b(Dictionary<string, J_JsonNode> var1) {
			return var1.TryGetValue(field_27066_a.ToString(), out var result) ? result : null;
		}

		public String toString() {
			return "a field called [\"" + this.field_27066_a.func_27216_b() + "\"]";
		}

		public override Object func_27063_c(Object var1) {
			return this.func_27064_b((Dictionary<string, J_JsonNode>)var1);
		}

		public override bool func_27058_a(Object var1) {
			return this.func_27065_a((Dictionary<string, J_JsonNode>)var1);
		}
	}

}


