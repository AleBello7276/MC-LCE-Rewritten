namespace net.minecraft.src
{
	public class MaterialLogic : Material 
	{
		public MaterialLogic(MapColor var1) :base(var1)
		{
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


