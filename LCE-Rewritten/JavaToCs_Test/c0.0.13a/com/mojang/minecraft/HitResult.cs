namespace com.mojang.minecraft
{
    public class HitResult
    {
        public int type;
        public int x;
        public int y;
        public int z;
        public int f;

        public HitResult(int type, int x, int y, int z, int f)
        {
            this.type = type;
            this.x = x;
            this.y = y;
            this.z = z;
            this.f = f;
        }

        public bool isCloserThan(Player player, HitResult o, int editMode)
        {
            float dist = this.distanceTo(player, 0);
            float dist2 = o.distanceTo(player, 0);
            if (dist < dist2)
            {
                return true;
            }
            else
            {
                dist = this.distanceTo(player, editMode);
                dist2 = o.distanceTo(player, editMode);
                return dist < dist2;
            }
        }

        private float distanceTo(Player player, int editMode)
        {
            int xx = this.x;
            int yy = this.y;
            int zz = this.z;
            if (editMode == 1)
            {
                if (this.f == 0)
                {
                    --yy;
                }

                if (this.f == 1)
                {
                    ++yy;
                }

                if (this.f == 2)
                {
                    --zz;
                }

                if (this.f == 3)
                {
                    ++zz;
                }

                if (this.f == 4)
                {
                    --xx;
                }

                if (this.f == 5)
                {
                    ++xx;
                }
            }

            float xd = (float)xx - player.x;
            float yd = (float)yy - player.y;
            float zd = (float)zz - player.z;
            return xd * xd + yd * yd + zd * zd;
        }
    }

}