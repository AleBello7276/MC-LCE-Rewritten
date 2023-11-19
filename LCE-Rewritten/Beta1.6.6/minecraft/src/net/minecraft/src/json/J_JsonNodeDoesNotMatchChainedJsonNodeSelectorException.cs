using System.Text;

namespace net.minecraft.src.json
{
	public sealed class J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException : J_JsonNodeDoesNotMatchJsonNodeSelectorException {
		public readonly J_Functor field_27326_a;
		private readonly List<J_JsonNodeSelector> field_27325_b;

		public static J_JsonNodeDoesNotMatchJsonNodeSelectorException func_27322_a(J_Functor var0) {
			return new J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException(var0, new List<J_JsonNodeSelector>());
		}

		public static J_JsonNodeDoesNotMatchJsonNodeSelectorException func_27323_a(J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException var0, J_JsonNodeSelector var1) {
			List<J_JsonNodeSelector> var2 = new List<J_JsonNodeSelector>(var0.field_27325_b);
			var2.Add(var1);
			return new J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException(var0.field_27326_a, var2);
		}

		public static J_JsonNodeDoesNotMatchJsonNodeSelectorException func_27321_b(J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException var0, J_JsonNodeSelector var1) {
			List<J_JsonNodeSelector> var2 = new List<J_JsonNodeSelector>(var0.field_27325_b);
			var2.Add(var1);
            return new J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException(var0.field_27326_a, var2);
		}

		private J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException(J_Functor var1, List<J_JsonNodeSelector> var2) :base("Failed to match any JSON node at [" + func_27324_a(var2) + "]")
		{
			this.field_27326_a = var1;
			this.field_27325_b = var2;
		}

		private static String func_27324_a(List<J_JsonNodeSelector> var0) {
			StringBuilder var1 = new StringBuilder();

			for(int var2 = var0.Count - 1; var2 >= 0; --var2) {
				var1.Append((J_JsonNodeSelector)var0[var2].func_27358_a());
				if(var2 != 0) {
					var1.Append(".");
				}
			}

			return var1.ToString();
		}

		public String toString() {
			return "JsonNodeDoesNotMatchJsonNodeSelectorException{failedNode=" + this.field_27326_a + ", failPath=" + this.field_27325_b + '}';
		}
	}

}


