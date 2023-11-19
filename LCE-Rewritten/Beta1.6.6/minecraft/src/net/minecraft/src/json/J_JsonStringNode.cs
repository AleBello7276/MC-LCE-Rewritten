
namespace net.minecraft.src.json
{
	public sealed class J_JsonStringNode : J_JsonNode {
		private readonly String field_27224_a;

		public J_JsonStringNode(String var1) {
			if(var1 == null) {
				throw new ArgumentNullException("Attempt to construct a JsonString with a null value.");
			} else {
				this.field_27224_a = var1;
			}
		}

		public override EnumJsonNodeType func_27218_a() {
			return EnumJsonNodeType.STRING;
		}

		public override String func_27216_b() {
			return this.field_27224_a;
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
				J_JsonStringNode var2 = (J_JsonStringNode)var1;
				return this.field_27224_a.Equals(var2.field_27224_a);
			} else {
				return false;
			}
		}

		public int hashCode() {
			return this.field_27224_a.GetHashCode();
		}

		public String toString() {
			return "JsonStringNode value:[" + this.field_27224_a + "]";
		}

		public int func_27223_a(J_JsonStringNode var1) {
			return this.field_27224_a.CompareTo(var1.field_27224_a);
		}

		public int compareTo(Object var1) {
			return this.func_27223_a((J_JsonStringNode)var1);
		}

       
    }

}

