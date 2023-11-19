using System;
using System.Collections;
using System.Collections.Generic;

namespace net.minecraft.src.json
{
	public sealed class J_JsonArrayNodeBuilder : J_JsonNodeBuilder {
		private readonly List<J_JsonNodeBuilder> field_27242_a = new List<J_JsonNodeBuilder>();
		public J_JsonArrayNodeBuilder func_27240_a(J_JsonNodeBuilder var1) {
			this.field_27242_a.Add(var1);
			return this;
		}

		public J_JsonRootNode func_27241_a() {
			List<J_JsonNode> var1 = new List<J_JsonNode>();

			foreach (J_JsonNodeBuilder var3 in this.field_27242_a)
            {
                var1.Add(var3.func_27234_b());
            }
			

			return J_JsonNodeFactories.func_27309_a(var1);
		}

		public J_JsonNode func_27234_b() {
			return this.func_27241_a();
		}
	}
}

