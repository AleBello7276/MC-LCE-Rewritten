

using com.mojang.minecraft;
using com.mojang.minecraft.level.tile;
using com.mojang.minecraft.phys;
using com.mojang.minecraft.renderer;

using OpenTK.Graphics.OpenGL;

namespace com.mojang.minecraft.level
{
    public class LevelRenderer : LevelListener
    {
        public static readonly int MAX_REBUILDS_PER_FRAME = 4;
        public static readonly int CHUNK_SIZE = 16;
        private Level level;
        private Chunk[] chunks;
        private Chunk[] sortedChunks;
        private int xChunks;
        private int yChunks;
        private int zChunks;
        private Textures textures;
        private int surroundLists;
        private int drawDistance = 0;
        float lX = 0.0F;
        float lY = 0.0F;
        float lZ = 0.0F;

        //Textures ids - AleBello
        int terrId;
        int rockId;
        int wtrId;

        public LevelRenderer(Level level, Textures textures)
        {
            this.level = level;
            this.textures = textures;
            terrId = this.textures.loadTexture("res/terrain.png", 9728);
            rockId = this.textures.loadTexture("res/rock.png", 9728);
            wtrId = this.textures.loadTexture("res/water.png", 9728);
            level.addListener(this);
            this.surroundLists = GL.GenLists(2);
            this.allChanged();
        }

        public void allChanged()
        {
            this.lX = -900000.0F;
            this.lY = -900000.0F;
            this.lZ = -900000.0F;
            this.xChunks = (this.level.width + 16 - 1) / 16;
            this.yChunks = (this.level.depth + 16 - 1) / 16;
            this.zChunks = (this.level.height + 16 - 1) / 16;
            this.chunks = new Chunk[this.xChunks * this.yChunks * this.zChunks];
            this.sortedChunks = new Chunk[this.xChunks * this.yChunks * this.zChunks];

            int x;
            for (x = 0; x < this.xChunks; ++x)
            {
                for (int y = 0; y < this.yChunks; ++y)
                {
                    for (int z = 0; z < this.zChunks; ++z)
                    {
                        int x0 = x * 16;
                        int y0 = y * 16;
                        int z0 = z * 16;
                        int x1 = (x + 1) * 16;
                        int y1 = (y + 1) * 16;
                        int z1 = (z + 1) * 16;
                        if (x1 > this.level.width)
                        {
                            x1 = this.level.width;
                        }

                        if (y1 > this.level.depth)
                        {
                            y1 = this.level.depth;
                        }

                        if (z1 > this.level.height)
                        {
                            z1 = this.level.height;
                        }

                        this.chunks[(x + y * this.xChunks) * this.zChunks + z] = new Chunk(this.level, x0, y0, z0, x1, y1, z1);
                        this.sortedChunks[(x + y * this.xChunks) * this.zChunks + z] = this.chunks[(x + y * this.xChunks) * this.zChunks + z];
                    }
                }
            }

            GL.NewList(this.surroundLists + 0, ListMode.Compile);
            this.compileSurroundingGround();
            GL.EndList();
            GL.NewList(this.surroundLists + 1, ListMode.Compile);
            this.compileSurroundingWater();
            GL.EndList();

            for (x = 0; x < this.chunks.Length; ++x)
            {
                this.chunks[x].reset();
            }

        }

        public List<Chunk> getAllDirtyChunks()
        {
            List<Chunk> dirty = null;

            for (int i = 0; i < this.chunks.Length; ++i)
            {
                Chunk chunk = this.chunks[i];
                if (chunk.isDirty())
                {
                    if (dirty == null)
                    {
                        dirty = new List<Chunk>();
                    }

                    dirty.Add(chunk);
                }
            }

            return dirty;
        }

        public void render(Player player, int layer)
        {


            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, terrId);
            float xd = player.x - this.lX;
            float yd = player.y - this.lY;
            float zd = player.z - this.lZ;
            if (xd * xd + yd * yd + zd * zd > 64.0F)
            {
                this.lX = player.x;
                this.lY = player.y;
                this.lZ = player.z;
                Array.Sort(this.sortedChunks, new DistanceSorter(player));
            }

