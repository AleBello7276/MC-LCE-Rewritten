

using com.mojang.minecraft.level;

namespace com.mojang.minecraft.level.tile
{
   public class GrassTile : Tile {
      public GrassTile(int id) : base(id){
      
         this.tex = 3;
         this.setTicking(true);
      }

      protected override int getTexture(int face) {
            if (face == 1)
            {
                return 0;
            }
            else
            {
                return face == 0 ? 2 : 3;
            }
        }

      public override void tick(Level level, int x, int y, int z, Random random) {
         if (new Random().Next(4) == 0) {
            if (!level.isLit(x, y + 1, z)) {
               level.setTile(x, y, z, Tile.dirt.id);
            } else {
               for(int i = 0; i < 4; ++i) {
                  int xt = x + new Random().Next(3) - 1;
                  int yt = y + new Random().Next(5) - 3;
                  int zt = z + new Random().Next(3) - 1;
                  if (level.getTile(xt, yt, zt) == Tile.dirt.id && level.isLit(xt, yt + 1, zt)) {
                     level.setTile(xt, yt, zt, Tile.grass.id);
                  }
               }
            }

         }
      }
   }
   
}
