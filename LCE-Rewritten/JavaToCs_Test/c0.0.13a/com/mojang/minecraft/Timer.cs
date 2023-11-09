namespace com.mojang.minecraft
{
    public class Timer
    {
        private static readonly long NS_PER_SECOND = 1000000000L;
        private static readonly long MAX_NS_PER_UPDATE = 1000000000L;
        private static readonly int MAX_TICKS_PER_UPDATE = 100;
        private float ticksPerSecond;
        private long lastTime;
        public int ticks;
        public float a;
        public float timeScale = 1.0F;
        public float fps = 0.0F;
        public float passedTime = 0.0F;

        public Timer(float ticksPerSecond)
        {
            this.ticksPerSecond = ticksPerSecond;
            this.lastTime = DateTime.Now.Ticks * 100;
        }

        public void advanceTime()
        {
            long now = DateTime.Now.Ticks * 100;
            long passedNs = now - this.lastTime;
            this.lastTime = now;
            if (passedNs < 0L)
            {
                passedNs = 0L;
            }

            if (passedNs > 1000000000L)
            {
                passedNs = 1000000000L;
            }

            this.fps = (float)(1000000000L / passedNs);
            this.passedTime += (float)passedNs * this.timeScale * this.ticksPerSecond / 1.0E9F;
            this.ticks = (int)this.passedTime;
            if (this.ticks > 100)
            {
                this.ticks = 100;
            }

            this.passedTime -= (float)this.ticks;
            this.a = this.passedTime;
        }
    }
}


