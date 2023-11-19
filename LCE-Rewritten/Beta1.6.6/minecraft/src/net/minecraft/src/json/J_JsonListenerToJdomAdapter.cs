using System.Collections.Generic;
namespace net.minecraft.src.json
{
	sealed class J_JsonListenerToJdomAdapter : J_JsonListener {
		private readonly Stack<J_NodeContainer> field_27210_a = new Stack<J_NodeContainer>();
		private J_JsonNodeBuilder field_27209_b;

		public J_JsonRootNode func_27208_a() {
			return (J_JsonRootNode)this.field_27209_b.func_27234_b();
		}

		public void func_27195_b() {
		}

		public void func_27204_c() {
		}

		public void func_27200_d() {
			J_JsonArrayNodeBuilder var1 = J_JsonNodeBuilders.func_27249_e();
			this.func_27207_a(var1);
			this.field_27210_a.Push(new J_ArrayNodeContainer(this, var1));
		}

		public void func_27197_e() {
			this.field_27210_a.Pop();
		}

		public void func_27194_f() {
			J_JsonObjectNodeBuilder var1 = J_JsonNodeBuilders.func_27253_d();
			this.func_27207_a(var1);
			this.field_27210_a.Push(new J_ObjectNodeContainer(this, var1));
		}

		public void func_27203_g() {
			this.field_27210_a.Pop();
		}

		public void func_27205_a(String var1) {
			J_JsonFieldBuilder var2 = J_JsonFieldBuilder.func_27301_a().func_27304_a(J_JsonNodeBuilders.func_27254_b(var1));
			((J_NodeContainer)this.field_27210_a.Peek()).func_27289_a(var2);
			this.field_27210_a.Push(new J_FieldNodeContainer(this, var2));
		}

		public void func_27199_h() {
			this.field_27210_a.Pop();
		}

		public void func_27201_b(String var1) {
			this.func_27206_b(J_JsonNodeBuilders.func_27250_a(var1));
		}

		public void func_27196_i() {
			this.func_27206_b(J_JsonNodeBuilders.func_27251_b());
		}

		public void func_27198_c(String var1) {
			this.func_27206_b(J_JsonNodeBuilders.func_27254_b(var1));
		}

		public void func_27193_j() {
			this.func_27206_b(J_JsonNodeBuilders.func_27252_c());
		}

		public void func_27202_k() {
			this.func_27206_b(J_JsonNodeBuilders.func_27248_a());
		}

		private void func_27207_a(J_JsonNodeBuilder var1) {
			if(this.field_27209_b == null) {
				this.field_27209_b = var1;
			} else {
				this.func_27206_b(var1);
			}

		}

		private void func_27206_b(J_JsonNodeBuilder var1) {
			((J_NodeContainer)this.field_27210_a.Peek()).func_27290_a(var1);
		}
	}

}

