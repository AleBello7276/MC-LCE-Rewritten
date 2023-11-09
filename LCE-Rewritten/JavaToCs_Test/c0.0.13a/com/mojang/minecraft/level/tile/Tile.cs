

using com.mojang.minecraft;
using com.mojang.minecraft.level;
using com.mojang.minecraft.particle;
using com.mojang.minecraft.phys;
using com.mojang.minecraft.renderer;
using System.Data.Common;
using System.Runtime.CompilerServices;


namespace com.mojang.minecraft.level.tile
{
    public class Tile
    {
        public static readonly int NOT_LIQUID = 0;
        public static readonly int LIQUID_WATER = 1;
        public static readonly int LIQUID_LAVA = 2;
        public static readonly Tile[] tiles = new Tile[256];
        public static readonly bool[] shouldTick = new bool[256];
        public static readonly Tile empty = null;
        public static readonly Tile rock = new Tile(1, 1);
        public static readonly Tile grass = new GrassTile(2);
        public static readonly Tile dirt = new DirtTile(3, 2);
        public static readonly Tile stoneBrick = new Tile(4, 16);
        public static readonly Tile wood = new Tile(5, 4);
        public static readonly Tile bush = new Bush(6);
        public static readonly Tile unbreakable = new Tile(21, 17);
        public static readonly Tile water = new LiquidTile(8, 1);
        public static readonly Tile calmWater = new CalmLiquidTile(9, 1);
        public static readonly Tile lava = new LiquidTile(10, 2);
        public static readonly Tile calmLava = new CalmLiquidTile(11, 2);
        public static readonly Tile RedstoneDust = new RedstoneTile(11);
        public static readonly Tile RedstoneBlock = new RedstoneBlock(7);
        public static readonly Tile TestButton = new Tile(55, 7);
        public static readonly Tile SlabLegno = new SlabTile(100, 4);
        public static readonly Tile ScalaLegno = new StairTile(101, 4);
        public static readonly Tile RetroTop = new SkinTile(9, 5);
        public static readonly Tile RetroBottom = new SkinTile(44, 6);


        public int tex;
        public int id;
        protected float xx0;
        protected float yy0;
        protected float zz0;
        protected float xx1;
        protected float yy1;
        protected float zz1;

