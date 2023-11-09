

using com.mojang.minecraft;
using com.mojang.minecraft.renderer;


using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace com.mojang.minecraft.gui
{
    public class Screen
    {

        protected Minecraft minecraft;
        protected int width;
        protected int height;


        public virtual void render(int xMouse, int yMouse)
        {
        }

        public void init(Minecraft minecraft, int width, int height)
        {
            this.minecraft = minecraft;
            this.width = width;
            this.height = height;
            this.init();
        }

        public virtual void init()
        {
        }

        protected void fill(int x0, int y0, int x1, int y1, int col)
        {
            float a = (float)(col >> 24 & 255) / 255.0F;
            float r = (float)(col >> 16 & 255) / 255.0F;
            float g = (float)(col >> 8 & 255) / 255.0F;
            float b = (float)(col & 255) / 255.0F;
            Tesselator t = Tesselator.instance;
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Color4(r, g, b, a);
            t.begin();
            t.vertex((float)x0, (float)y1, 0.0F);
            t.vertex((float)x1, (float)y1, 0.0F);
            t.vertex((float)x1, (float)y0, 0.0F);
            t.vertex((float)x0, (float)y0, 0.0F);
            t.end();
            GL.Disable(EnableCap.Blend);
        }

        protected void fillGradient(int x0, int y0, int x1, int y1, int col1, int col2)
        {
            float a1 = (float)(col1 >> 24 & 255) / 255.0F;
            float r1 = (float)(col1 >> 16 & 255) / 255.0F;
            float g1 = (float)(col1 >> 8 & 255) / 255.0F;
            float b1 = (float)(col1 & 255) / 255.0F;
            float a2 = (float)(col2 >> 24 & 255) / 255.0F;
            float r2 = (float)(col2 >> 16 & 255) / 255.0F;
            float g2 = (float)(col2 >> 8 & 255) / 255.0F;
            float b2 = (float)(col2 & 255) / 255.0F;
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(r1, g1, b1, a1);
            GL.Vertex2((float)x1, (float)y0);
            GL.Vertex2((float)x0, (float)y0);
            GL.Color4(r2, g2, b2, a2);
            GL.Vertex2((float)x0, (float)y1);
            GL.Vertex2((float)x1, (float)y1);
            GL.End();
            GL.Disable(EnableCap.Blend);
        }

        public void drawCenteredString(String str, int x, int y, int color)
        {
            Font font = this.minecraft.font;
            font.drawShadow(str, x - font.width(str) / 2, y, color);
        }

        public void drawString(String str, int x, int y, int color)
        {
            Font font = this.minecraft.font;
            font.drawShadow(str, x, y, color);
        }

        public void updateEvents()
        {
            if (minecraft.MouseState.IsButtonDown(MouseButton.Left))
            {
                //Console.WriteLine("Left");
                int xm = (int)(minecraft.MouseState.X * this.width / this.minecraft.width);
                int ym = 240 - (this.height - (int)minecraft.MouseState.Y * this.height / this.minecraft.height - 1);
                
                this.mouseClicked(xm, ym, 0);
            }
        }



        protected virtual void keyPressed(char eventCharacter, int eventKey)
        {
        }

        protected virtual void mouseClicked(int x, int y, int button)
        {
        }

        public void tick()
        {
        }
    }

}

