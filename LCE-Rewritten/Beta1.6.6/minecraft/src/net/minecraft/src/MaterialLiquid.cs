namespace net.minecraft.src
{
	public class MaterialLiquid : Material 
	{
		public MaterialLiquid(MapColor var1) :base(var1)
		{
			this.func_27284_f();
		}

		public override bool getIsLiquid() {
			return true;
		}

		public override bool getIsSolid() {
			return false;
		}

		public override bool isSolid() {
			return false;
		}
	}

}


