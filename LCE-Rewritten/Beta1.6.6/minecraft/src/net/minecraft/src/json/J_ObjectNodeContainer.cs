namespace net.minecraft.src.json
{
	class J_ObjectNodeContainer : J_NodeContainer {
		readonly J_JsonObjectNodeBuilder field_27296_a;
		readonly J_JsonListenerToJdomAdapter field_27295_b;

		public J_ObjectNodeContainer(J_JsonListenerToJdomAdapter var1, J_JsonObjectNodeBuilder var2) {
			this.field_27295_b = var1;
			this.field_27296_a = var2;
		}

		public void func_27290_a(J_JsonNodeBuilder var1) {
			throw new ApplicationException("Coding failure in Argo:  Attempt to add a node to an object.");
		}

		public void func_27289_a(J_JsonFieldBuilder var1) {
			this.field_27296_a.func_27237_a(var1);
		}
	}
}

