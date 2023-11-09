
using com.mojang.minecraft.renderer;




using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace com.mojang.minecraft.gui
{
   using System;
   using System.Drawing;
   using System.Drawing.Imaging;
   using OpenTK;
   using OpenTK.Graphics;
   using OpenTK.Graphics.OpenGL;

   public class Font
   {
      private int[] charWidths = new int[256];
      private int fontTexture = 0;

      public Font(string name, Textures textures)
      {
         Bitmap img;
         try
         {
               img = new Bitmap(name);
         }
         catch (Exception ex)
         {
               throw new ApplicationException(ex.Message);
         }

         int w = img.Width;
         int h = img.Height;
         int[] rawPixels = new int[w * h];

         // Extract raw pixel data from the Bitmap
         BitmapData bitmapData = img.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
         System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, rawPixels, 0, rawPixels.Length);
         img.UnlockBits(bitmapData);

         for (int i = 0; i < 128; i++)
         {
               int xt = i % 16;
               int yt = i / 16;
               int x = 0;

               for (bool emptyColumn = false; x < 8 && !emptyColumn; x++)
               {
                  int xPixel = xt * 8 + x;
                  emptyColumn = true;

                  for (int y = 0; y < 8 && emptyColumn; y++)
                  {
                     int yPixel = (yt * 8 + y) * w;
                     int pixel = (rawPixels[xPixel + yPixel] & 0xFF);
                     if (pixel > 128)
                     {
                           emptyColumn = false;
                     }
                  }
               }

               if (i == 32)
               {
                  x = 4;
               }

               charWidths[i] = x;
         }

         // Load the font texture using OpenTK
         fontTexture = textures.loadTexture(name, 9728);
      }

      public void drawShadow(string str, int x, int y, int color)
      {
         draw(str, x + 1, y + 1, color, true);
         draw(str, x, y, color);
      }

      public void draw(string str, int x, int y, int color)
      {
         draw(str, x, y, color, false);
      }

      public void draw(string str, int x, int y, int color, bool darken)
      {
         char[] chars = str.ToCharArray();
         if (darken)
         {
               color = (color & 16579836) >> 2;
         }

         GL.Enable(EnableCap.Texture2D);
         GL.BindTexture(TextureTarget.Texture2D, fontTexture);
         Tesselator t = Tesselator.instance;
         t.begin();
         t.color(color);
         int xo = 0;

         for (int i = 0; i < chars.Length; i++)
         {
               // Implement character rendering logic similar to Java code
               // ...
            int ix;
            int iy;

            if (chars[i] == '&') {
               ix = "0123456789abcdef".IndexOf(chars[i + 1]);
               iy = (ix & 8) * 8;
               int b = (ix & 1) * 191 + iy;
               int g = ((ix & 2) >> 1) * 191 + iy;
               int r = ((ix & 4) >> 2) * 191 + iy;
               color = r << 16 | g << 8 | b;
               i += 2;
               if (darken) {
                  color = (color & 16579836) >> 2;
               }

               t.color(color);
            }
            
            ix = chars[i] % 16 * 8;
            iy = 127 - ((chars[i] / 16 * 8) - 1);
            
            t.vertexUV((float)(x + xo), (float)(y + 8), 0.0F, (float)ix / 128.0F, (float)(iy - 8) / 128.0F);
            t.vertexUV((float)(x + xo + 8), (float)(y + 8), 0.0F, (float)(ix + 8) / 128.0F, (float)(iy - 8) / 128.0F);
            t.vertexUV((float)(x + xo + 8), (float)y, 0.0F, (float)(ix + 8) / 128.0F, (float)iy / 128.0F);
            t.vertexUV((float)(x + xo), (float)y, 0.0F, (float)ix / 128.0F, (float)iy / 128.0F); // 1
            xo += this.charWidths[chars[i]];            
         }
         t.end();
         GL.Disable(EnableCap.Texture2D);
      }

      public int width(string str)
      {
         char[] chars = str.ToCharArray();
         int len = 0;

         for (int i = 0; i < chars.Length; i++)
         {
               if (chars[i] == '&')
               {
                  i++;
               }
               else
               {
                  len += charWidths[chars[i]];
               }
         }

         return len;
      }
   }


}