            for (int i = 0; i < this.sortedChunks.Length; ++i)
            {
                if (this.sortedChunks[i].visible)
                {
                    float dd = (float)(256 / (1 << this.drawDistance));
                    if (this.drawDistance == 0 || this.sortedChunks[i].distanceToSqr(player) < dd * dd)
                    {
                        this.sortedChunks[i].render(layer);
                    }
                }
            }

            GL.Disable(EnableCap.Texture2D);
        }

        public void renderSurroundingGround()
        {
            GL.CallList(this.surroundLists + 0);
        }

        public void compileSurroundingGround()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, rockId);
            GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
            Tesselator t = Tesselator.instance;
            float y = this.level.getGroundLevel() - 2.0F;
            int s = 128;
            if (s > this.level.width)
            {
                s = this.level.width;
            }

            if (s > this.level.height)
            {
                s = this.level.height;
            }

            int d = 5;
            t.begin();

            int zz;
            for (zz = -s * d; zz < this.level.width + s * d; zz += s)
            {
                for (int zz1 = -s * d; zz1 < this.level.height + s * d; zz1 += s)
                {
                    float yy = y;
                    if (zz1 >= 0 && zz1 >= 0 && zz1 < this.level.width && zz1 < this.level.height)
                    {
                        yy = 0.0F;
                    }

                    t.vertexUV((float)(zz1 + 0), yy, (float)(zz1 + s), 0.0F, (float)s);
                    t.vertexUV((float)(zz1 + s), yy, (float)(zz1 + s), (float)s, (float)s);
                    t.vertexUV((float)(zz1 + s), yy, (float)(zz1 + 0), (float)s, 0.0F);
                    t.vertexUV((float)(zz1 + 0), yy, (float)(zz1 + 0), 0.0F, 0.0F);
                }
            }

            t.end();
            GL.BindTexture(TextureTarget.Texture2D, rockId);
            GL.Color3(0.8F, 0.8F, 0.8F);
            t.begin();

            for (zz = 0; zz < this.level.width; zz += s)
            {
                t.vertexUV((float)(zz + 0), 0.0F, 0.0F, 0.0F, 0.0F);
                t.vertexUV((float)(zz + s), 0.0F, 0.0F, (float)s, 0.0F);
                t.vertexUV((float)(zz + s), y, 0.0F, (float)s, y);
                t.vertexUV((float)(zz + 0), y, 0.0F, 0.0F, y);
                t.vertexUV((float)(zz + 0), y, (float)this.level.height, 0.0F, y);
                t.vertexUV((float)(zz + s), y, (float)this.level.height, (float)s, y);
                t.vertexUV((float)(zz + s), 0.0F, (float)this.level.height, (float)s, 0.0F);
                t.vertexUV((float)(zz + 0), 0.0F, (float)this.level.height, 0.0F, 0.0F);
            }

            GL.Color3(0.6F, 0.6F, 0.6F);

            for (zz = 0; zz < this.level.height; zz += s)
            {
                t.vertexUV(0.0F, y, (float)(zz + 0), 0.0F, 0.0F);
                t.vertexUV(0.0F, y, (float)(zz + s), (float)s, 0.0F);
                t.vertexUV(0.0F, 0.0F, (float)(zz + s), (float)s, y);
                t.vertexUV(0.0F, 0.0F, (float)(zz + 0), 0.0F, y);
                t.vertexUV((float)this.level.width, 0.0F, (float)(zz + 0), 0.0F, y);
                t.vertexUV((float)this.level.width, 0.0F, (float)(zz + s), (float)s, y);
                t.vertexUV((float)this.level.width, y, (float)(zz + s), (float)s, 0.0F);
                t.vertexUV((float)this.level.width, y, (float)(zz + 0), 0.0F, 0.0F);
            }

            t.end();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }

        public void renderSurroundingWater()
        {
            GL.CallList(this.surroundLists + 1);
        }

        public void compileSurroundingWater()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Color3(1.0F, 1.0F, 1.0F);
            GL.BindTexture(TextureTarget.Texture2D, wtrId);
            float y = this.level.getGroundLevel();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            Tesselator t = Tesselator.instance;
            int s = 128;
            if (s > this.level.width)
            {
                s = this.level.width;
            }

            if (s > this.level.height)
            {
                s = this.level.height;
            }

            int d = 5;
            t.begin();

            for (int xx = -s * d; xx < this.level.width + s * d; xx += s)
            {
                for (int zz = -s * d; zz < this.level.height + s * d; zz += s)
                {
                    float yy = y - 0.1F;
                    if (xx < 0 || zz < 0 || xx >= this.level.width || zz >= this.level.height)
                    {
                        t.vertexUV((float)(xx + 0), yy, (float)(zz + s), 0.0F, (float)s);
                        t.vertexUV((float)(xx + s), yy, (float)(zz + s), (float)s, (float)s);
                        t.vertexUV((float)(xx + s), yy, (float)(zz + 0), (float)s, 0.0F);
                        t.vertexUV((float)(xx + 0), yy, (float)(zz + 0), 0.0F, 0.0F);
                        t.vertexUV((float)(xx + 0), yy, (float)(zz + 0), 0.0F, 0.0F);
                        t.vertexUV((float)(xx + s), yy, (float)(zz + 0), (float)s, 0.0F);
                        t.vertexUV((float)(xx + s), yy, (float)(zz + s), (float)s, (float)s);
                        t.vertexUV((float)(xx + 0), yy, (float)(zz + s), 0.0F, (float)s);
                    }
                }
            }

            t.end();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }



        public void UpdateDirtyChunks(Player player)
        {
            List<Chunk> dirty = this.getAllDirtyChunks();

            if (dirty != null)
            {
                dirty.Sort(new DirtyChunkSorter(player));

                for (int i = 0; i < 4 && i < dirty.Count; i++)
                {
                    dirty[i].rebuild();
                }
            }
        }

        public void pick(Player player, Frustum frustum)
        {
            Tesselator t = Tesselator.instance;
            float r = 2.5F;
            AABB box = player.bb.grow(r, r, r);
            int x0 = (int)box.x0;
            int x1 = (int)(box.x1 + 1.0F);
            int y0 = (int)box.y0;
            int y1 = (int)(box.y1 + 1.0F);
            int z0 = (int)box.z0;
            int z1 = (int)(box.z1 + 1.0F);
            GL.InitNames();
            GL.PushName(0);
            GL.PushName(0);

            for (int x = x0; x < x1; ++x)
            {
                GL.LoadName(x);
                GL.PushName(0);

                for (int y = y0; y < y1; ++y)
                {
                    GL.LoadName(y);
                    GL.PushName(0);

                    for (int z = z0; z < z1; ++z)
                    {
                        Tile tile = Tile.tiles[this.level.getTile(x, y, z)];
                        if (tile != null && tile.mayPick() && frustum.isVisible(tile.getTileAABB(x, y, z)))
                        {
                            GL.LoadName(z);
                            GL.PushName(0);

                            for (int i = 0; i < 6; ++i)
                            {
                                GL.LoadName(i);
                                t.begin();
                                tile.renderFaceNoTexture(player, t, x, y, z, i);
                                t.end();
                            }

                            GL.PopName();
                        }
                    }

                    GL.PopName();
                }

                GL.PopName();
            }

            GL.PopName();
            GL.PopName();
        }

        public void renderHit(Player player, HitResult h, int mode, int tileType)
        {
            Tesselator t = Tesselator.instance;
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.One);
            GL.Color4(1.0F, 1.0F, 1.0F, ((float)Math.Sin((double)((DateTime.Now.Ticks * 100) / TimeSpan.TicksPerMillisecond) / 100.0D) * 0.2F + 0.4F) * 0.5F);
            if (mode == 0)
            {
                t.begin();

                for (int i = 0; i < 6; ++i)
                {
                    Tile.rock.renderFaceNoTexture(player, t, h.x, h.y, h.z, i);
                }

                t.end();
            }
            else
            {
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                float br = (float)Math.Sin((double)((DateTime.Now.Ticks * 100) / TimeSpan.TicksPerMillisecond) / 100.0D) * 0.2F + 0.8F;
                GL.Color4(br, br, br, (float)Math.Sin((double)((DateTime.Now.Ticks * 100) / TimeSpan.TicksPerMillisecond) / 200.0D) * 0.2F + 0.5F);
                GL.Enable(EnableCap.Texture2D);

                GL.BindTexture(TextureTarget.Texture2D, terrId);
                int x = h.x;
                int y = h.y;
                int z = h.z;
                if (h.f == 0)
                {
                    --y;
                }

                if (h.f == 1)
                {
                    ++y;
                }

                if (h.f == 2)
                {
                    --z;
                }

                if (h.f == 3)
                {
                    ++z;
                }

                if (h.f == 4)
                {
                    --x;
                }

                if (h.f == 5)
                {
                    ++x;
                }

                t.begin();
                t.NoColor();
                Tile.tiles[tileType].render(t, this.level, 0, x, y, z);
                Tile.tiles[tileType].render(t, this.level, 1, x, y, z);
                t.end();
                GL.Disable(EnableCap.Texture2D);
            }

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.AlphaTest);
        }

        public void renderHitOutline(Player player, HitResult h, int mode, int tileType)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Color4(0.0F, 0.0F, 0.0F, 0.4F);
            float x = (float)h.x;
            float y = (float)h.y;
            float z = (float)h.z;
            if (mode == 1)
            {
                if (h.f == 0)
                {
                    --y;
                }

                if (h.f == 1)
                {
                    ++y;
                }

                if (h.f == 2)
                {
                    --z;
                }

                if (h.f == 3)
                {
                    ++z;
                }

                if (h.f == 4)
                {
                    --x;
                }

                if (h.f == 5)
                {
                    ++x;
                }
            }

            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(x, y, z);
            GL.Vertex3(x + 1.0F, y, z);
            GL.Vertex3(x + 1.0F, y, z + 1.0F);
            GL.Vertex3(x, y, z + 1.0F);
            GL.Vertex3(x, y, z);
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(x, y + 1.0F, z);
            GL.Vertex3(x + 1.0F, y + 1.0F, z);
            GL.Vertex3(x + 1.0F, y + 1.0F, z + 1.0F);
            GL.Vertex3(x, y + 1.0F, z + 1.0F);
            GL.Vertex3(x, y + 1.0F, z);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(x, y, z);
            GL.Vertex3(x, y + 1.0F, z);
            GL.Vertex3(x + 1.0F, y, z);
            GL.Vertex3(x + 1.0F, y + 1.0F, z);
            GL.Vertex3(x + 1.0F, y, z + 1.0F);
            GL.Vertex3(x + 1.0F, y + 1.0F, z + 1.0F);
            GL.Vertex3(x, y, z + 1.0F);
            GL.Vertex3(x, y + 1.0F, z + 1.0F);
            GL.End();
            GL.Disable(EnableCap.Blend);
        }

        public void setDirty(int x0, int y0, int z0, int x1, int y1, int z1)
        {
            x0 /= 16;
            x1 /= 16;
            y0 /= 16;
            y1 /= 16;
            z0 /= 16;
            z1 /= 16;
            if (x0 < 0)
            {
                x0 = 0;
            }

            if (y0 < 0)
            {
                y0 = 0;
            }

            if (z0 < 0)
            {
                z0 = 0;
            }

            if (x1 >= this.xChunks)
            {
                x1 = this.xChunks - 1;
            }

            if (y1 >= this.yChunks)
            {
                y1 = this.yChunks - 1;
            }

            if (z1 >= this.zChunks)
            {
                z1 = this.zChunks - 1;
            }

            for (int x = x0; x <= x1; ++x)
            {
                for (int y = y0; y <= y1; ++y)
                {
                    for (int z = z0; z <= z1; ++z)
                    {
                        this.chunks[(x + y * this.xChunks) * this.zChunks + z].setDirty();
                    }
                }
            }

        }

        public void tileChanged(int x, int y, int z)
        {
            this.setDirty(x - 1, y - 1, z - 1, x + 1, y + 1, z + 1);
        }

        public void lightColumnChanged(int x, int z, int y0, int y1)
        {
            this.setDirty(x - 1, y0 - 1, z - 1, x + 1, y1 + 1, z + 1);
        }

        public void toggleDrawDistance()
        {
            this.drawDistance = (this.drawDistance + 1) % 4;
        }

        public void cull(Frustum frustum)
        {



            for (int i = 0; i < this.chunks.Length; ++i)
            {
                this.chunks[i].visible = frustum.isVisible(this.chunks[i].aabb);
            }

        }


    }

}
