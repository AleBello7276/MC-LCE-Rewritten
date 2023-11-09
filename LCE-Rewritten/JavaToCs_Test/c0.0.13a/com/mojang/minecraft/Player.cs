using System;


using OpenTK.Input;



using com.mojang.minecraft.level;
using com.mojang.minecraft.level.tile;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace com.mojang.minecraft
{
    public class Player : Entity
    {
        public static readonly int KEY_UP = 0;
        public static readonly int KEY_DOWN = 1;
        public static readonly int KEY_LEFT = 2;
        public static readonly int KEY_RIGHT = 3;
        public static readonly int KEY_JUMP = 4;
        private bool[] keys = new bool[10];
        public bool toggle;

       
        //Mod variable
        public float SpeedOnGround;



        public Player(Level level) : base(level)
        {
            this.heightOffset = 1.62F;
        }


        public void releaseAllKeys(bool enable)
        {
            toggle = enable;
        }

        public void tick(KeyboardState input)
        {

            // Rotation Fix (not very useful lol) - AleBello
            if (this.yRot >= 720.00000000)
            {
                this.yRot = 0.00000000F;
            }
            if (this.yRot <= -720.00000000)
            {
                this.yRot = 0.00000000F;
            }

            this.xo = this.x;
            this.yo = this.y;
            this.zo = this.z;
            float xa = 0.0F;
            float ya = 0.0F;
            bool inWater = this.isInWater();
            bool inLava = this.isInLava();


            if (!toggle)
            {
                if (input.IsKeyDown(Keys.W))
                {
                    --ya;
                }

                if (input.IsKeyDown(Keys.S))
                {
                    ++ya;
                }

                if (input.IsKeyDown(Keys.A))
                {
                    --xa;
                }

                if (input.IsKeyDown(Keys.D))
                {
                    ++xa;
                }

                if (input.IsKeyDown(Keys.Space))
                {
                    if (inWater)
                    {
                        this.yd += 0.04F;
                    }
                    else if (inLava)
                    {
                        this.yd += 0.04F;
                    }
                    else if (this.onGround)
                    {
                        this.yd = 0.42F;

                        
                    }
                }

                //Mod - Alebello
                //Sprint
                if (input.IsKeyDown(Keys.LeftControl))
                {
                    SpeedOnGround = 0.3f;
                }
                else
                {
                    SpeedOnGround = 0.15f;
                }
                // "Fly"
                if (input.IsKeyDown(Keys.E))
                {

                    SpeedOnGround = 0.1f;
                    Console.WriteLine("Fly\n");

                    yd = .3f;
                }
            }






            float yo;
            if (inWater)
            {
                yo = this.y;
                this.moveRelative(xa, ya, 0.02F);
                this.move(this.xd, this.yd, this.zd);
                this.xd *= 0.8F;
                this.yd *= 0.8F;
                this.zd *= 0.8F;
                this.yd = (float)((double)this.yd - 0.02D);
                if (this.horizontalCollision && this.isFree(this.xd, this.yd + 0.6F - this.y + yo, this.zd))
                {
                    this.yd = 0.3F;
                }
            }
            else if (inLava)
            {
                yo = this.y;
                this.moveRelative(xa, ya, 0.02F);
                this.move(this.xd, this.yd, this.zd);
                this.xd *= 0.5F;
                this.yd *= 0.5F;
                this.zd *= 0.5F;
                this.yd = (float)((double)this.yd - 0.02D);
                if (this.horizontalCollision && this.isFree(this.xd, this.yd + 0.6F - this.y + yo, this.zd))
                {
                    this.yd = 0.3F;
                }
            }
            else
            {
                this.moveRelative(xa, ya, this.onGround ? SpeedOnGround : 0.02F);
                this.move(this.xd, this.yd, this.zd);
                this.xd *= 0.91F;
                this.yd *= 0.98F;
                this.zd *= 0.91F;
                this.yd = (float)((double)this.yd - 0.08D);
                if (this.onGround)
                {
                    this.xd *= 0.6F;
                    this.zd *= 0.6F;
                }
            }

        }

        internal void ResetPos()
        {
            resetPos();
        }
    }
}