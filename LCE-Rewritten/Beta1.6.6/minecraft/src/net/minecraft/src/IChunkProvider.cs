using net.minecraft.src.world.chunk;

namespace net.minecraft.src
{
	public interface IChunkProvider {
		bool chunkExists(int var1, int var2);

		Chunk provideChunk(int var1, int var2);

		Chunk func_538_d(int var1, int var2);

		void populate(IChunkProvider var1, int var2, int var3);

		bool saveChunks(bool var1, IProgressUpdate var2);

		bool func_532_a();

		bool func_536_b();

		string makeString();
	}
}



