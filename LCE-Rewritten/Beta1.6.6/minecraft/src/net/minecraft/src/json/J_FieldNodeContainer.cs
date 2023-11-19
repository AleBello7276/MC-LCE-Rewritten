namespace net.minecraft.src.json
{
	class J_FieldNodeContainer : J_NodeContainer {
		public readonly J_JsonFieldBuilder field_27292_a;
		public readonly J_JsonListenerToJdomAdapter field_27291_b;

		public J_FieldNodeContainer(J_JsonListenerToJdomAdapter var1, J_JsonFieldBuilder var2) {
			this.field_27291_b = var1;
			this.field_27292_a = var2;
		}

		public void func_27290_a(J_JsonNodeBuilder var1) {
			this.field_27292_a.func_27300_b(var1);
		}

		public void func_27289_a(J_JsonFieldBuilder var1) {
			throw new ApplicationException("Coding failure in Argo:  Attempt to add a field to a field.");
		}
	}
}

