using System;
using System.IO;
using System.Text.RegularExpressions;

namespace net.minecraft.src.world.chunk
{
	public class ChunkFile : IComparable {
		private readonly FileInfo field_22326_a;
		private readonly int field_22325_b;
		private readonly int field_22327_c;

		public ChunkFile(FileInfo var1) {
			this.field_22326_a = var1;
			Regex var222 = ChunkFilePattern.field_22189_a;
            Match match = var222.Match(var1.Name);

			Match var2 = ChunkFilePattern.field_22189_a.Match(var1.Name);

			if(var2.Success) {
				this.field_22325_b = Convert.ToInt32(var2.Groups[1].Value, 36);;
				this.field_22327_c = Convert.ToInt32(var2.Groups[2].Value, 36);;
			} else {
				this.field_22325_b = 0;
				this.field_22327_c = 0;
			}

		}

		public int func_22322_a(ChunkFile var1) {
			int var2 = this.field_22325_b >> 5;
			int var3 = var1.field_22325_b >> 5;
			if(var2 == var3) {
				int var4 = this.field_22327_c >> 5;
				int var5 = var1.field_22327_c >> 5;
				return var4 - var5;
			} else {
				return var2 - var3;
			}
		}

		public FileInfo func_22324_a() {
			return this.field_22326_a;
		}

		public int func_22323_b() {
			return this.field_22325_b;
		}

		public int func_22321_c() {
			return this.field_22327_c;
		}

		public int CompareTo(object var1) {
			return this.func_22322_a((ChunkFile)var1);
		}
	}

}