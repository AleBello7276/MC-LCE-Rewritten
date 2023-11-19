namespace net.minecraft.src 
{
	public class MaterialTransparent : Material 
	{
		public MaterialTransparent(MapColor var1) :base(var1)
		{
			this.func_27284_f();
		}

		public override bool isSolid() {
			return false;
		}

		public override bool getCanBlockGrass() {
			return false;
		}

		public override bool getIsSolid() {
			return false;
		}
	}
}

