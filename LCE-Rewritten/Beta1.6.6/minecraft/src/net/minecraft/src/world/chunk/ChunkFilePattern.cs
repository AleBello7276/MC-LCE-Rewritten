
using System.IO;
using System.Text.RegularExpressions;

namespace net.minecraft.src.world.chunk
{
	public class ChunkFilePattern : IFileNameFilter {
		public static readonly Regex field_22189_a = new Regex("c\\.(-?[0-9a-z]+)\\.(-?[0-9a-z]+)\\.dat");

		private ChunkFilePattern() 
		{
		}

		public bool accept(FileInfo  var1, String var2) {
			Match var3 = field_22189_a.Match(var2);
			return var3.Success;
		}

		
		public ChunkFilePattern(Empty2 var1) :this()
        {
           
        }
		
	}
	public interface IFileNameFilter
    {
        bool accept(FileInfo file, string var2);
    }
}

