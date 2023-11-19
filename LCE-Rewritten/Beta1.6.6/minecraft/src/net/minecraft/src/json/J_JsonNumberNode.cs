using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace net.minecraft.src.json
{
	internal sealed class J_JsonNumberNode : J_JsonNode {
		
		private static readonly Regex field_27226_a = new Regex("(-?)(0|([1-9]([0-9]*)))(\\.[0-9]+)?((e|E)(\\+|-)?[0-9]+)?");
		private readonly String field_27225_b;

		public J_JsonNumberNode(String var1) {
			if(var1 == null) {
				throw new ArgumentNullException("Attempt to construct a JsonNumber with a null value.");
			} else if(!field_27226_a.IsMatch(var1)) {
				throw new ArgumentException("Attempt to construct a JsonNumber with a String [" + var1 + "] that does not match the JSON number specification.");
			} else {
				this.field_27225_b = var1;
			}
		}

		public override EnumJsonNodeType func_27218_a() {
			return EnumJsonNodeType.NUMBER;
		}

		public override String func_27216_b() {
			return this.field_27225_b;
		}

		public override Dictionary<string, J_JsonNode> func_27214_c() {
			throw new InvalidOperationException("Attempt to get fields on a JsonNode without fields.");
		}

		public override List<J_JsonNode> func_27215_d() {
			throw new InvalidOperationException("Attempt to get elements on a JsonNode without elements.");
		}

		public bool equals(Object var1) {
			if(this == var1) {
				return true;
			} else if(var1 != null && this.GetType() == var1.GetType()) {
				J_JsonNumberNode var2 = (J_JsonNumberNode)var1;
				return this.field_27225_b.Equals(var2.field_27225_b);
			} else {
				return false;
			}
		}

		public int hashCode() {
			return this.field_27225_b.GetHashCode();
		}

		public String toString() {
			return "JsonNumberNode value:[" + this.field_27225_b + "]";
		}
	}

}

