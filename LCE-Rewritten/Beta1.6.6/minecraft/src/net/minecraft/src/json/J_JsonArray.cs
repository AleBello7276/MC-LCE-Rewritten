using System;
using System.Collections;
using System.Collections.Generic;

namespace net.minecraft.src.json
{
	sealed class J_JsonArray : J_JsonRootNode {
		private readonly List<J_JsonNode> field_27221_a;


		public J_JsonArray(IEnumerable<J_JsonNode> var1) {
			this.field_27221_a = func_27220_a(var1);
		}

		public override EnumJsonNodeType func_27218_a() {
			return EnumJsonNodeType.ARRAY;
		}

		public override List<J_JsonNode> func_27215_d() {
			return new List<J_JsonNode>(this.field_27221_a);
		}

		public override String func_27216_b() {
			throw new InvalidOperationException("Attempt to get text on a JsonNode without text.");
		}

		public override Dictionary<string, J_JsonNode> func_27214_c() {
			throw new InvalidOperationException("Attempt to get fields on a JsonNode without fields.");
		}

		public bool equals(Object var1) {
			if(this == var1) {
				return true;
			} else if(var1 != null && this.GetType() == var1.GetType()) {
				J_JsonArray var2 = (J_JsonArray)var1;
				return this.field_27221_a.Equals(var2.field_27221_a);
			} else {
				return false;
			}
		}

		public int hashCode() {
			return this.field_27221_a.GetHashCode();
		}

		public String toString() {
			return "JsonArray elements:[" + this.field_27221_a + "]";
		}

		private static List<J_JsonNode> func_27220_a(IEnumerable<J_JsonNode> var0) {
			return new J_JsonNodeList(var0);
		}
	}

}
