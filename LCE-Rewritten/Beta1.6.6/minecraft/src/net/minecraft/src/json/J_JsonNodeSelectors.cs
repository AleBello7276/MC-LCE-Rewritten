using System;
using System.Linq;

namespace net.minecraft.src.json
{
	public sealed class J_JsonNodeSelectors {
		public static J_JsonNodeSelector func_27349_a(params object[] var0) {
			return func_27352_a(var0, new J_JsonNodeSelector(new J_JsonStringNodeSelector()));
		}

		public static J_JsonNodeSelector func_27346_b(params object[] var0) {
			return func_27352_a(var0, new J_JsonNodeSelector(new J_JsonArrayNodeSelector()));
		}

		public static J_JsonNodeSelector func_27353_c(params object[] var0) {
			return func_27352_a(var0, new J_JsonNodeSelector(new J_JsonObjectNodeSelector()));
		}

		public static J_JsonNodeSelector func_27348_a(String var0) {
			return func_27350_a(J_JsonNodeFactories.func_27316_a(var0));
		}

		public static J_JsonNodeSelector func_27350_a(J_JsonStringNode var0) {
			return new J_JsonNodeSelector(new J_JsonFieldNodeSelector(var0));
		}

		public static J_JsonNodeSelector func_27351_b(String var0) {
			return func_27353_c(new Object[0]).func_27355_a(func_27348_a(var0));
		}

		public static J_JsonNodeSelector func_27347_a(int var0) {
			return new J_JsonNodeSelector(new J_JsonElementNodeSelector(var0));
		}

		public static J_JsonNodeSelector func_27354_b(int var0) {
			return func_27346_b(new Object[0]).func_27355_a(func_27347_a(var0));
		}

		private static J_JsonNodeSelector func_27352_a(Object[] var0, J_JsonNodeSelector var1) {
			var var2 = var1;

			for(int var3 = var0.Length - 1; var3 >= 0; --var3) {
				if(var0[var3] is int) {
					var2 = func_27345_a(func_27354_b((int)var0[var3]), var2);
				} else {
					if(!(var0[var3] is String)) {
						throw new ArgumentException("Element [" + var0[var3] + "] of path elements" + " [" + string.Join(", ", var0) + "] was of illegal type [" + var0[var3].GetType().FullName + "]; only Integer and String are valid.");
					}

					var2 = func_27345_a(func_27351_b((String)var0[var3]), var2);
				}
			}

			return var2;
		}

		private static J_JsonNodeSelector func_27345_a(J_JsonNodeSelector var0, J_JsonNodeSelector var1) {
			return new J_JsonNodeSelector(new J_ChainedFunctor(var0, var1));
		}
	}

}


