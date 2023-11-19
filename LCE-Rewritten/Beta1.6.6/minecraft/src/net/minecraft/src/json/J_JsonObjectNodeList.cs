namespace net.minecraft.src.json
{
	public class J_JsonObjectNodeList : Dictionary<J_JsonStringNode, J_JsonNode> {
		readonly J_JsonObjectNodeBuilder field_27308_a;

		public J_JsonObjectNodeList(J_JsonObjectNodeBuilder var1) {
			this.field_27308_a = var1;
			
			var iterator = J_JsonObjectNodeBuilder.func_27236_a(field_27308_a).GetEnumerator();

			while(iterator.MoveNext()) {
				J_JsonFieldBuilder var3 = (J_JsonFieldBuilder)iterator.Current;
				this.Add(var3.func_27303_b(), var3.func_27302_c());
			}

		}
	}

}



