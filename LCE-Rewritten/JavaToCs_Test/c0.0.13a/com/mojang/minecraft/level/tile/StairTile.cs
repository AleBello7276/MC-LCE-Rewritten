


using com.mojang.minecraft.character;
using com.mojang.minecraft.level;
using com.mojang.minecraft.phys;
using com.mojang.minecraft.renderer;
using com.mojang.minecraft;




namespace com.mojang.minecraft.level.tile
{
    public class StairTile : Tile
    {
        public StairTile(int id, int tex) : base(id, tex)
        {
            this.setTicking(true);
        }







        public override void tick(Level level, int x, int y, int z, Random random)
        {
            int below = level.getTile(x, y - 1, z);
            if (!level.isLit(x, y, z) || below == Tile.RedstoneBlock.id)
            {
                level.setTile(x, y, z, 0);
                Console.WriteLine("Energy block under\n");
            }
        }



        public override void render(Tesselator t, Level level, int layer, int x, int y, int z)
        {
            sbyte c1 = -1;
            sbyte c2 = -52;
            sbyte c3 = -103;
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

            
            if (this.shouldRenderFace(level, x, y - 1, z, layer, 6))
            {
                t.color(c1, c1, c1);
                this.renderFace(t, x, y, z, 6);
            }

            if (this.shouldRenderFace(level, x, y + 1, z, layer, 7))
            {
                t.color(c1, c1, c1);
                this.renderFace(t, x, y, z, 7);
            }

            if (this.shouldRenderFace(level, x, y, z - 1, layer, 8))
            {
                t.color(c2, c2, c2);
                this.renderFace(t, x, y, z, 8);
            }

            if (this.shouldRenderFace(level, x, y, z + 1, layer, 9))
            {
                t.color(c2, c2, c2);
                this.renderFace(t, x, y, z, 9);
            }

            if (this.shouldRenderFace(level, x - 1, y, z, layer, 10))
            {
                t.color(c3, c3, c3);
                this.renderFace(t, x, y, z, 10);
            }

            if (this.shouldRenderFace(level, x + 1, y, z, layer, 11))
            {
                t.color(c3, c3, c3);
                this.renderFace(t, x, y, z, 11);
            }

        }

        protected override bool shouldRenderFace(Level level, int x, int y, int z, int layer, int face)
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

        protected override int getTexture(int face)
        {
            return this.tex;
        }

        public override void renderFace(Tesselator t, int x, int y, int z, int face)
        {
            int tex = this.getTexture(face);
            int col = tex % 16; //16
            int row = tex / 16; //0

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
                t.vertexUV(x1, y + 0.5f, z + 0.5f, u1, v1);
                t.vertexUV(x1, y + 0.5f, z0, u1, v0);
                t.vertexUV(x0, y + 0.5f, z0, u0, v0);
                t.vertexUV(x0, y + 0.5f, z + 0.5f, u0, v1);
            }
            else if (face == 2)
            {
                t.vertexUV(x0, y + 0.5f, z0, u0, v1);
                t.vertexUV(x1, y + 0.5f, z0, u0, v0);
                t.vertexUV(x1, y0, z0, u1, v0);
                t.vertexUV(x0, y0, z0, u1, v1);
            }
            else if (face == 3)
            {
                t.vertexUV(x0, y0, z0, u1, v1);
                t.vertexUV(x0, y0, z0, u0, v1);
                t.vertexUV(x0, y0, z0, u0, v0);
                t.vertexUV(x0, y0, z0, u1, v0);
            }
            else if (face == 4)
            {
                t.vertexUV(x0, y + 0.5f, z + 0.5f, u0, v1);
                t.vertexUV(x0, y + 0.5f, z0, u0, v0);
                t.vertexUV(x0, y0, z0, u1, v0);
                t.vertexUV(x0, y0, z + 0.5f, u1, v1);
            }
            else if (face == 5)
            {
                t.vertexUV(x1, y0, z + 0.5f, u1, v0);
                t.vertexUV(x1, y0, z0, u1, v1);
                t.vertexUV(x1, y + 0.5f, z0, u0, v1);
                t.vertexUV(x1, y + 0.5f, z + 0.5f, u0, v0);
            }

            
            if (face == 6)
            {

            }
            else if (face == 7)
            {
                t.vertexUV(x1, y1, z1, u1, v1);
                t.vertexUV(x1, y1, z + 0.5f, u1, v0);
                t.vertexUV(x0, y1, z + 0.5f, u0, v0);
                t.vertexUV(x0, y1, z1, u0, v1);
            }
            else if (face == 8)
            {
                t.vertexUV(x0, y1, z + 0.5f, u1, v1);
                t.vertexUV(x1, y1, z + 0.5f, u0, v1);
                t.vertexUV(x1, y + 0.5f, z + 0.5f, u0, v0);
                t.vertexUV(x0, y + 0.5f, z + 0.5f, u1, v0);
            }
            else if (face == 9)
            {
                t.vertexUV(x0, y1, z1, u0, v0);
                t.vertexUV(x0, y0, z1, u0, v1);
                t.vertexUV(x1, y0, z1, u1, v1);
                t.vertexUV(x1, y1, z1, u1, v0);
            }
            else if (face == 10)
            {
                t.vertexUV(x1, y0, z1, u1, v1);
                t.vertexUV(x1, y0, z + 0.5f, u0, v1);
                t.vertexUV(x1, y1, z + 0.5f, u0, v0);
                t.vertexUV(x1, y1, z1, u1, v0);
            }
            else if (face == 11)
            {
                t.vertexUV(x0, y1, z1, u1, v0);
                t.vertexUV(x0, y1, z + 0.5f, u0, v0);
                t.vertexUV(x0, y0, z + 0.5f, u0, v0);
                t.vertexUV(x0, y0, z1, u0, v0);
            }
        }

        public override AABB getAABB(int x, int y, int z)
        {
            return new AABB((float)x, (float)y, (float)z, (float)(x + 0.5), (float)(y + 0.5), (float)(z + 0.5));
        }

        public override AABB getAABB2(int x, int y, int z)
        {
            return new AABB((float)x, (float)y, (float)z, (float)(x + 0.5), (float)(y + 0.5), (float)(z + 1));
        }


        public override bool blocksLight()
        {
            return false;
        }

        public override bool isSolid()
        {
            return false;
        }
    }




}
