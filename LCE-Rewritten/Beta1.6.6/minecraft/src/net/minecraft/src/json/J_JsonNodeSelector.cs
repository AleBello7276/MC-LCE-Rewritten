namespace net.minecraft.src.json
{
	public sealed class J_JsonNodeSelector {
		readonly J_Functor field_27359_a;

		public J_JsonNodeSelector(J_Functor var1) {
			this.field_27359_a = var1;
		}

		public bool func_27356_a(Object var1) {
			return this.field_27359_a.func_27058_a(var1);
		}

		public Object func_27357_b(Object var1) {
			return this.field_27359_a.func_27059_b(var1);
		}

		public J_JsonNodeSelector func_27355_a(J_JsonNodeSelector var1) {
			return new J_JsonNodeSelector(new J_ChainedFunctor(this, var1));
		}

		public String func_27358_a() {
			return this.field_27359_a.func_27060_a();
		}

		public String toString() {
			return this.field_27359_a.ToString();
		}

        public static explicit operator J_JsonNodeSelector(string v)
        {
            throw new NotImplementedException();
        }
    }

}
