using System.Collections.Generic;

namespace net.minecraft.src.json
{
	sealed class J_JsonElementNodeSelector : J_LeafFunctor {
		readonly int field_27069_a;

		public J_JsonElementNodeSelector(int var1) {
			this.field_27069_a = var1;
		}

		public bool func_27067_a(List<object> var1) {
			return var1.Count > this.field_27069_a;
		}

		public override String func_27060_a() {
			return field_27069_a.ToString();
		}

		public J_JsonNode func_27068_b(List<object> var1) {
			return (J_JsonNode)var1[field_27069_a];
		}

		public String toString() {
			return "an element at index [" + this.field_27069_a + "]";
		}

		public override Object func_27063_c(Object var1) {
			return this.func_27068_b((List<object>)var1);
		}

		public override bool func_27058_a(Object var1) {
			return this.func_27067_a((List<object>)var1);
		}
	}
}

