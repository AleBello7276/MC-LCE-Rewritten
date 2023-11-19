using OpenTK.Graphics.OpenGL;
using System.Drawing;

using net.minecraft.src.entity;
using net.minecraft.src.renderers;
namespace net.minecraft.src
{
	public class MapItemRenderer {
		private int[] pixels = new int[16384];
		private int field_28158_b;
		private GameSettings gameSettings;
		private FontRenderer fontRenderer;

		public MapItemRenderer(FontRenderer font, GameSettings settings, RenderEngine render) {
			this.gameSettings = settings;
			this.fontRenderer = font;
			this.field_28158_b = render.allocateAndSetupTexture(new Bitmap(128, 128));

			for (int var4 = 0; var4 < 16384; ++var4) {
				this.pixels[var4] = 0;
			}

		}

		public void func_28157_a(EntityPlayer var1, RenderEngine var2, MapData var3) {
			for (int var4 = 0; var4 < 16384; ++var4) {
				byte var5 = var3.field_28176_f[var4];
				if (var5 / 4 == 0) {
					this.pixels[var4] = (var4 + var4 / 128 & 1) * 8 + 16 << 24;
				} else {
					int var6 = MapColor.mapColorArray[var5 / 4].colorValue;
					int var7 = var5 & 3;
					short var8 = 220;
					if (var7 == 2) {
						var8 = 255;
					}

					if (var7 == 0) {
						var8 = 180;
					}

					int var9 = (var6 >> 16 & 255) * var8 / 255;
					int var10 = (var6 >> 8 & 255) * var8 / 255;
					int var11 = (var6 & 255) * var8 / 255;
					if (this.gameSettings.anaglyph) {
						int var12 = (var9 * 30 + var10 * 59 + var11 * 11) / 100;
						int var13 = (var9 * 30 + var10 * 70) / 100;
						int var14 = (var9 * 30 + var11 * 70) / 100;
						var9 = var12;
						var10 = var13;
						var11 = var14;
					}

					this.pixels[var4] = -16777216 | var9 << 16 | var10 << 8 | var11;
				}
			}

			var2.func_28150_a(this.pixels, 128, 128, this.field_28158_b);
			byte var15 = 0;
			byte var16 = 0;
			Tessellator tes = Tessellator.instance;
			float var18 = 0.0F;
			GL.BindTexture(TextureTarget.Texture2D, this.field_28158_b);
			GL.Enable(EnableCap.Blend);
			GL.Disable(EnableCap.AlphaTest);
			tes.startDrawingQuads();
			tes.addVertexWithUV((double) ((float) (var15 + 0) + var18), (double) ((float) (var16 + 128) - var18),
					(double) -0.01F, 0.0D, 1.0D);
			tes.addVertexWithUV((double) ((float) (var15 + 128) - var18), (double) ((float) (var16 + 128) - var18),
					(double) -0.01F, 1.0D, 1.0D);
			tes.addVertexWithUV((double) ((float) (var15 + 128) - var18), (double) ((float) (var16 + 0) + var18),
					(double) -0.01F, 1.0D, 0.0D);
			tes.addVertexWithUV((double) ((float) (var15 + 0) + var18), (double) ((float) (var16 + 0) + var18),
					(double) -0.01F, 0.0D, 0.0D);
			tes.draw();
			GL.Enable(EnableCap.AlphaTest);
			GL.Disable(EnableCap.Blend);
			var2.bindTexture(var2.getTexture("/misc/mapicons.png"));

			foreach (MapCoord var20 in var3.field_28173_i)
            {
                GL.PushMatrix();
                GL.Translate((float)var15 + (float)var20.field_28216_b / 2.0F + 64.0F,
                    (float)var16 + (float)var20.field_28220_c / 2.0F + 64.0F, -0.02F);
                GL.Rotate((float)(var20.field_28219_d * 360) / 16.0F, 0.0F, 0.0F, 1.0F);
                GL.Scale(4.0F, 4.0F, 3.0F);
                GL.Translate(-(2.0F / 16.0F), 2.0F / 16.0F, 0.0F);
                float var21 = (float)(var20.field_28217_a % 4 + 0) / 4.0F;
                float var22 = (float)(var20.field_28217_a / 4 + 0) / 4.0F;
                float var23 = (float)(var20.field_28217_a % 4 + 1) / 4.0F;
                float var24 = (float)(var20.field_28217_a / 4 + 1) / 4.0F;
                tes.StartDrawingQuads();
                tes.AddVertexWithUV(-1.0D, 1.0D, 0.0D, (double)var21, (double)var22);
                tes.AddVertexWithUV(1.0D, 1.0D, 0.0D, (double)var23, (double)var22);
                tes.AddVertexWithUV(1.0D, -1.0D, 0.0D, (double)var23, (double)var24);
                tes.AddVertexWithUV(-1.0D, -1.0D, 0.0D, (double)var21, (double)var24);
                tes.Draw();
                GL.PopMatrix();
            }

			GL.PushMatrix();
			GL.Translate(0.0F, 0.0F, -0.04F);
			GL.Scale(1.0F, 1.0F, 1.0F);
			this.fontRenderer.drawString(var3.field_28168_a, var15, var16, -16777216);
			GL.PopMatrix();
		}
	}


}
