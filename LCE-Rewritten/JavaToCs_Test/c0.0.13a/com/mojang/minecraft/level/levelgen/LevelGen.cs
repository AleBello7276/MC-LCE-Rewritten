

using com.mojang.minecraft.level;
using com.mojang.minecraft.level;
using com.mojang.minecraft.level.tile;


namespace com.mojang.minecraft.level.levelgen
{
   public class LevelGen {
      private LevelLoaderListener levelLoaderListener;
      private int width;
      private int height;
      private int depth;
      private Random random = new Random();
      private byte[] blocks;
      private int[] coords = new int[1048576];

      public LevelGen(LevelLoaderListener levelLoaderListener) {
         this.levelLoaderListener = levelLoaderListener;
      }

      public bool generateLevel(Level level, String userName, int width, int height, int depth) {
         this.levelLoaderListener.beginLevelLoading("Generating level");
         this.width = width;
         this.height = height;
         this.depth = depth;
         this.blocks = new byte[width * height * depth];
         this.levelLoaderListener.levelLoadUpdate("Raising..");
         double[] heightMap = this.buildHeightmap(width, height);
         this.levelLoaderListener.levelLoadUpdate("Eroding..");
         this.buildBlocks(heightMap);
         this.levelLoaderListener.levelLoadUpdate("Carving..");
         this.carveTunnels();
         this.levelLoaderListener.levelLoadUpdate("Watering..");
         this.addWater();
         this.levelLoaderListener.levelLoadUpdate("Melting..");
         this.addLava();
         level.setData(width, depth, height, this.blocks);
         level.createTime = (DateTime.Now.Ticks * 100) / TimeSpan.TicksPerMillisecond;
         level.creator = userName;
         level.name = "A Nice World";
         return true;
      }

      private void buildBlocks(double[] heightMap) {
         int w = this.width;
         int h = this.height;
         int d = this.depth;

         for(int x = 0; x < w; ++x) {
            for(int y = 0; y < d; ++y) {
               for(int z = 0; z < h; ++z) {
                  int dh = d / 2;
                  int rh = d / 3;
                  int i = (y * this.height + z) * this.width + x;
                  int id = 0;
                  if (y == dh && y >= d / 2 - 1) {
                     id = Tile.grass.id;
                  } else if (y <= dh) {
                     id = Tile.dirt.id;
                  }

                  if (y <= rh) {
                     id = Tile.rock.id;
                  }

                  this.blocks[i] = (byte)id;
               }
            }
         }

      }

      private double[] buildHeightmap(int width, int height) {
         double[] heightmap = new double[width * height];
         return heightmap;
      }

      public void carveTunnels() {
         int w = this.width;
         int h = this.height;
         int d = this.depth;
         int count = w * h * d / 256 / 64;

         for(int i = 0; i < count; ++i) {
            float x = this.random.NextSingle() * (float)w;
            float y = this.random.NextSingle() * (float)d;
            float z = this.random.NextSingle() * (float)h;
            int length = (int)(this.random.NextSingle() + this.random.NextSingle() * 150.0F);
            float dir1 = (float)((double)this.random.NextSingle() * 3.141592653589793D * 2.0D);
            float dira1 = 0.0F;
            float dir2 = (float)((double)this.random.NextSingle() * 3.141592653589793D * 2.0D);
            float dira2 = 0.0F;

            for(int l = 0; l < length; ++l) {
               x = (float)((double)x + Math.Sin((double)dir1) * Math.Cos((double)dir2));
               z = (float)((double)z + Math.Cos((double)dir1) * Math.Cos((double)dir2));
               y = (float)((double)y + Math.Sin((double)dir2));
               dir1 += dira1 * 0.2F;
               dira1 *= 0.9F;
               dira1 += this.random.NextSingle() - this.random.NextSingle();
               dir2 += dira2 * 0.5F;
               dir2 *= 0.5F;
               dira2 *= 0.9F;
               dira2 += this.random.NextSingle() - this.random.NextSingle();
               float size = (float)(Math.Sin((double)l * 3.141592653589793D / (double)length) * 2.5D + 1.0D);

               for(int xx = (int)(x - size); xx <= (int)(x + size); ++xx) {
                  for(int yy = (int)(y - size); yy <= (int)(y + size); ++yy) {
                     for(int zz = (int)(z - size); zz <= (int)(z + size); ++zz) {
                        float xd = (float)xx - x;
                        float yd = (float)yy - y;
                        float zd = (float)zz - z;
                        float dd = xd * xd + yd * yd * 2.0F + zd * zd;
                        if (dd < size * size && xx >= 1 && yy >= 1 && zz >= 1 && xx < this.width - 1 && yy < this.depth - 1 && zz < this.height - 1) {
                           int ii = (yy * this.height + zz) * this.width + xx;
                           if (this.blocks[ii] == Tile.rock.id) {
                              this.blocks[ii] = 0;
                           }
                        }
                     }
                  }
               }
            }
         }

      }

