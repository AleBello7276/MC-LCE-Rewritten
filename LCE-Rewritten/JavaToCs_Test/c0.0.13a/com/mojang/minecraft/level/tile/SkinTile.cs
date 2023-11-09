

using com.mojang.minecraft.level;

namespace com.mojang.minecraft.level.tile
{
    public class SkinTile : Tile
    {
        public SkinTile(int id, int tex) : base(id, tex)
        {
        }




        public override void tick(Level level, int x, int y, int z, Random random)
        {
            if (level.getTile(0, 0, 0) == Tile.RetroTop.id)
            {
                Console.WriteLine("Top");
            }

            if (level.getTile(0, 0, 0) == Tile.RetroBottom.id)
            {
                Console.WriteLine("Bot");
            }
        }

        public override bool isSolid()
        {
            return false;
        }
    }
}

