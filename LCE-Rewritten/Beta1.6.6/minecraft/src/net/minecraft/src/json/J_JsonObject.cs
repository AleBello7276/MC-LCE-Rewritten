namespace net.minecraft.src.json
{
	 internal sealed class J_JsonObject : J_JsonRootNode {
		  private readonly Dictionary<string, J_JsonObjectNodeBuilder> field_27222_a;

		public J_JsonObject(Dictionary<string, J_JsonObjectNodeBuilder> var1) {
			this.field_27222_a = new Dictionary<string, J_JsonObjectNodeBuilder>(field_27222_a);
		}

		public override Dictionary<string, J_JsonNode> func_27214_c() {
			return new Dictionary<string, J_JsonNode>((IDictionary<string, J_JsonNode>)this.field_27222_a);
		}

		public override EnumJsonNodeType func_27218_a() {
			return EnumJsonNodeType.OBJECT;
		}

		public override String func_27216_b() {
			throw new InvalidOperationException("Attempt to get text on a JsonNode without text.");
		}

		public override List<object> func_27215_d() {
			throw new InvalidOperationException("Attempt to get elements on a JsonNode without elements.");
		}

		public bool equals(Object var1) {
			if(this == var1) {
				return true;
			} else if(var1 != null && this.GetType() == var1.GetType()) {
				J_JsonObject var2 = (J_JsonObject)var1;
				return this.field_27222_a.Equals(var2.field_27222_a);
			} else {
				return false;
			}
		}

		public int hashCode() {
			return this.field_27222_a.GetHashCode();
		}

		public String toString() {
			return "JsonObject fields:[" + this.field_27222_a + "]";
		}
	}

}


