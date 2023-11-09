

using com.mojang.minecraft.level;
using com.mojang.minecraft.phys;
using com.mojang.minecraft.renderer;

namespace com.mojang.minecraft.level.tile
{
   public class LiquidTile : Tile {
      protected int liquidType;
      protected int calmTileId;
      protected int tileId;
      protected int spreadSpeed = 1;

      public LiquidTile(int id, int liquidType) : base(id){
         
         this.liquidType = liquidType;
         this.tex = 14;
         if (liquidType == 2) {
            this.tex = 30;
         }

         if (liquidType == 1) {
            this.spreadSpeed = 8;
         }

         if (liquidType == 2) {
            this.spreadSpeed = 2;
         }

         this.tileId = id;
         this.calmTileId = id + 1;
         float dd = 0.1F;
         this.setShape(0.0F, 0.0F - dd, 0.0F, 1.0F, 1.0F - dd, 1.0F);
         this.setTicking(true);
      }

        public override void tick(Level level, int x, int y, int z, Random random) {
         this.updateWater(level, x, y, z, 0);
      }

      public bool updateWater(Level level, int x, int y, int z, int depth) {
         bool hasChanged = false;

         bool change;
         do {
            --y;
            if (level.getTile(x, y, z) != 0) {
               break;
            }

            change = level.setTile(x, y, z, this.tileId);
            if (change) {
               hasChanged = true;
            }
         } while(change && this.liquidType != 2);

         ++y;
         if (this.liquidType == 1 || !hasChanged) {
            hasChanged |= this.checkWater(level, x - 1, y, z, depth);
            hasChanged |= this.checkWater(level, x + 1, y, z, depth);
            hasChanged |= this.checkWater(level, x, y, z - 1, depth);
            hasChanged |= this.checkWater(level, x, y, z + 1, depth);
         }

         if (!hasChanged) {
            level.setTileNoUpdate(x, y, z, this.calmTileId);
         }

         return hasChanged;
      }

      private bool checkWater(Level level, int x, int y, int z, int depth) {
         bool hasChanged = false;
         int type = level.getTile(x, y, z);
         if (type == 0) {
            bool changed = level.setTile(x, y, z, this.tileId);
            if (changed && depth < this.spreadSpeed) {
               hasChanged |= this.updateWater(level, x, y, z, depth + 1);
            }
         }

         return hasChanged;
      }

        protected override bool shouldRenderFace(Level level, int x, int y, int z, int layer, int face) {
         if (x >= 0 && y >= 0 && z >= 0 && x < level.width && z < level.height) {
            if (layer != 2 && this.liquidType == 1) {
               return false;
            } else {
               int id = level.getTile(x, y, z);
               return id != this.tileId && id != this.calmTileId ? base.shouldRenderFace(level, x, y, z, -1, face) : false;
            }
         } else {
            return false;
         }
      }

        public override void renderFace(Tesselator t, int x, int y, int z, int face) {
         base.renderFace(t, x, y, z, face);
         base.renderBackFace(t, x, y, z, face);
      }

        public override bool mayPick() {
         return false;
      }

        public override AABB getAABB(int x, int y, int z) {
         return null;
      }

        public override bool blocksLight() {
         return true;
      }

        public override bool isSolid() {
         return false;
      }

        public override int getLiquidType() {
         return this.liquidType;
      }

        public override void neighborChanged(Level level, int x, int y, int z, int type) {
         if (this.liquidType == 1 && (type == Tile.lava.id || type == Tile.calmLava.id)) {
            level.setTileNoUpdate(x, y, z, Tile.rock.id);
         }

         if (this.liquidType == 2 && (type == Tile.water.id || type == Tile.calmWater.id)) {
            level.setTileNoUpdate(x, y, z, Tile.rock.id);
         }

      }
   }

}

