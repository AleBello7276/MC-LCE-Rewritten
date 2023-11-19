using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src.json
{	
	public sealed class J_CompactJsonFormatter : J_JsonFormatter {
		public String func_27327_a(J_JsonRootNode var1) {
			StringBuilder var2 = new StringBuilder();

			try {
				this.func_27329_a(var1, var2);
			} catch (IOException var4) {
				throw new ApplicationException("Coding failure in Argo:  StringWriter gave an IOException", var4);
			}

			return var2.ToString();
		}

		public void func_27329_a(J_JsonRootNode var1, StringBuilder var2) {
			this.func_27328_a(var1, var2);
		}

		private void func_27328_a(J_JsonNode var1, StringBuilder var2)
        {
            bool var3 = true;
            IEnumerator<J_JsonNode> var4;
            switch (EnumJsonNodeTypeMappingHelper.field_27341_a[(int)var1.func_27218_a()])
            {
                case 1:
                    var2.Append('[');
                    var4 = (IEnumerator<J_JsonNode>)var1.func_27215_d().GetEnumerator();

                    while (var4.MoveNext())
                    {
                        J_JsonNode var6 = var4.Current;
                        if (!var3)
                        {
                            var2.Append(',');
                        }

                        var3 = false;
                        func_27328_a(var6, var2);
                    }

                    var2.Append(']');
                    break;
                case 2:
                    var2.Append('{');
                    var4 = new SortedSet<J_JsonNode>((IComparer<J_JsonNode>?)var1.func_27214_c()).GetEnumerator();

                    while (var4.MoveNext())
                    {
                        J_JsonStringNode var5 = (J_JsonStringNode)var4.Current;
                        if (!var3)
                        {
                            var2.Append(',');
                        }

                        var3 = false;
                        func_27328_a(var5, var2);
                        var2.Append(':');
                        func_27328_a(var1, var2);
                    }

                    var2.Append('}');
                    break;
                case 3:
                   var2.Append('"').Append(new J_JsonEscapedString(var1.func_27216_b()).ToString()).Append('"');
                    break;
                case 4:
                    var2.Append(var1.func_27216_b());
                    break;
                case 5:
                    var2.Append("false");
                    break;
                case 6:
                    var2.Append("true");
                    break;
                case 7:
                    var2.Append("null");
                    break;
                default:
                    throw new ApplicationException("Coding failure in Argo:  Attempt to format a JsonNode of unknown type [" + var1.func_27218_a() + "];");
            }
        }
	}

}