

using com.mojang.minecraft.level.tile;
using com.mojang.minecraft.phys;

namespace com.mojang.minecraft.level
{
   public class Level {
      private static readonly int TILE_UPDATE_INTERVAL = 200;
      public int width;
      public int height;
      public int depth;
      public byte[] blocks;
      private int[] lightDepths;
      private List<LevelListener> levelListeners = new List<LevelListener>();
      private Random random = new Random();
      private int randValue;
      public String name;
      public String creator;
      public long createTime;
      int unprocessed;
      private static readonly int multiplier = 1664525;
      private static readonly int addend = 1013904223;

      public Level() {
         this.randValue = new Random().Next();
         this.unprocessed = 0;
      }

      public void setData(int w, int d, int h, byte[] blocks) {
         this.width = w;
         this.height = h;
         this.depth = d;
         this.blocks = blocks;
         this.lightDepths = new int[w * h];
         this.calcLightDepths(0, 0, w, h);

         for(int i = 0; i < this.levelListeners.Count(); ++i) {
            ((LevelListener)this.levelListeners[i]).allChanged();
         }

      }

      public void calcLightDepths(int x0, int y0, int x1, int y1) {
         for(int x = x0; x < x0 + x1; ++x) {
            for(int z = y0; z < y0 + y1; ++z) {
               int oldDepth = this.lightDepths[x + z * this.width];

               int y;
               for(y = this.depth - 1; y > 0 && !this.isLightBlocker(x, y, z); --y) {
               }

               this.lightDepths[x + z * this.width] = y + 1;
               if (oldDepth != y) {
                  int yl0 = oldDepth < y ? oldDepth : y;
                  int yl1 = oldDepth > y ? oldDepth : y;

                  for(int i = 0; i < this.levelListeners.Count(); ++i) {
                     ((LevelListener)this.levelListeners[i]).lightColumnChanged(x, z, yl0, yl1);
                  }
               }
            }
         }

      }

      public void addListener(LevelListener levelListener) {
         this.levelListeners.Add(levelListener);
      }

      public void removeListener(LevelListener levelListener) {
         this.levelListeners.Remove(levelListener);
      }

      public bool isLightBlocker(int x, int y, int z) {
         Tile tile = Tile.tiles[this.getTile(x, y, z)];
         return tile == null ? false : tile.blocksLight();
      }

      public List<AABB> getCubes(AABB box) {
         List<AABB> boxes = new List<AABB>();
         int x0 = (int)Math.Floor((double)box.x0);
         int x1 = (int)Math.Floor((double)(box.x1 + 1.0F));
         int y0 = (int)Math.Floor((double)box.y0);
         int y1 = (int)Math.Floor((double)(box.y1 + 1.0F));
         int z0 = (int)Math.Floor((double)box.z0);
         int z1 = (int)Math.Floor((double)(box.z1 + 1.0F));

         for(int x = x0; x < x1; ++x) {
            for(int y = y0; y < y1; ++y) {
               for(int z = z0; z < z1; ++z) {
                  if (x >= 0 && y >= 0 && z >= 0 && x < this.width && y < this.depth && z < this.height) {
                     Tile tile = Tile.tiles[this.getTile(x, y, z)];
                     if (tile != null) {
                        AABB aabb = tile.getAABB(x, y, z);
                        if (aabb != null) {
                           boxes.Add(aabb);
                        }
                        
                     }
                  } else if (x < 0 || y < 0 || z < 0 || x >= this.width || z >= this.height) {
                     AABB aabb = Tile.unbreakable.getAABB(x, y, z);
                     if (aabb != null) {
                        boxes.Add(aabb);
                     }
                  }
               }
            }
         }

         return boxes;
      }
      
      

      public bool setTile(int x, int y, int z, int type) {
         if (x >= 0 && y >= 0 && z >= 0 && x < this.width && y < this.depth && z < this.height) {
            if (type == this.blocks[(y * this.height + z) * this.width + x]) {
               return false;
            } else {
               this.blocks[(y * this.height + z) * this.width + x] = (byte)type;
               this.neighborChanged(x - 1, y, z, type);
               this.neighborChanged(x + 1, y, z, type);
               this.neighborChanged(x, y - 1, z, type);
               this.neighborChanged(x, y + 1, z, type);
               this.neighborChanged(x, y, z - 1, type);
               this.neighborChanged(x, y, z + 1, type);
               this.calcLightDepths(x, z, 1, 1);

               for(int i = 0; i < this.levelListeners.Count(); ++i) {
                  ((LevelListener)this.levelListeners[i]).tileChanged(x, y, z);
               }

               return true;
            }
         } else {
            return false;
         }
      }

