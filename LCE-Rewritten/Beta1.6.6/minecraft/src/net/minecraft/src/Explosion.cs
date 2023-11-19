using net.minecraft.src;
using net.minecraft.util;
using net.minecraft.src.blocks;
using net.minecraft.src.entity;
using net.minecraft.src.world;
using net.minecraft.src.world.chunk;

namespace net.minecraft.src
{
	public class Explosion {
		public bool field_12257_a = false;
		private JavaRandom ExplosionRNG = new JavaRandom();
		private World worldObj;
		public double explosionX;
		public double explosionY;
		public double explosionZ;
		public Entity exploder;
		public float explosionSize;
		public HashSet<ChunkPosition> destroyedBlockPositions { get; set; } = new HashSet<ChunkPosition>();

		public Explosion(World var1, Entity var2, double var3, double var5, double var7, float var9) {
			this.worldObj = var1;
			this.exploder = var2;
			this.explosionSize = var9;
			this.explosionX = var3;
			this.explosionY = var5;
			this.explosionZ = var7;
		}

		public void doExplosionA() {
			float var1 = this.explosionSize;
			byte var2 = 16;

			int var3;
			int var4;
			int var5;
			double var15;
			double var17;
			double var19;
			for(var3 = 0; var3 < var2; ++var3) {
				for(var4 = 0; var4 < var2; ++var4) {
					for(var5 = 0; var5 < var2; ++var5) {
						if(var3 == 0 || var3 == var2 - 1 || var4 == 0 || var4 == var2 - 1 || var5 == 0 || var5 == var2 - 1) {
							double var6 = (double)((float)var3 / ((float)var2 - 1.0F) * 2.0F - 1.0F);
							double var8 = (double)((float)var4 / ((float)var2 - 1.0F) * 2.0F - 1.0F);
							double var10 = (double)((float)var5 / ((float)var2 - 1.0F) * 2.0F - 1.0F);
							double var12 = Math.Sqrt(var6 * var6 + var8 * var8 + var10 * var10);
							var6 /= var12;
							var8 /= var12;
							var10 /= var12;
							float var14 = this.explosionSize * (0.7F + this.worldObj.rand.nextFloat() * 0.6F);
							var15 = this.explosionX;
							var17 = this.explosionY;
							var19 = this.explosionZ;

							for(float var21 = 0.3F; var14 > 0.0F; var14 -= var21 * (12.0F / 16.0F)) {
								int var22 = MathHelper.floor_double(var15);
								int var23 = MathHelper.floor_double(var17);
								int var24 = MathHelper.floor_double(var19);
								int var25 = this.worldObj.getBlockId(var22, var23, var24);
								if(var25 > 0) {
									var14 -= (Block.blocksList[var25].getExplosionResistance(this.exploder) + 0.3F) * var21;
								}

								if(var14 > 0.0F) {
									this.destroyedBlockPositions.Add(new ChunkPosition(var22, var23, var24));
								}

								var15 += var6 * (double)var21;
								var17 += var8 * (double)var21;
								var19 += var10 * (double)var21;
							}
						}
					}
				}
			}

			this.explosionSize *= 2.0F;
			var3 = MathHelper.floor_double(this.explosionX - (double)this.explosionSize - 1.0D);
			var4 = MathHelper.floor_double(this.explosionX + (double)this.explosionSize + 1.0D);
			var5 = MathHelper.floor_double(this.explosionY - (double)this.explosionSize - 1.0D);
			int var29 = MathHelper.floor_double(this.explosionY + (double)this.explosionSize + 1.0D);
			int var7 = MathHelper.floor_double(this.explosionZ - (double)this.explosionSize - 1.0D);
			int var30 = MathHelper.floor_double(this.explosionZ + (double)this.explosionSize + 1.0D);
			List<Entity> var9 = this.worldObj.getEntitiesWithinAABBExcludingEntity(this.exploder, AxisAlignedBB.getBoundingBoxFromPool((double)var3, (double)var5, (double)var7, (double)var4, (double)var29, (double)var30));
			Vec3D var31 = Vec3D.createVector(this.explosionX, this.explosionY, this.explosionZ);

			for(int var11 = 0; var11 < var9.Count; ++var11) {
				Entity var33 = (Entity)var9[var11];
				double var13 = var33.getDistance(this.explosionX, this.explosionY, this.explosionZ) / (double)this.explosionSize;
				if(var13 <= 1.0D) {
					var15 = var33.posX - this.explosionX;
					var17 = var33.posY - this.explosionY;
					var19 = var33.posZ - this.explosionZ;
					double var39 = (double)MathHelper.sqrt_double(var15 * var15 + var17 * var17 + var19 * var19);
					var15 /= var39;
					var17 /= var39;
					var19 /= var39;
					double var40 = (double)this.worldObj.func_675_a(var31, var33.boundingBox);
					double var41 = (1.0D - var13) * var40;
					var33.attackEntityFrom(this.exploder, (int)((var41 * var41 + var41) / 2.0D * 8.0D * (double)this.explosionSize + 1.0D));
					var33.motionX += var15 * var41;
					var33.motionY += var17 * var41;
					var33.motionZ += var19 * var41;
				}
			}

			this.explosionSize = var1;
			List<ChunkPosition> var32 = new List<ChunkPosition>();
			var32.AddRange(destroyedBlockPositions);
			if(this.field_12257_a) {
				for(int var34 = var32.Count - 1; var34 >= 0; --var34) {
					ChunkPosition var35 = (ChunkPosition)var32[var34];
					int var36 = var35.x;
					int var37 = var35.y;
					int var16 = var35.z;
					int var38 = this.worldObj.getBlockId(var36, var37, var16);
					int var18 = this.worldObj.getBlockId(var36, var37 - 1, var16);
					if(var38 == 0 && Block.opaqueCubeLookup[var18] && this.ExplosionRNG.nextInt(3) == 0) {
						this.worldObj.setBlockWithNotify(var36, var37, var16, Block.fire.blockID);
					}
				}
			}

		}

