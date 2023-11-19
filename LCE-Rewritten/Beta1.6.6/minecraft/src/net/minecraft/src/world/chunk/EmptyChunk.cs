
using net.minecraft.util;



using net.minecraft.src;
using net.minecraft.src.entity;
using net.minecraft.src.world;

namespace net.minecraft.src.world.chunk
{
	public class EmptyChunk : Chunk {
		public EmptyChunk(World var1, int var2, int var3) :base(var1, var2, var3)
		{
			this.neverSave = true;
		}

		public EmptyChunk(World var1, byte[] var2, int var3, int var4) :base(var1, var2, var3, var4)
		{
			this.neverSave = true;
		}

		public bool isAtLocation(int var1, int var2) {
			return var1 == this.xPosition && var2 == this.zPosition;
		}

		public int getHeightValue(int var1, int var2) {
			return 0;
		}

		public void func_1014_a() {
		}

		public void generateHeightMap() {
		}

		public void func_1024_c() {
		}

		public void func_4143_d() {
		}

		public int getBlockID(int var1, int var2, int var3) {
			return 0;
		}

		public bool setBlockIDWithMetadata(int var1, int var2, int var3, int var4, int var5) {
			return true;
		}

		public bool setBlockID(int var1, int var2, int var3, int var4) {
			return true;
		}

		public int getBlockMetadata(int var1, int var2, int var3) {
			return 0;
		}

		public void setBlockMetadata(int var1, int var2, int var3, int var4) {
		}

		public int getSavedLightValue(EnumSkyBlock var1, int var2, int var3, int var4) {
			return 0;
		}

		public void setLightValue(EnumSkyBlock var1, int var2, int var3, int var4, int var5) {
		}

		public int getBlockLightValue(int var1, int var2, int var3, int var4) {
			return 0;
		}

		public void addEntity(Entity var1) {
		}

		public void removeEntity(Entity var1) {
		}

		public void removeEntityAtIndex(Entity var1, int var2) {
		}

		public bool canBlockSeeTheSky(int var1, int var2, int var3) {
			return false;
		}

		public TileEntity getChunkBlockTileEntity(int var1, int var2, int var3) {
			return null;
		}

		public void func_1001_a(TileEntity var1) {
		}

		public void setChunkBlockTileEntity(int var1, int var2, int var3, TileEntity var4) {
		}

		public void removeChunkBlockTileEntity(int var1, int var2, int var3) {
		}

		public void onChunkLoad() {
		}

		public void onChunkUnload() {
		}

		public void setChunkModified() {
		}

		public void getEntitiesWithinAABBForEntity(Entity var1, AxisAlignedBB var2, List<object> var3) {
		}

		public void getEntitiesOfTypeWithinAAAB(Type var1, AxisAlignedBB var2, List<object> var3) {
		}

		public bool needsSaving(bool var1) {
			return false;
		}

		public int setChunkData(byte[] var1, int var2, int var3, int var4, int var5, int var6, int var7, int var8) {
			int var9 = var5 - var2;
			int var10 = var6 - var3;
			int var11 = var7 - var4;
			int var12 = var9 * var10 * var11;
			return var12 + var12 / 2 * 3;
		}

		public JavaRandom func_997_a(long var1) {
			return new JavaRandom(this.worldObj.getRandomSeed() + (long)(this.xPosition * this.xPosition * 4987142) + (long)(this.xPosition * 5947611) + (long)(this.zPosition * this.zPosition) * 4392871L + (long)(this.zPosition * 389711) ^ var1);
		}

		public bool func_21167_h() {
			return true;
		}
	}

}

