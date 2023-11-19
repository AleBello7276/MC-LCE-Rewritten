
using net.minecraft.src.entity.tileEntity;
using net.minecraft.src.world;

namespace net.minecraft.src
{
	public interface IBlockAccess {
		int getBlockId(int var1, int var2, int var3);

		TileEntity getBlockTileEntity(int var1, int var2, int var3);

		float getBrightness(int var1, int var2, int var3, int var4);

		float getLightBrightness(int var1, int var2, int var3);

		int getBlockMetadata(int var1, int var2, int var3);

		Material getBlockMaterial(int var1, int var2, int var3);

		bool isBlockOpaqueCube(int var1, int var2, int var3);

		bool func_28100_h(int var1, int var2, int var3);

		WorldChunkManager getWorldChunkManager();
	}
}

