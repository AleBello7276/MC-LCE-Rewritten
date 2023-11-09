using com.mojang.minecraft;
using com.mojang.minecraft.level;
using com.mojang.minecraft.renderer;
using OpenTK.Graphics.OpenGL;

namespace com.mojang.minecraft.character
{
    public class Zombie : Entity
    {
        public float rot;
        public float timeOffs;
        public float speed;
        public float rotA;
        private static ZombieModel zombieModel = new ZombieModel();
        private Textures textures;
        int retSkinId;
        public Zombie(Level level, Textures textures, float x, float y, float z) : base(level)
        {
            this.textures = textures;
            retSkinId = this.textures.loadTexture("res/char.png", 9728);
            this.rotA = (float)(new Random().NextDouble() + 1.0) * 0.01f;
            this.setPos(x, y, z);
            this.timeOffs = (float)(new Random().NextDouble() * 1239813.0);
            this.rot = (float)(new Random().NextDouble() * 2 * Math.PI);
            this.speed = 1.0f;

        }


        public void tick()
        {
            this.xo = this.x;
            this.yo = this.y;
            this.zo = this.z;
            float xa = 0.0F;
            float ya = 0.0F;
            if (this.y < -100.0F)
            {
                this.remove();
            }

            this.rot += this.rotA;
            this.rotA = (float)((double)this.rotA * 0.99D);
            this.rotA = (float)((double)this.rotA + (new Random().NextDouble() - new Random().NextDouble()) * new Random().NextDouble() * new Random().NextDouble() * 0.07999999821186066D);
            xa = (float)Math.Sin((double)this.rot);
            ya = (float)Math.Cos((double)this.rot);
            if (this.onGround && new Random().NextDouble() < 0.08D)
            {
                this.yd = 0.5F;
            }

            this.moveRelative(xa, ya, this.onGround ? 0.1F : 0.02F);
            this.yd = (float)((double)this.yd - 0.08D);
            this.move(this.xd, this.yd, this.zd);
            this.xd *= 0.91F;
            this.yd *= 0.98F;
            this.zd *= 0.91F;
            if (this.onGround)
            {
                this.xd *= 0.7F;
                this.zd *= 0.7F;
            }

        }

        public void render(float a)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, retSkinId);
            GL.PushMatrix();
            double time = (double)(DateTime.Now.Ticks * 100) / 1.0E9D * 10.0D * (double)this.speed + (double)this.timeOffs;
            float size = 0.058333334F;
            float yy = (float)(-Math.Abs(Math.Sin(time * 0.6662D)) * 5.0D - 23.0D);
            GL.Translate(this.xo + (this.x - this.xo) * a, this.yo + (this.y - this.yo) * a, this.zo + (this.z - this.zo) * a);
            GL.Scale(1.0F, -1.0F, 1.0F);
            GL.Scale(size, size, size);
            GL.Translate(0.0F, yy, 0.0F);
            float c = 57.29578F;
            GL.Rotate(this.rot * c + 180.0F, 0.0F, 1.0F, 0.0F);
            zombieModel.render((float)time);
            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }
    }

}