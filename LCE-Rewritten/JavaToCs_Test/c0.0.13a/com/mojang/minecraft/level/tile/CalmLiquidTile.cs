
using com.mojang.minecraft.level;


namespace com.mojang.minecraft.level.tile
{
   public class CalmLiquidTile : LiquidTile {
      public CalmLiquidTile(int id, int liquidType) :base(id, liquidType){
         
         this.tileId = id - 1;
         this.calmTileId = id;
         this.setTicking(false);
      }

      public override void tick(Level level, int x, int y, int z, Random random) {
      }

        public override void neighborChanged(Level level, int x, int y, int z, int type) {
         bool hasAirNeighbor = false;
         if (level.getTile(x - 1, y, z) == 0) {
            hasAirNeighbor = true;
         }

         if (level.getTile(x + 1, y, z) == 0) {
            hasAirNeighbor = true;
         }

         if (level.getTile(x, y, z - 1) == 0) {
            hasAirNeighbor = true;
         }

         if (level.getTile(x, y, z + 1) == 0) {
            hasAirNeighbor = true;
         }

         if (level.getTile(x, y - 1, z) == 0) {
            hasAirNeighbor = true;
         }

         if (hasAirNeighbor) {
            level.setTileNoUpdate(x, y, z, this.tileId);
         }

         if (this.liquidType == 1 && type == Tile.lava.id) {
            level.setTileNoUpdate(x, y, z, Tile.rock.id);
         }

         if (this.liquidType == 2 && type == Tile.water.id) {
            level.setTileNoUpdate(x, y, z, Tile.rock.id);
         }

      }
   }

}
