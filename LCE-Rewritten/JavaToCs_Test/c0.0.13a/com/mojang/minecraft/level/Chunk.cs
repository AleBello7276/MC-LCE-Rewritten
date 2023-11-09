using com.mojang.minecraft;
using com.mojang.minecraft.level.tile;
using com.mojang.minecraft.phys;
using com.mojang.minecraft.renderer;
using OpenTK.Graphics.OpenGL;

namespace com.mojang.minecraft.level
{
   public class Chunk {
      public AABB aabb;
      public readonly  Level level;
      public readonly int x0;
      public readonly int y0;
      public readonly int z0;
      public readonly int x1;
      public readonly int y1;
      public readonly int z1;
      public readonly float x;
      public readonly float y;
      public readonly float z;
      private bool dirty = true;
      private int lists = -1;
      public long dirtiedTime = 0L;
      public bool visible;
      private static Tesselator t;
      public static int updates;
      private static long totalTime;
      private static int totalUpdates;

      static Chunk()
      {
         t = Tesselator.instance;
         updates = 0;
         totalTime = 0L;
         totalUpdates = 0;
      }

      public Chunk(Level level, int x0, int y0, int z0, int x1, int y1, int z1) {
         this.level = level;
         this.x0 = x0;
         this.y0 = y0;
         this.z0 = z0;
         this.x1 = x1;
         this.y1 = y1;
         this.z1 = z1;
         this.x = (float)(x0 + x1) / 2.0F;
         this.y = (float)(y0 + y1) / 2.0F;
         this.z = (float)(z0 + z1) / 2.0F;
         this.aabb = new AABB((float)x0, (float)y0, (float)z0, (float)x1, (float)y1, (float)z1);
         this.lists = GL.GenLists(3);
      }

      private void rebuild(int layer) {
         long before = DateTime.Now.Ticks * 100;
         GL.NewList(this.lists + layer, ListMode.Compile);
         t.begin();
         int tiles = 0;

         for(int x = this.x0; x < this.x1; ++x) {
            for(int y = this.y0; y < this.y1; ++y) {
               for(int z = this.z0; z < this.z1; ++z) {
                  int tileId = this.level.getTile(x, y, z);
                  if (tileId > 0) {
                            
                            Tile.tiles[tileId].render(t, this.level, layer, x, y, z);
                     ++tiles;
                  }
               }
            }
         }

         t.end();
         GL.EndList();
         long after = DateTime.Now.Ticks * 100;
         if (tiles > 0) {
            totalTime += after - before;
            ++totalUpdates;
         }

      }

      public void rebuild() {
         ++updates;
         this.rebuild(0);
         this.rebuild(1);
         this.rebuild(2);
         this.dirty = false;
      }

      public void render(int layer) {
         GL.CallList(this.lists + layer);
      }

      public void setDirty() {
         if (!this.dirty) {
            this.dirtiedTime = (DateTime.Now.Ticks * 100) / TimeSpan.TicksPerMillisecond;
         }

         this.dirty = true;
      }

      public bool isDirty() {
         return this.dirty;
      }

      public float distanceToSqr(Player player) {
         float xd = player.x - this.x;
         float yd = player.y - this.y;
         float zd = player.z - this.z;
         return xd * xd + yd * yd + zd * zd;
      }

      public void reset() {
         this.dirty = true;

         for(int i = 0; i < 3; ++i) {
            GL.NewList(this.lists + i, ListMode.Compile);
            GL.EndList();
         }

      }
   }

}

