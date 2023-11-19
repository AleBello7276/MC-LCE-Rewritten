

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using net.minecraft.src.entity;
using net.minecraft.src.nbt;

namespace net.minecraft.src.world.chunk
{
	public class ChunkLoader : IChunkLoader {
		private DirectoryInfo saveDir;
        private bool createIfNecessary;

		public ChunkLoader(DirectoryInfo  var1, bool var2) {
			this.saveDir = var1;
			this.createIfNecessary = var2;
		}

		private FileInfo chunkFileForXZ (int var1, int var2) {
			String var3 = "c." + Convert.ToString(var1, 36) + "." + Convert.ToString(var2, 36) + ".dat";
			String var4 = Convert.ToString(var1 & 63, 36);
			String var5 = Convert.ToString(var2 & 63, 36);
			DirectoryInfo  var6 = new DirectoryInfo(Path.Combine(this.saveDir.FullName, var4));
			if(!var6.Exists) {
				if(!this.createIfNecessary) {
					return null;
				}

				var6.Create();
			}

			var6 = new DirectoryInfo(Path.Combine(var6.FullName, var5));
			if(!var6.Exists) {
				if(!this.createIfNecessary) {
					return null;
				}

				var6.Create();
			}

			
			return !var6.Exists && !this.createIfNecessary ? null : new FileInfo(Path.Combine(var6.FullName, var3));
		}

		public Chunk loadChunk(World var1, int var2, int var3) {
			FileInfo var4 = this.chunkFileForXZ(var2, var3);
			if(var4 != null && var4.Exists) {
				try 
				{
					
					using (FileStream var5 = new FileStream(var4.FullName, FileMode.Open))
					{
						NBTTagCompound var6 = CompressedStreamTools.func_1138_a(var5);
						if(!var6.hasKey("Level")) {
							Console.WriteLine("Chunk file at " + var2 + "," + var3 + " is missing level data, skipping");
							return null;
						}

						if(!var6.getCompoundTag("Level").hasKey("Blocks")) {
							Console.WriteLine("Chunk file at " + var2 + "," + var3 + " is missing block data, skipping");
							return null;
						}

						Chunk var7 = loadChunkIntoWorldFromCompound(var1, var6.getCompoundTag("Level"));
						if(!var7.isAtLocation(var2, var3)) {
							Console.WriteLine("Chunk file at " + var2 + "," + var3 + " is in the wrong location; relocating. (Expected " + var2 + ", " + var3 + ", got " + var7.xPosition + ", " + var7.zPosition + ")");
							var6.setInteger("xPos", var2);
							var6.setInteger("zPos", var3);
							var7 = loadChunkIntoWorldFromCompound(var1, var6.getCompoundTag("Level"));
						}

						var7.func_25124_i();
						return var7;
					}
					
				} 
				catch (Exception var8) {
					var8.StackTrace.ToString();
				}
			}

			return null;
		}

		public void saveChunk(World var1, Chunk var2) {
			var1.checkSessionLock();
			FileInfo var3 = this.chunkFileForXZ(var2.xPosition, var2.zPosition);
			if(var3.exists()) {
				WorldInfo var4 = var1.getWorldInfo();
				var4.setSizeOnDisk(var4.getSizeOnDisk() - var3.length());
			}

			try {
				FileInfo var10 = new FileInfo(Path.Combine(this.saveDir.FullName, "tmp_chunk.dat"));
				using (FileStream var5 = new FileStream(var10.FullName, FileMode.Create))
				{
					NBTTagCompound var6 = new NBTTagCompound();
					NBTTagCompound var7 = new NBTTagCompound();
					var6.setTag("Level", var7);
					storeChunkInCompound(var2, var1, var7);
					CompressedStreamTools.writeGzippedCompoundToOutputStream(var6, var5);
					var5.Close();
				}
				if(var3.Exists) {
					var3.Delete();
				}

				var10.MoveTo(var3.FullName);
				WorldInfo var8 = var1.getWorldInfo();
				var8.setSizeOnDisk(var8.getSizeOnDisk() + var3.Length);
			} catch (Exception var9) {
				var9.StackTrace.ToString();
			}

		}

