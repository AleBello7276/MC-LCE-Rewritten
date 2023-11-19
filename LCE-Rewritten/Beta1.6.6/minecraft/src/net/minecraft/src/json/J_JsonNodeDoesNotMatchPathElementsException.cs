using System.Text;
namespace net.minecraft.src.json
{
	public sealed class J_JsonNodeDoesNotMatchPathElementsException : J_JsonNodeDoesNotMatchJsonNodeSelectorException {
		private static readonly J_JsonFormatter field_27320_a = new J_CompactJsonFormatter();

		public static J_JsonNodeDoesNotMatchPathElementsException func_27319_a(J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException var0, Object[] var1, J_JsonRootNode var2) {
			return new J_JsonNodeDoesNotMatchPathElementsException(var0, var1, var2);
		}

		private J_JsonNodeDoesNotMatchPathElementsException(J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException var1, Object[] var2, J_JsonRootNode var3) :base(func_27318_b(var1, var2, var3))
		{
		}

		private static String func_27318_b(J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException var0, Object[] var1, J_JsonRootNode var2) {
			return "Failed to find " + var0.field_27326_a.ToString() + " at [" + J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException.func_27322_a(var0.field_27326_a) + "] while resolving [" + func_27317_a(var1) + "] in " + field_27320_a.func_27327_a(var2) + ".";
		}

		private static String func_27317_a(Object[] var0) {
			StringBuilder var1 = new StringBuilder();
			bool var2 = true;
			Object[] var3 = var0;
			int var4 = var0.Length;

			for(int var5 = 0; var5 < var4; ++var5) {
				Object var6 = var3[var5];
				if(!var2) {
					var1.Append(".");
				}

				var2 = false;
				if(var6 is String) {
					var1.Append("\"").Append(var6).Append("\"");
				} else {
					var1.Append(var6);
				}
			}

			return var1.ToString();
		}
    }

}