      public bool setTileNoUpdate(int x, int y, int z, int type) {
         if (x >= 0 && y >= 0 && z >= 0 && x < this.width && y < this.depth && z < this.height) {
            if (type == this.blocks[(y * this.height + z) * this.width + x]) {
               return false;
            } else {
               this.blocks[(y * this.height + z) * this.width + x] = (byte)type;
               return true;
            }
         } else {
            return false;
         }
      }

      private void neighborChanged(int x, int y, int z, int type) {
         if (x >= 0 && y >= 0 && z >= 0 && x < this.width && y < this.depth && z < this.height) {
            Tile tile = Tile.tiles[this.blocks[(y * this.height + z) * this.width + x]];
            if (tile != null) {
               tile.neighborChanged(this, x, y, z, type);
            }

         }
      }

      public bool isLit(int x, int y, int z) {
         if (x >= 0 && y >= 0 && z >= 0 && x < this.width && y < this.depth && z < this.height) {
            return y >= this.lightDepths[x + z * this.width];
         } else {
            return true;
         }
      }

      public int getTile(int x, int y, int z) {
         return x >= 0 && y >= 0 && z >= 0 && x < this.width && y < this.depth && z < this.height ? this.blocks[(y * this.height + z) * this.width + x] : 0;
      }

      public bool isSolidTile(int x, int y, int z) {
         Tile tile = Tile.tiles[this.getTile(x, y, z)];
         return tile == null ? false : tile.isSolid();
      }

      public void tick() {
         this.unprocessed += this.width * this.height * this.depth;
         int ticks = this.unprocessed / 200;
         this.unprocessed -= ticks * 200;

         for(int i = 0; i < ticks; ++i) {
            this.randValue = this.randValue * 1664525 + 1013904223;
            int x = this.randValue >> 16 & this.width - 1;
            this.randValue = this.randValue * 1664525 + 1013904223;
            int y = this.randValue >> 16 & this.depth - 1;
            this.randValue = this.randValue * 1664525 + 1013904223;
            int z = this.randValue >> 16 & this.height - 1;
            int id = this.blocks[(y * this.height + z) * this.width + x];
            if (Tile.shouldTick[id]) {
               Tile.tiles[id].tick(this, x, y, z, this.random);
            }
         }

      }

      public float getGroundLevel() {
         return 32.0F;
      }

      public bool containsAnyLiquid(AABB box) {
         int x0 = (int)Math.Floor((double)box.x0);
         int x1 = (int)Math.Floor((double)(box.x1 + 1.0F));
         int y0 = (int)Math.Floor((double)box.y0);
         int y1 = (int)Math.Floor((double)(box.y1 + 1.0F));
         int z0 = (int)Math.Floor((double)box.z0);
         int z1 = (int)Math.Floor((double)(box.z1 + 1.0F));
         if (x0 < 0) {
            x0 = 0;
         }

         if (y0 < 0) {
            y0 = 0;
         }

         if (z0 < 0) {
            z0 = 0;
         }

         if (x1 > this.width) {
            x1 = this.width;
         }

         if (y1 > this.depth) {
            y1 = this.depth;
         }

         if (z1 > this.height) {
            z1 = this.height;
         }

         for(int x = x0; x < x1; ++x) {
            for(int y = y0; y < y1; ++y) {
               for(int z = z0; z < z1; ++z) {
                  Tile tile = Tile.tiles[this.getTile(x, y, z)];
                  if (tile != null && tile.getLiquidType() > 0) {
                     return true;
                  }
               }
            }
         }

         return false;
      }

      public bool containsLiquid(AABB box, int liquidId) {
         int x0 = (int)Math.Floor((double)box.x0);
         int x1 = (int)Math.Floor((double)(box.x1 + 1.0F));
         int y0 = (int)Math.Floor((double)box.y0);
         int y1 = (int)Math.Floor((double)(box.y1 + 1.0F));
         int z0 = (int)Math.Floor((double)box.z0);
         int z1 = (int)Math.Floor((double)(box.z1 + 1.0F));
         if (x0 < 0) {
            x0 = 0;
         }

         if (y0 < 0) {
            y0 = 0;
         }

         if (z0 < 0) {
            z0 = 0;
         }

         if (x1 > this.width) {
            x1 = this.width;
         }

         if (y1 > this.depth) {
            y1 = this.depth;
         }

         if (z1 > this.height) {
            z1 = this.height;
         }

         for(int x = x0; x < x1; ++x) {
            for(int y = y0; y < y1; ++y) {
               for(int z = z0; z < z1; ++z) {
                  Tile tile = Tile.tiles[this.getTile(x, y, z)];
                  if (tile != null && tile.getLiquidType() == liquidId) {
                     return true;
                  }
               }
            }
         }

         return false;
      }
   }

}
