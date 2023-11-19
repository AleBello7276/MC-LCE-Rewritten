namespace net.minecraft.src
{
	public class Material 
	{
		public static readonly Material air = new MaterialTransparent(MapColor.Black);
		public static readonly Material grassMaterial = new Material(MapColor.green2);
		public static readonly Material ground = new Material(MapColor.brown1);
		public static readonly Material wood = (new Material(MapColor.brown2)).setBurning();
		public static readonly Material rock = new Material(MapColor.grey1);
		public static readonly Material iron = new Material(MapColor.grey3);
		public static readonly Material water = new MaterialLiquid(MapColor.blue);
		public static readonly Material lava = new MaterialLiquid(MapColor.Red);
		public static readonly Material leaves = (new Material(MapColor.green)).setBurning().func_28127_i();
		public static readonly Material plants = new MaterialLogic(MapColor.green);
		public static readonly Material sponge = new Material(MapColor.grey4);
		public static readonly Material cloth = (new Material(MapColor.grey4)).setBurning();
		public static readonly Material fire = new MaterialTransparent(MapColor.Black);
		public static readonly Material sand = new Material(MapColor.ocher);
		public static readonly Material circuits = new MaterialLogic(MapColor.Black);
		public static readonly Material glass = (new Material(MapColor.Black)).func_28127_i();
		public static readonly Material tnt = (new Material(MapColor.Red)).setBurning().func_28127_i();
		public static readonly Material field_4262_q = new Material(MapColor.green);
		public static readonly Material ice = (new Material(MapColor.lilac)).func_28127_i();
		public static readonly Material snow = (new MaterialLogic(MapColor.white)).func_27284_f().func_28127_i();
		public static readonly Material builtSnow = new Material(MapColor.white);
		public static readonly Material cactus = (new Material(MapColor.green)).func_28127_i();
		public static readonly Material clay = new Material(MapColor.grey2);
		public static readonly Material pumpkin = new Material(MapColor.green);
		public static readonly Material portal = new MaterialPortal(MapColor.Black);
		public static readonly Material cakeMaterial = new Material(MapColor.Black);
		private bool canBurn;
		private bool field_27285_A;
		private bool field_28128_D;
		public readonly MapColor materialMapColor;

		public Material(MapColor var1) {
			this.materialMapColor = var1;
		}

		public virtual bool getIsLiquid() {
			return false;
		}

		public virtual bool isSolid() {
			return true;
		}

		public virtual bool getCanBlockGrass() {
			return true;
		}

		public virtual bool getIsSolid() {
			return true;
		}

		private Material func_28127_i() {
			this.field_28128_D = true;
			return this;
		}

		private Material setBurning() {
			this.canBurn = true;
			return this;
		}

		public virtual bool getBurning() {
			return this.canBurn;
		}

		public Material func_27284_f() {
			this.field_27285_A = true;
			return this;
		}

		public virtual bool func_27283_g() {
			return this.field_27285_A;
		}

		public virtual bool func_28126_h() {
			return this.field_28128_D ? false : this.getIsSolid();
		}
	}

}
