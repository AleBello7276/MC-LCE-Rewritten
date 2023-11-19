namespace net.minecraft.src
{
	public class MaterialPortal : Material 
	{
		public MaterialPortal(MapColor var1) :base(var1)
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
