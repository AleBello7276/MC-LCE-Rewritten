namespace net.minecraft.src
{
	public class MapColor 
	{
		// color names from AleBello because retromcp didn't map some before the names were like "field_28210_d" :}

		public static readonly MapColor[] mapColorArray = new MapColor[16];
		public static readonly MapColor Black = new MapColor(0, 0);
		public static readonly MapColor green2 = new MapColor(1, 8368696);
		public static readonly MapColor ocher = new MapColor(2, 16247203);
		public static readonly MapColor grey4 = new MapColor(3, 10987431);
		public static readonly MapColor Red = new MapColor(4, 16711680);
		public static readonly MapColor lilac = new MapColor(5, 10526975);
		public static readonly MapColor grey3 = new MapColor(6, 10987431);
		public static readonly MapColor green = new MapColor(7, 31744);
		public static readonly MapColor white = new MapColor(8, 16777215);
		public static readonly MapColor grey2 = new MapColor(9, 10791096);
		public static readonly MapColor brown1 = new MapColor(10, 12020271);
		public static readonly MapColor grey1 = new MapColor(11, 7368816);
		public static readonly MapColor blue = new MapColor(12, 4210943);
		public static readonly MapColor brown2 = new MapColor(13, 6837042);
		public readonly int colorValue;
		public readonly int colorIndex;

		private MapColor(int inx, int val) {
			this.colorIndex = inx;
			this.colorValue = val;
			mapColorArray[inx] = this;
		}
	}

}

