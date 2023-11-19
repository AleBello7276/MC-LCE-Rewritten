using System.Collections;

namespace net.minecraft.src.json
{
	public abstract class J_JsonNode {
		public abstract EnumJsonNodeType func_27218_a();

		public abstract String func_27216_b();

		public abstract Dictionary<string, J_JsonNode> func_27214_c();

		public abstract IList func_27215_d();

		public String func_27213_a(params object[] var1) {
			return (String)this.func_27219_a(J_JsonNodeSelectors.func_27349_a(var1), this, var1);
		}

		public IList func_27217_b(params object[] var1) {
			return (IList)this.func_27219_a(J_JsonNodeSelectors.func_27346_b(var1), this, var1);
		}

		private Object func_27219_a(J_JsonNodeSelector var1, J_JsonNode var2, Object[] var3) {
			try {
				return var1.func_27357_b(var2);
			} catch (J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException var5) {
                throw J_JsonNodeDoesNotMatchPathElementsException.func_27319_a(var5, var3, J_JsonNodeFactories.func_27315_a(new J_JsonNode[] { var2 }));
			}
		}
	}
}

