namespace net.minecraft.src.json
{
	class J_ArrayNodeContainer : J_NodeContainer {
		readonly J_JsonArrayNodeBuilder field_27294_a;
		readonly J_JsonListenerToJdomAdapter field_27293_b;

		public J_ArrayNodeContainer(J_JsonListenerToJdomAdapter var1, J_JsonArrayNodeBuilder var2) {
			this.field_27293_b = var1;
			this.field_27294_a = var2;
		}

		public void func_27290_a(J_JsonNodeBuilder var1) {
			this.field_27294_a.func_27240_a(var1);
		}

		public void func_27289_a(J_JsonFieldBuilder var1) {
			throw new ApplicationException("Coding failure in Argo:  Attempt to add a field to an array.");
		}
	}
}

