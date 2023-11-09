
using com.mojang.minecraft;
using com.mojang.minecraft.level;
using com.mojang.minecraft.renderer;

using OpenTK.Graphics.OpenGL;

namespace com.mojang.minecraft.particle
{
    public class ParticleEngine
    {
        protected Level level;
        private List<Particle> particles = new List<Particle>();
        private Textures textures;

        int terrId;

        public ParticleEngine(Level level, Textures textures)
        {
            this.level = level;
            this.textures = textures;
           terrId = this.textures.loadTexture("res/terrain.png", 9728);
        }

        public void add(Particle p)
        {
            this.particles.Add(p);
        }

        public void tick()
        {
            for (int i = 0; i < this.particles.Count; ++i)
            {
                Particle p = (Particle)this.particles[i];
                p.tick();
                if (p.removed)
                {
                    this.particles.RemoveAt(i--);
                }
            }

        }

        public void render(Player player, float a, int layer)
        {
            if (this.particles.Count != 0)
            {
                GL.Enable(EnableCap.Texture2D);

                GL.BindTexture(TextureTarget.Texture2D, terrId);
                float xa = -((float)Math.Cos((double)player.yRot * 3.141592653589793D / 180.0D));
                float za = -((float)Math.Sin((double)player.yRot * 3.141592653589793D / 180.0D));
                float xa2 = -za * (float)Math.Sin((double)player.xRot * 3.141592653589793D / 180.0D);
                float za2 = xa * (float)Math.Sin((double)player.xRot * 3.141592653589793D / 180.0D);
                float ya = (float)Math.Cos((double)player.xRot * 3.141592653589793D / 180.0D);
                Tesselator t = Tesselator.instance;
                GL.Color4(0.8F, 0.8F, 0.8F, 1.0F);
                t.begin();

                for (int i = 0; i < this.particles.Count; ++i)
                {
                    Particle p = (Particle)this.particles[i];
                    if (p.isLit() ^ layer == 1)
                    {
                        p.render(t, a, xa, ya, za, xa2, za2);
                    }
                }

                t.end();
                GL.Disable(EnableCap.Texture2D);
            }
        }
    }

}