		public void doExplosionB(bool var1) {
			this.worldObj.playSoundEffect(this.explosionX, this.explosionY, this.explosionZ, "random.explode", 4.0F, (1.0F + (this.worldObj.rand.nextFloat() - this.worldObj.rand.nextFloat()) * 0.2F) * 0.7F);
			List<ChunkPosition> var2 = new List<ChunkPosition>();
			var2.AddRange(destroyedBlockPositions);

			for(int var3 = var2.Count - 1; var3 >= 0; --var3) {
				ChunkPosition var4 = (ChunkPosition)var2[var3];
				int var5 = var4.x;
				int var6 = var4.y;
				int var7 = var4.z;
				int var8 = this.worldObj.getBlockId(var5, var6, var7);
				if(var1) {
					double var9 = (double)((float)var5 + this.worldObj.rand.nextFloat());
					double var11 = (double)((float)var6 + this.worldObj.rand.nextFloat());
					double var13 = (double)((float)var7 + this.worldObj.rand.nextFloat());
					double var15 = var9 - this.explosionX;
					double var17 = var11 - this.explosionY;
					double var19 = var13 - this.explosionZ;
					double var21 = (double)MathHelper.sqrt_double(var15 * var15 + var17 * var17 + var19 * var19);
					var15 /= var21;
					var17 /= var21;
					var19 /= var21;
					double var23 = 0.5D / (var21 / (double)this.explosionSize + 0.1D);
					var23 *= (double)(this.worldObj.rand.nextFloat() * this.worldObj.rand.nextFloat() + 0.3F);
					var15 *= var23;
					var17 *= var23;
					var19 *= var23;
					this.worldObj.spawnParticle("explode", (var9 + this.explosionX * 1.0D) / 2.0D, (var11 + this.explosionY * 1.0D) / 2.0D, (var13 + this.explosionZ * 1.0D) / 2.0D, var15, var17, var19);
					this.worldObj.spawnParticle("smoke", var9, var11, var13, var15, var17, var19);
				}

				if(var8 > 0) {
					Block.blocksList[var8].dropBlockAsItemWithChance(this.worldObj, var5, var6, var7, this.worldObj.getBlockMetadata(var5, var6, var7), 0.3F);
					this.worldObj.setBlockWithNotify(var5, var6, var7, 0);
					Block.blocksList[var8].onBlockDestroyedByExplosion(this.worldObj, var5, var6, var7);
				}
			}

		}
	}

}
