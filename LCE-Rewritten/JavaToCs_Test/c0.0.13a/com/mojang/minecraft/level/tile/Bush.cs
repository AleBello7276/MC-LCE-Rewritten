

using com.mojang.minecraft.level;
using com.mojang.minecraft.phys;
using com.mojang.minecraft.renderer;


namespace com.mojang.minecraft.level.tile
{
   public class Bush : Tile {
      public Bush(int id) :base(id){
        
         this.tex = 15;
         this.setTicking(true);
      }

        public override void tick(Level level, int x, int y, int z, Random random) {
         int below = level.getTile(x, y - 1, z);
         if (!level.isLit(x, y, z) || below != Tile.dirt.id && below != Tile.grass.id) {
            level.setTile(x, y, z, 0);
         }

      }

        public override void render(Tesselator t, Level level, int layer, int x, int y, int z) {
         if (!(level.isLit(x, y, z) ^ layer != 1)) {
            int tex = this.getTexture(15);
            float u0 = (float)(tex % 16) / 16.0F;
            float u1 = u0 + 0.0624375F;
            float v0 = (float)(tex / 16) / 16.0F;
            float v1 = v0 + 0.0624375F;
            int rots = 2;
            t.color((int)255, (int)255, (int)255);

            for(int r = 0; r < rots; ++r) {
               float xa = (float)(Math.Sin((double)r * 3.141592653589793D / (double)rots + 0.7853981633974483D) * 0.5D);
               float za = (float)(Math.Cos((double)r * 3.141592653589793D / (double)rots + 0.7853981633974483D) * 0.5D);
               float x0 = (float)x + 0.5F - xa;
               float x1 = (float)x + 0.5F + xa;
               float y0 = (float)y + 0.0F;
               float y1 = (float)y + 1.0F;
               float z0 = (float)z + 0.5F - za;
               float z1 = (float)z + 0.5F + za;
               t.vertexUV(x0, y1, z0, u1, v0);
               t.vertexUV(x1, y1, z1, u0, v0);
               t.vertexUV(x1, y0, z1, u0, v1);
               t.vertexUV(x0, y0, z0, u1, v1);
               t.vertexUV(x1, y1, z1, u1, v0);
               t.vertexUV(x0, y1, z0, u0, v0);
               t.vertexUV(x0, y0, z0, u0, v1);
               t.vertexUV(x1, y0, z1, u1, v1);
            }

         }
      }

      public AABB getAABB(int x, int y, int z) {
         return null;
      }

      public bool blocksLight() {
         return false;
      }

      public bool isSolid() {
         return false;
      }
   }

}
