using OpenTK.Graphics.OpenGL;

namespace com.mojang.minecraft.character
{
    public class Cube
    {
        private Vertex[]? vertices;
        private Polygon[]? polygons;
        private int xTexOffs;
        private int yTexOffs;
        public float x;
        public float y;
        public float z;
        public float xRot;
        public float yRot;
        public float zRot;
        private bool compiled = false;
        private int list = 0;

        public Cube(int xTexOffs, int yTexOffs)
        {
            this.xTexOffs = xTexOffs;
            this.yTexOffs = yTexOffs;
        }

        public void setTexOffs(int xTexOffs, int yTexOffs)
        {
            this.xTexOffs = xTexOffs;
            this.yTexOffs = yTexOffs;
        }

        public void addBox(float x0, float y0, float z0, int w, int h, int d)
        {
            this.vertices = new Vertex[8];
            this.polygons = new Polygon[6];
            float x1 = x0 + (float)w;
            float y1 = y0 + (float)h;
            float z1 = z0 + (float)d;
            Vertex u0 = new Vertex(x0, y0, z0, 0.0F, 0.0F);
            Vertex u1 = new Vertex(x1, y0, z0, 0.0F, 8.0F);
            Vertex u2 = new Vertex(x1, y1, z0, 8.0F, 8.0F);
            Vertex u3 = new Vertex(x0, y1, z0, 8.0F, 0.0F);
            Vertex l0 = new Vertex(x0, y0, z1, 0.0F, 0.0F);
            Vertex l1 = new Vertex(x1, y0, z1, 0.0F, 8.0F);
            Vertex l2 = new Vertex(x1, y1, z1, 8.0F, 8.0F);
            Vertex l3 = new Vertex(x0, y1, z1, 8.0F, 0.0F);
            this.vertices[0] = u0;
            this.vertices[1] = u1;
            this.vertices[2] = u2;
            this.vertices[3] = u3;
            this.vertices[4] = l0;
            this.vertices[5] = l1;
            this.vertices[6] = l2;
            this.vertices[7] = l3;
            this.polygons[0] = new Polygon(new Vertex[] { l1, u1, u2, l2 }, this.xTexOffs + d + w, this.yTexOffs + d, this.xTexOffs + d + w + d, this.yTexOffs + d + h);
            this.polygons[1] = new Polygon(new Vertex[] { u0, l0, l3, u3 }, this.xTexOffs + 0, this.yTexOffs + d, this.xTexOffs + d, this.yTexOffs + d + h);
            this.polygons[2] = new Polygon(new Vertex[] { l1, l0, u0, u1 }, this.xTexOffs + d, this.yTexOffs + 0, this.xTexOffs + d + w, this.yTexOffs + d);
            this.polygons[3] = new Polygon(new Vertex[] { u2, u3, l3, l2 }, this.xTexOffs + d + w, this.yTexOffs + 0, this.xTexOffs + d + w + w, this.yTexOffs + d);
            this.polygons[4] = new Polygon(new Vertex[] { u1, u0, u3, u2 }, this.xTexOffs + d, this.yTexOffs + d, this.xTexOffs + d + w, this.yTexOffs + d + h);
            this.polygons[5] = new Polygon(new Vertex[] { l0, l1, l2, l3 }, this.xTexOffs + d + w + d, this.yTexOffs + d, this.xTexOffs + d + w + d + w, this.yTexOffs + d + h);
        }

        public void setPos(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void render()
        {
            if (!this.compiled)
            {
                this.compile();
            }

            float c = 57.29578F;
            GL.PushMatrix();
            GL.Translate(this.x, this.y, this.z);
            GL.Rotate(this.zRot * c, 0.0F, 0.0F, 1.0F);
            GL.Rotate(this.yRot * c, 0.0F, 1.0F, 0.0F);
            GL.Rotate(this.xRot * c, 1.0F, 0.0F, 0.0F);
            GL.CallList(this.list);
            GL.PopMatrix();
        }

        private void compile()
        {
            this.list = GL.GenLists(1);
            GL.NewList(this.list, ListMode.Compile);
            GL.Begin(PrimitiveType.Quads);

            for (int i = 0; i < this.polygons.Length; ++i)
            {
                this.polygons[i].render();
            }

            GL.End();
            GL.EndList();
            this.compiled = true;
        }
    }

}
