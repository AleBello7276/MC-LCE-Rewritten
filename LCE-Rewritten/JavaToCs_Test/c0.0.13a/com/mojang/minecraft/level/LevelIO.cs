using System;
using System.IO;
using System.IO.Compression;
using System.Text.Unicode;

namespace com.mojang.minecraft.level
{
    public class LevelIO
    {
        private static readonly int MAGIC_NUMBER = 656127880;
        private static readonly int CURRENT_VERSION = 1;
        private LevelLoaderListener levelLoaderListener;
        public String error = null;

        public LevelIO(LevelLoaderListener levelLoaderListener)
        {
            this.levelLoaderListener = levelLoaderListener;
        }

        public bool load(Level level, Stream stream)
        {
            this.levelLoaderListener.beginLevelLoading("Loading level");
            this.levelLoaderListener.levelLoadUpdate("Reading..");

            try
            {
                using (BinaryReader br = new BinaryReader(new GZipStream(stream, CompressionMode.Decompress)))
                {
                    int magic = br.ReadInt32();
                    if (magic != 656127880)
                    {
                        this.error = "Bad level file format";
                        return false;
                    }
                    else
                    {
                        byte version = br.ReadByte();
                        if (version > 1)
                        {
                            this.error = "Bad level file format";
                            return false;
                        }
                        else
                        {
                            string name = br.ReadString();
                            string creator = br.ReadString();
                            long createTime = br.ReadInt64();
                            int width = br.ReadInt16();
                            int height = br.ReadInt16();
                            int depth = br.ReadInt16();
                            byte[] blocks = new byte[width * height * depth];
                            br.Read(blocks, 0, blocks.Length);
                            br.Close();
                            level.setData(width, depth, height, blocks);
                            level.name = name;
                            level.creator = creator;
                            level.createTime = createTime;
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.error = "Failed to load level: " + ex.ToString();
                return false;
            }
        }


        public bool loadLegacy(Level level, Stream inputStream)
        {
            this.levelLoaderListener.beginLevelLoading("Loading level");
            this.levelLoaderListener.levelLoadUpdate("Reading..");

            try
            {
                using (BinaryReader reader = new BinaryReader(new GZipStream(inputStream, CompressionMode.Decompress)))
                {
                    string name = "--";
                    string creator = "unknown";
                    long createTime = 0L;
                    int width = 256;
                    int height = 256;
                    int depth = 64;
                    byte[] blocks = new byte[width * height * depth];
                    reader.Read(blocks, 0, blocks.Length);

                    level.setData(width, depth, height, blocks);
                    level.name = name;
                    level.creator = creator;
                    level.createTime = createTime;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.error = "Failed to load level: " + ex.ToString();
                return false;
            }
        }


        public void save(Level level, Stream outputStream)
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(new GZipStream(outputStream, CompressionMode.Compress)))
                {
                    writer.Write((int)656127880);
                    writer.Write((byte)1);
                    writer.Write((string)level.name);
                    writer.Write((string)level.creator);
                    writer.Write((long)level.createTime);
                    writer.Write((short)level.width);
                    writer.Write((short)level.height);
                    writer.Write((short)level.depth);
                    writer.Write(level.blocks);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }

}