        protected Tile(int id)
        {
            tiles[id] = this;
            this.id = id;
            this.setShape(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
        }

        protected virtual void setTicking(bool tick)
        {
            shouldTick[this.id] = tick;
        }

        protected virtual void setShape(float x0, float y0, float z0, float x1, float y1, float z1)
        {
            this.xx0 = x0;
            this.yy0 = y0;
            this.zz0 = z0;
            this.xx1 = x1;
            this.yy1 = y1;
            this.zz1 = z1;
        }

        protected virtual void setShapeInverse(float x0, float y0, float z0, float x1, float y1, float z1)
        {
            this.xx0 = x0;
            this.yy0 = y0;
            this.zz0 = z0;
            this.xx1 = x1;
            this.yy1 = y1;
            this.zz1 = z1;
        }

        protected Tile(int id, int texx) : this(id)
        {
            tex = texx;
        }

        public virtual void render(Tesselator t, Level level, int layer, int x, int y, int z)
        {
            sbyte c1 = -10;
            sbyte c2 = -70;
            sbyte c3 = -128;
            if (this.shouldRenderFace(level, x, y - 1, z, layer, 0))
            {
                t.color(c1, c1, c1);
                this.renderFace(t, x, y, z, 0);
            }

            if (this.shouldRenderFace(level, x, y + 1, z, layer, 1))
            {
                t.color(c1, c1, c1);
                this.renderFace(t, x, y, z, 1);
            }

            if (this.shouldRenderFace(level, x, y, z - 1, layer, 2))
            {
                t.color(c2, c2, c2);
                this.renderFace(t, x, y, z, 2);
            }

            if (this.shouldRenderFace(level, x, y, z + 1, layer, 3))
            {
                t.color(c2, c2, c2);
                this.renderFace(t, x, y, z, 3);
            }

            if (this.shouldRenderFace(level, x - 1, y, z, layer, 4))
            {
                t.color(c3, c3, c3);
                this.renderFace(t, x, y, z, 4);
            }

            if (this.shouldRenderFace(level, x + 1, y, z, layer, 5))
            {
                t.color(c3, c3, c3);
                this.renderFace(t, x, y, z, 5);
            }

        }

        protected virtual bool shouldRenderFace(Level level, int x, int y, int z, int layer, int face)
        {
            bool layerOk = true;
            if (layer == 2)
            {
                return false;
            }
            else
            {
                if (layer >= 0)
                {
                    layerOk = level.isLit(x, y, z) ^ layer == 1;
                }

                return !level.isSolidTile(x, y, z) && layerOk;
            }
        }

        protected virtual int getTexture(int face)
        {
            return this.tex;
        }



        public virtual void renderFace(Tesselator t, int x, int y, int z, int face)
        {
            int tex = this.getTexture(face); 
            int col = tex % 16; 
            int row = tex / 16; 

            float u0 = (float)col * 16 / 256;
            float u1 = (float)(col + 1) * 16 / 256;
            float v0 = 1f - (float)(row + 1) * 16 / 256;
            float v1 = 1f - (float)row * 16 / 256;
            float x0 = (float)x + this.xx0;
            float x1 = (float)x + this.xx1;
            float y0 = (float)y + this.yy0;
            float y1 = (float)y + this.yy1;
            float z0 = (float)z + this.zz0;
            float z1 = (float)z + this.zz1;
            if (face == 0)
            {
                t.vertexUV(x0, y0, z1, u0, v1);
                t.vertexUV(x0, y0, z0, u0, v0);
                t.vertexUV(x1, y0, z0, u1, v0);
                t.vertexUV(x1, y0, z1, u1, v1);
            }
            else if (face == 1)
            {
                t.vertexUV(x1, y1, z1, u1, v1);
                t.vertexUV(x1, y1, z0, u1, v0);
                t.vertexUV(x0, y1, z0, u0, v0);
                t.vertexUV(x0, y1, z1, u0, v1);
            }
            else if (face == 2)
            {
                t.vertexUV(x0, y1, z0, u1, v0);
                t.vertexUV(x1, y1, z0, u0, v0);
                t.vertexUV(x1, y0, z0, u0, v1);
                t.vertexUV(x0, y0, z0, u1, v1);
            }
            else if (face == 3)
            {
                t.vertexUV(x0, y1, z1, u0, v0);
                t.vertexUV(x0, y0, z1, u0, v1);
                t.vertexUV(x1, y0, z1, u1, v1);
                t.vertexUV(x1, y1, z1, u1, v0);
            }
            else if (face == 4)
            {
                t.vertexUV(x0, y1, z1, u1, v0);
                t.vertexUV(x0, y1, z0, u0, v0);
                t.vertexUV(x0, y0, z0, u0, v1);
                t.vertexUV(x0, y0, z1, u1, v1);
            }
            else if (face == 5)
            {
                t.vertexUV(x1, y0, z1, u0, v1);
                t.vertexUV(x1, y0, z0, u1, v1);
                t.vertexUV(x1, y1, z0, u1, v0);
                t.vertexUV(x1, y1, z1, u0, v0);
            }
        }

        public virtual void renderBackFace(Tesselator t, int x, int y, int z, int face)
        {
            int tex = this.getTexture(face);
            float u0 = (float)(tex % 16) / 16.0F;
            float u1 = u0 + 0.0624375F;
            float v0 = (float)(tex / 16) / 16.0F;
            float v1 = v0 + 0.0624375F;
            float x0 = (float)x + this.xx0;
            float x1 = (float)x + this.xx1;
            float y0 = (float)y + this.yy0;
            float y1 = (float)y + this.yy1;
            float z0 = (float)z + this.zz0;
            float z1 = (float)z + this.zz1;
            if (face == 0)
            {
                t.vertexUV(x1, y0, z1, u1, v1);
                t.vertexUV(x1, y0, z0, u1, v0);
                t.vertexUV(x0, y0, z0, u0, v0);
                t.vertexUV(x0, y0, z1, u0, v1);
            }

            if (face == 1)
            {
                t.vertexUV(x0, y1, z1, u0, v1);
                t.vertexUV(x0, y1, z0, u0, v0);
                t.vertexUV(x1, y1, z0, u1, v0);
                t.vertexUV(x1, y1, z1, u1, v1);
            }

            if (face == 2)
            {
                t.vertexUV(x0, y0, z0, u1, v1);
                t.vertexUV(x1, y0, z0, u0, v1);
                t.vertexUV(x1, y1, z0, u0, v0);
                t.vertexUV(x0, y1, z0, u1, v0);
            }

            if (face == 3)
            {
                t.vertexUV(x1, y1, z1, u1, v0);
                t.vertexUV(x1, y0, z1, u1, v1);
                t.vertexUV(x0, y0, z1, u0, v1);
                t.vertexUV(x0, y1, z1, u0, v0);
            }

            if (face == 4)
            {
                t.vertexUV(x0, y0, z1, u1, v1);
                t.vertexUV(x0, y0, z0, u0, v1);
                t.vertexUV(x0, y1, z0, u0, v0);
                t.vertexUV(x0, y1, z1, u1, v0);
            }

            if (face == 5)
            {
                t.vertexUV(x1, y1, z1, u0, v0);
                t.vertexUV(x1, y1, z0, u1, v0);
                t.vertexUV(x1, y0, z0, u1, v1);
                t.vertexUV(x1, y0, z1, u0, v1);
            }

        }

        public virtual void renderFaceNoTexture(Player player, Tesselator t, int x, int y, int z, int face)
        {
            float x0 = (float)x + 0.0F;
            float x1 = (float)x + 1.0F;
            float y0 = (float)y + 0.0F;
            float y1 = (float)y + 1.0F;
            float z0 = (float)z + 0.0F;
            float z1 = (float)z + 1.0F;
            if (face == 0 && (float)y > player.y)
            {
                t.vertex(x0, y0, z1);
                t.vertex(x0, y0, z0);
                t.vertex(x1, y0, z0);
                t.vertex(x1, y0, z1);
            }

            if (face == 1 && (float)y < player.y)
            {
                t.vertex(x1, y1, z1);
                t.vertex(x1, y1, z0);
                t.vertex(x0, y1, z0);
                t.vertex(x0, y1, z1);
            }

            if (face == 2 && (float)z > player.z)
            {
                t.vertex(x0, y1, z0);
                t.vertex(x1, y1, z0);
                t.vertex(x1, y0, z0);
                t.vertex(x0, y0, z0);
            }

            if (face == 3 && (float)z < player.z)
            {
                t.vertex(x0, y1, z1);
                t.vertex(x0, y0, z1);
                t.vertex(x1, y0, z1);
                t.vertex(x1, y1, z1);
            }

            if (face == 4 && (float)x > player.x)
            {
                t.vertex(x0, y1, z1);
                t.vertex(x0, y1, z0);
                t.vertex(x0, y0, z0);
                t.vertex(x0, y0, z1);
            }

            if (face == 5 && (float)x < player.x)
            {
                t.vertex(x1, y0, z1);
                t.vertex(x1, y0, z0);
                t.vertex(x1, y1, z0);
                t.vertex(x1, y1, z1);
            }

        }

        public virtual AABB getTileAABB(int x, int y, int z)
        {
            return new AABB((float)x, (float)y, (float)z, (float)(x + 1), (float)(y + 1), (float)(z + 1));
        }

        public virtual AABB getAABB(int x, int y, int z)
        {
            return new AABB((float)x, (float)y, (float)z, (float)(x + 1), (float)(y + 1), (float)(z + 1));
        }

        public virtual AABB getTileAABB2(int x, int y, int z)
        {
            return new AABB((float)x, (float)y, (float)z, (float)(x + 1), (float)(y + 1), (float)(z + 1));
        }

        public virtual AABB getAABB2(int x, int y, int z)
        {
            return new AABB((float)x, (float)y, (float)z, (float)(x + 1), (float)(y + 1), (float)(z + 1));
        }

        public virtual bool blocksLight()
        {
            return true;
        }

        public virtual bool isSolid()
        {
            return true;
        }

        public virtual bool mayPick()
        {
            return true;
        }

        //Mod
        public virtual bool isPowerSource()
        {
            return false;
        }

        public virtual void tick(Level level, int x, int y, int z, Random random)
        {
        }

        public virtual void destroy(Level level, int x, int y, int z, ParticleEngine particleEngine)
        {
            int SD = 4;

            for (int xx = 0; xx < SD; ++xx)
            {
                for (int yy = 0; yy < SD; ++yy)
                {
                    for (int zz = 0; zz < SD; ++zz)
                    {
                        float xp = (float)x + ((float)xx + 0.5F) / (float)SD;
                        float yp = (float)y + ((float)yy + 0.5F) / (float)SD;
                        float zp = (float)z + ((float)zz + 0.5F) / (float)SD;
                        particleEngine.add(new Particle(level, xp, yp, zp, xp - (float)x - 0.5F, yp - (float)y - 0.5F, zp - (float)z - 0.5F, this.tex));
                    }
                }
            }

        }

        public virtual int getLiquidType()
        {
            return 0;
        }

        public virtual void neighborChanged(Level level, int x, int y, int z, int type)
        {
        }
    }

}

