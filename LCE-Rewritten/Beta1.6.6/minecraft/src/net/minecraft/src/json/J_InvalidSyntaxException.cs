namespace net.minecraft.src.json
{
	public sealed class J_InvalidSyntaxException : Exception {
	private readonly int field_27191_a;
	private readonly int field_27190_b;

	public J_InvalidSyntaxException(String var1, J_ThingWithPosition var2) :base("At line " + var2.func_27330_b() + ", column " + var2.func_27331_a() + ":  " + var1)
	{
		this.field_27191_a = var2.func_27331_a();
		this.field_27190_b = var2.func_27330_b();
	}

	public J_InvalidSyntaxException(String var1, Exception var2, J_ThingWithPosition var3) :base("At line " + var3.func_27330_b() + ", column " + var3.func_27331_a() + ":  " + var1, var2)
	{
		this.field_27191_a = var3.func_27331_a();
		this.field_27190_b = var3.func_27330_b();
	}
}
}