		public static void storeChunkInCompound(Chunk var0, World var1, NBTTagCompound var2) {
			var1.checkSessionLock();
			var2.setInteger("xPos", var0.xPosition);
			var2.setInteger("zPos", var0.zPosition);
			var2.setLong("LastUpdate", var1.getWorldTime());
			var2.setByteArray("Blocks", var0.blocks);
			var2.setByteArray("Data", var0.data.data);
			var2.setByteArray("SkyLight", var0.skylightMap.data);
			var2.setByteArray("BlockLight", var0.blocklightMap.data);
			var2.setByteArray("HeightMap", var0.heightMap);
			var2.setBoolean("TerrainPopulated", var0.isTerrainPopulated);
			var0.hasEntities = false;
			NBTTagList var3 = new NBTTagList();

			NBTTagCompound var7;
			foreach (IEnumerable var5 in var0.entities) {
				
				foreach (Entity var6 in (IEnumerable<Entity>)var5) {
					
					var0.hasEntities = true;
					var7 = new NBTTagCompound();
					if(var6.addEntityID(var7)) {
						var3.setTag(var7);
					}
				}
			}

			var2.setTag("Entities", var3);
			NBTTagList var8 = new NBTTagList();
			

			foreach (TileEntity var9 in var0.chunkTileEntityMap.Values) {
				
				var7 = new NBTTagCompound();
				var9.writeToNBT(var7);
				var8.setTag(var7);
			}

			var2.setTag("TileEntities", var8);
		}

		public static Chunk loadChunkIntoWorldFromCompound(World var0, NBTTagCompound var1) {
			int var2 = var1.getInteger("xPos");
			int var3 = var1.getInteger("zPos");
			Chunk var4 = new Chunk(var0, var2, var3);
			var4.blocks = var1.getByteArray("Blocks");
			var4.data = new NibbleArray(var1.getByteArray("Data"));
			var4.skylightMap = new NibbleArray(var1.getByteArray("SkyLight"));
			var4.blocklightMap = new NibbleArray(var1.getByteArray("BlockLight"));
			var4.heightMap = var1.getByteArray("HeightMap");
			var4.isTerrainPopulated = var1.getBoolean("TerrainPopulated");
			if(!var4.data.isValid()) {
				var4.data = new NibbleArray(var4.blocks.Length);
			}

			if(var4.heightMap == null || !var4.skylightMap.isValid()) {
				var4.heightMap = new byte[256];
				var4.skylightMap = new NibbleArray(var4.blocks.Length);
				var4.func_1024_c();
			}

			if(!var4.blocklightMap.isValid()) {
				var4.blocklightMap = new NibbleArray(var4.blocks.Length);
				var4.func_1014_a();
			}

			NBTTagList var5 = var1.getTagList("Entities");
			if(var5 != null) {
				for(int var6 = 0; var6 < var5.tagCount(); ++var6) {
					NBTTagCompound var7 = (NBTTagCompound)var5.tagAt(var6);
					Entity var8 = EntityList.createEntityFromNBT(var7, var0);
					var4.hasEntities = true;
					if(var8 != null) {
						var4.addEntity(var8);
					}
				}
			}

			NBTTagList var10 = var1.getTagList("TileEntities");
			if(var10 != null) {
				for(int var11 = 0; var11 < var10.tagCount(); ++var11) {
					NBTTagCompound var12 = (NBTTagCompound)var10.tagAt(var11);
					TileEntity var9 = TileEntity.createAndLoadEntity(var12);
					if(var9 != null) {
						var4.func_1001_a(var9);
					}
				}
			}

			return var4;
		}

		public void func_814_a() {
		}

		public void saveExtraData() {
		}

		public void saveExtraChunkData(World var1, Chunk var2)  {
		}
	}

}
