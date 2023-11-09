using com.mojang.minecraft.level;
using com.mojang.minecraft.phys;
using System.Numerics;

namespace com.mojang.minecraft
{
    public class Entity
    {
        protected Level level;
        public float xo;
        public float yo;
        public float zo;
        public float x;
        public float y;
        public float z;
        public float xd;
        public float yd;
        public float zd;
        public float yRot;
        public float xRot;
        public AABB bb;
        public bool onGround = false;
        public bool horizontalCollision = false;
        public bool removed = false;
        protected float heightOffset = 0.0F;
        protected float bbWidth = 0.6F;
        protected float bbHeight = 1.8F;



        public Entity(Level level)
        {
            this.level = level;
            this.resetPos();
        }

        public void resetPos()
        {
            float x = (float)(new Random().NextDouble() * (this.level.width - 2) + 1.0);
            float y = (float)(this.level.depth + 10);
            float z = (float)(new Random().NextDouble() * (this.level.height - 2) + 1.0);

            this.setPos(x, y, z);
        }

        public void remove()
        {
            this.removed = true;
        }

        protected void setSize(float w, float h)
        {
            this.bbWidth = w;
            this.bbHeight = h;
        }

        protected void setPos(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            float w = this.bbWidth / 2.0F;
            float h = this.bbHeight / 2.0F;
            this.bb = new AABB(x - w, y - h, z - w, x + w, y + h, z + w);
        }

        public void turn(float xo, float yo)
        {
            yRot += xo * 0.15f;
            xRot += yo * 0.15f;

            //Clamp rotation
            if (this.xRot < -90.0F)
            {
                this.xRot = -90.0F;
            }
            if (this.xRot > 90.0F)
            {
                this.xRot = 90.0F;
            }

        }

        public virtual void tick()
        {
            this.xo = this.x;
            this.yo = this.y;
            this.zo = this.z;
        }

        public bool isFree(float xa, float ya, float za)
        {
            AABB box = this.bb.cloneMove(xa, ya, za);
            List<AABB> aABBs = this.level.getCubes(box);
            if (aABBs.Count() > 0)
            {
                return false;
            }
            else
            {
                return !this.level.containsAnyLiquid(box);
            }
        }

        public void move(float xa, float ya, float za)
        {
            float xaOrg = xa;
            float yaOrg = ya;
            float zaOrg = za;
            List<AABB> aABBs = this.level.getCubes(this.bb.expand(xa, ya, za));

            int i;
            for (i = 0; i < aABBs.Count(); ++i)
            {
                ya = ((AABB)aABBs[i]).clipYCollide(this.bb, ya);
            }

            this.bb.move(0.0F, ya, 0.0F);

            for (i = 0; i < aABBs.Count(); ++i)
            {
                xa = ((AABB)aABBs[i]).clipXCollide(this.bb, xa);
            }

            this.bb.move(xa, 0.0F, 0.0F);

            for (i = 0; i < aABBs.Count(); ++i)
            {
                za = ((AABB)aABBs[i]).clipZCollide(this.bb, za);
            }

            this.bb.move(0.0F, 0.0F, za);
            this.horizontalCollision = xaOrg != xa || zaOrg != za;
            this.onGround = yaOrg != ya && yaOrg < 0.0F;
            if (xaOrg != xa)
            {
                this.xd = 0.0F;
            }

            if (yaOrg != ya)
            {
                this.yd = 0.0F;
            }

            if (zaOrg != za)
            {
                this.zd = 0.0F;
            }

            this.x = (this.bb.x0 + this.bb.x1) / 2.0F;
            this.y = this.bb.y0 + this.heightOffset;
            this.z = (this.bb.z0 + this.bb.z1) / 2.0F;
        }

        public bool isInWater()
        {
            return this.level.containsLiquid(this.bb.grow(0.0F, -0.4F, 0.0F), 1);
        }

        public bool isInLava()
        {
            return this.level.containsLiquid(this.bb, 2);
        }

        public void moveRelative(float xa, float za, float speed)
        {
            float dist = xa * xa + za * za;
            if (!(dist < 0.01F))
            {
                dist = speed / (float)Math.Sqrt((double)dist);
                xa *= dist;
                za *= dist;
                float sin = (float)Math.Sin((double)this.yRot * 3.141592653589793D / 180.0D);
                float cos = (float)Math.Cos((double)this.yRot * 3.141592653589793D / 180.0D);
                this.xd += xa * cos - za * sin;
                this.zd += za * cos + xa * sin;
            }
        }

        public bool isLit()
        {
            int xTile = (int)this.x;
            int yTile = (int)this.y;
            int zTile = (int)this.z;
            return this.level.isLit(xTile, yTile, zTile);
        }

        public virtual void render(float a)
        {
        }
    }

}
