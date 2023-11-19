

using System.IO;
using System.Text.RegularExpressions;

namespace net.minecraft.src.world.chunk
{
	public class ChunkFolderPattern : FileFilter {
		public static readonly Regex field_22392_a =new Regex("[0-9a-z]|([0-9a-z][0-9a-z])");

		private ChunkFolderPattern() {
		}

		public bool accept(FileInfo var1) {
			if(var1.Directory != null) {
				Match var2 = field_22392_a.Match(var1.Name);
				return var2.Success;
			} else {
				return false;
			}
		}

		public ChunkFolderPattern(Empty2 var1) : this()
		{
		}
		
	}

	// Define the equivalent of FileFilter interface in C#
    public interface FileFilter
    {
        bool accept(FileInfo file);
    }
	

}

