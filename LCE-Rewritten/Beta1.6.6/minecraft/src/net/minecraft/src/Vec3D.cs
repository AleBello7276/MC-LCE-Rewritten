namespace net.minecraft.src
{
	public class Vec3D {
		private static List<Vec3D> vectorList = new List<Vec3D>();
		private static int nextVector = 0;
		public double xCoord;
		public double yCoord;
		public double zCoord;

		public static Vec3D createVectorHelper(double var0, double var2, double var4) {
			return new Vec3D(var0, var2, var4);
		}

		public static void func_28215_a() {
			vectorList.Clear();
			nextVector = 0;
		}

		public static void initialize() {
			nextVector = 0;
		}

		public static Vec3D createVector(double var0, double var2, double var4) {
			if(nextVector >= vectorList.Count) {
				vectorList.Add(createVectorHelper(0.0D, 0.0D, 0.0D));
			}

			return ((Vec3D)vectorList[nextVector++]).setComponents(var0, var2, var4);
		}

		private Vec3D(double var1, double var3, double var5) {
			if(var1 == -0.0D) {
				var1 = 0.0D;
			}

			if(var3 == -0.0D) {
				var3 = 0.0D;
			}

			if(var5 == -0.0D) {
				var5 = 0.0D;
			}

			this.xCoord = var1;
			this.yCoord = var3;
			this.zCoord = var5;
		}

		private Vec3D setComponents(double var1, double var3, double var5) {
			this.xCoord = var1;
			this.yCoord = var3;
			this.zCoord = var5;
			return this;
		}

		public Vec3D subtract(Vec3D var1) {
			return createVector(var1.xCoord - this.xCoord, var1.yCoord - this.yCoord, var1.zCoord - this.zCoord);
		}

		public Vec3D normalize() {
			double var1 = (double)MathHelper.sqrt_double(this.xCoord * this.xCoord + this.yCoord * this.yCoord + this.zCoord * this.zCoord);
			return var1 < 1.0E-4D ? createVector(0.0D, 0.0D, 0.0D) : createVector(this.xCoord / var1, this.yCoord / var1, this.zCoord / var1);
		}

		public Vec3D crossProduct(Vec3D var1) {
			return createVector(this.yCoord * var1.zCoord - this.zCoord * var1.yCoord, this.zCoord * var1.xCoord - this.xCoord * var1.zCoord, this.xCoord * var1.yCoord - this.yCoord * var1.xCoord);
		}

		public Vec3D addVector(double var1, double var3, double var5) {
			return createVector(this.xCoord + var1, this.yCoord + var3, this.zCoord + var5);
		}

		public double distanceTo(Vec3D var1) {
			double var2 = var1.xCoord - this.xCoord;
			double var4 = var1.yCoord - this.yCoord;
			double var6 = var1.zCoord - this.zCoord;
			return (double)MathHelper.sqrt_double(var2 * var2 + var4 * var4 + var6 * var6);
		}

		public double squareDistanceTo(Vec3D var1) {
			double var2 = var1.xCoord - this.xCoord;
			double var4 = var1.yCoord - this.yCoord;
			double var6 = var1.zCoord - this.zCoord;
			return var2 * var2 + var4 * var4 + var6 * var6;
		}

		public double squareDistanceTo(double var1, double var3, double var5) {
			double var7 = var1 - this.xCoord;
			double var9 = var3 - this.yCoord;
			double var11 = var5 - this.zCoord;
			return var7 * var7 + var9 * var9 + var11 * var11;
		}

		public double lengthVector() {
			return (double)MathHelper.sqrt_double(this.xCoord * this.xCoord + this.yCoord * this.yCoord + this.zCoord * this.zCoord);
		}

		public Vec3D getIntermediateWithXValue(Vec3D var1, double var2) {
			double var4 = var1.xCoord - this.xCoord;
			double var6 = var1.yCoord - this.yCoord;
			double var8 = var1.zCoord - this.zCoord;
			if(var4 * var4 < (double)1.0E-7F) {
				return null;
			} else {
				double var10 = (var2 - this.xCoord) / var4;
				return var10 >= 0.0D && var10 <= 1.0D ? createVector(this.xCoord + var4 * var10, this.yCoord + var6 * var10, this.zCoord + var8 * var10) : null;
			}
		}

		public Vec3D getIntermediateWithYValue(Vec3D var1, double var2) {
			double var4 = var1.xCoord - this.xCoord;
			double var6 = var1.yCoord - this.yCoord;
			double var8 = var1.zCoord - this.zCoord;
			if(var6 * var6 < (double)1.0E-7F) {
				return null;
			} else {
				double var10 = (var2 - this.yCoord) / var6;
				return var10 >= 0.0D && var10 <= 1.0D ? createVector(this.xCoord + var4 * var10, this.yCoord + var6 * var10, this.zCoord + var8 * var10) : null;
			}
		}

		public Vec3D getIntermediateWithZValue(Vec3D var1, double var2) {
			double var4 = var1.xCoord - this.xCoord;
			double var6 = var1.yCoord - this.yCoord;
			double var8 = var1.zCoord - this.zCoord;
			if(var8 * var8 < (double)1.0E-7F) {
				return null;
			} else {
				double var10 = (var2 - this.zCoord) / var8;
				return var10 >= 0.0D && var10 <= 1.0D ? createVector(this.xCoord + var4 * var10, this.yCoord + var6 * var10, this.zCoord + var8 * var10) : null;
			}
		}

		public String toString() {
			return "(" + this.xCoord + ", " + this.yCoord + ", " + this.zCoord + ")";
		}

		public void rotateAroundX(float var1) {
			float var2 = MathHelper.cos(var1);
			float var3 = MathHelper.sin(var1);
			double var4 = this.xCoord;
			double var6 = this.yCoord * (double)var2 + this.zCoord * (double)var3;
			double var8 = this.zCoord * (double)var2 - this.yCoord * (double)var3;
			this.xCoord = var4;
			this.yCoord = var6;
			this.zCoord = var8;
		}

		public void rotateAroundY(float var1) {
			float var2 = MathHelper.cos(var1);
			float var3 = MathHelper.sin(var1);
			double var4 = this.xCoord * (double)var2 + this.zCoord * (double)var3;
			double var6 = this.yCoord;
			double var8 = this.zCoord * (double)var2 - this.xCoord * (double)var3;
			this.xCoord = var4;
			this.yCoord = var6;
			this.zCoord = var8;
		}
	}

}