      public void addWater() {
         long before = DateTime.Now.Ticks * 100;
         long tiles = 0L;
         int source = 0;
         int target = Tile.calmWater.id;

         int i;
         for(i = 0; i < this.width; ++i) {
            tiles += this.floodFillLiquid(i, this.depth / 2 - 1, 0, source, target);
            tiles += this.floodFillLiquid(i, this.depth / 2 - 1, this.height - 1, source, target);
         }

         for(i = 0; i < this.height; ++i) {
            tiles += this.floodFillLiquid(0, this.depth / 2 - 1, i, source, target);
            tiles += this.floodFillLiquid(this.width - 1, this.depth / 2 - 1, i, source, target);
         }

         for(i = 0; i < this.width * this.height / 5000; ++i) {
            int x = this.random.Next(this.width);
            int y = this.depth / 2 - 1;
            int z = this.random.Next(this.height);
            if (this.blocks[(y * this.height + z) * this.width + x] == 0) {
               tiles += this.floodFillLiquid(x, y, z, 0, target);
            }
         }

         long after = DateTime.Now.Ticks * 100;
        Console.WriteLine("Flood filled " + tiles + " tiles in " + (double)(after - before) / 1000000.0D + " ms");
      }

      public void addLava() {
         int lavaCount = 0;

         for(int i = 0; i < this.width * this.height * this.depth / 10000; ++i) {
            int x = this.random.Next(this.width);
            int y = this.random.Next(this.depth / 2);
            int z = this.random.Next(this.height);
            if (this.blocks[(y * this.height + z) * this.width + x] == 0) {
               ++lavaCount;
               this.floodFillLiquid(x, y, z, 0, Tile.calmLava.id);
            }
         }

         Console.WriteLine("LavaCount: " + lavaCount);
      }

      public long floodFillLiquid(int x, int y, int z, int source, int tt) {
         byte target = (byte)tt;
         List<int[]> coordBuffer = new List<int[]>();
         int p = 0;
         int wBits = 1;

         int hBits;
         for(hBits = 1; 1 << wBits < this.width; ++wBits) {
         }

         while(1 << hBits < this.height) {
            ++hBits;
         }

         int hMask = this.height - 1;
         int wMask = this.width - 1;
         int p1 = p + 1;
         this.coords[p] = ((y << hBits) + z << wBits) + x;
         long tiles = 0L;
         int upStep = this.width * this.height;

         while(p > 0) {
            --p;
            int cl = this.coords[p];
            if (p == 0 && coordBuffer.Count > 0)
            {
               Console.WriteLine("IT HAPPENED!");
               this.coords = (int[])coordBuffer[0]; 
               coordBuffer.RemoveAt(0);
               p = this.coords.Length;
            }

            int z0 = cl >> wBits & hMask;
            int y0 = cl >> wBits + hBits;
            int x0 = cl & wMask;

            int x1;
            for(x1 = x0; x0 > 0 && this.blocks[cl - 1] == source; --cl) {
               --x0;
            }

            while(x1 < this.width && this.blocks[cl + x1 - x0] == source) {
               ++x1;
            }

            int z1 = cl >> wBits & hMask;
            int y1 = cl >> wBits + hBits;
            if (z1 != z0 || y1 != y0) {
              Console.WriteLine("hoooly fuck");
            }

            bool lastNorth = false;
            bool lastSouth = false;
            bool lastBelow = false;
            tiles += (long)(x1 - x0);

            for(int xx = x0; xx < x1; ++xx) {
               this.blocks[cl] = target;
               bool south;
               if (z0 > 0) {
                  south = this.blocks[cl - this.width] == source;
                  if (south && !lastNorth) {
                     if (p == this.coords.Length) {
                        coordBuffer.Add(this.coords);
                        this.coords = new int[1048576];
                        p = 0;
                     }

                     this.coords[p++] = cl - this.width;
                  }

                  lastNorth = south;
               }

               if (z0 < this.height - 1) {
                  south = this.blocks[cl + this.width] == source;
                  if (south && !lastSouth) {
                     if (p == this.coords.Length) {
                        coordBuffer.Add(this.coords);
                        this.coords = new int[1048576];
                        p = 0;
                     }

                     this.coords[p++] = cl + this.width;
                  }

                  lastSouth = south;
               }

               if (y0 > 0) {
                  int belowId = this.blocks[cl - upStep];
                  if ((target == Tile.lava.id || target == Tile.calmLava.id) && (belowId == Tile.water.id || belowId == Tile.calmWater.id)) {
                     this.blocks[cl - upStep] = (byte)Tile.rock.id;
                  }

                  bool below = belowId == source;
                  if (below && !lastBelow) {
                     if (p == this.coords.Length) {
                        coordBuffer.Add(this.coords);
                        this.coords = new int[1048576];
                        p = 0;
                     }

                     this.coords[p++] = cl - upStep;
                  }

                  lastBelow = below;
               }

               ++cl;
            }
         }

         return tiles;
      }
   }

}
