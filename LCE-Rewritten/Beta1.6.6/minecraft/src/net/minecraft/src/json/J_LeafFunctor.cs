namespace net.minecraft.src.json
{
	abstract class J_LeafFunctor : J_Functor {
        public abstract bool func_27058_a(object var1);
        

        public Object func_27059_b(Object var1) {
			if(!this.func_27058_a(var1)) {
				throw J_JsonNodeDoesNotMatchChainedJsonNodeSelectorException.func_27322_a(this);
			} else {
				return this.func_27063_c(var1);
			}
		}

        public abstract string func_27060_a();
        

        public abstract Object func_27063_c(Object var1);
	}
}

