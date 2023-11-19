using System.IO;
using System.Text;
namespace net.minecraft.src.json
{
	sealed class J_PositionTrackingPushbackReader : J_ThingWithPosition {
		
		private int field_27337_b = 0;
		private int field_27340_c = 1;
		private bool field_27339_d = false;

		private readonly TextReader reader;
        private readonly StringBuilder pushbackBuffer;
        private int position = 0;
        private int line = 1;
        private bool unreadFlag = false;

        public J_PositionTrackingPushbackReader(TextReader reader)
        {
            this.reader = reader;
            this.pushbackBuffer = new StringBuilder();
        }

		public void func_27334_a(char var1)  {
			--this.field_27337_b;
			if(this.field_27337_b < 0) {
				this.field_27337_b = 0;
			}

			this.Unread(var1);
		}

		public void func_27335_a(char[] var1) {
			this.field_27337_b -= var1.Length;
			if(this.field_27337_b < 0) {
				this.field_27337_b = 0;
			}

		}

		public int func_27333_c()  {
			int var1 = this.Read();
			this.func_27332_a(var1);
			return var1;
		}

		public int func_27336_b(char[] var1)  {
			int var2 = this.Read();
			char[] var3 = var1;
			int var4 = var1.Length;

			for(int var5 = 0; var5 < var4; ++var5) {
				char var6 = var3[var5];
				this.func_27332_a(var6);
			}

			return var2;
		}

		private void func_27332_a(int var1) {
			if(13 == var1) {
				this.field_27337_b = 0;
				++this.field_27340_c;
				this.field_27339_d = true;
			} else {
				if(10 == var1 && !this.field_27339_d) {
					this.field_27337_b = 0;
					++this.field_27340_c;
				} else {
					++this.field_27337_b;
				}

				this.field_27339_d = false;
			}

		}

		public int func_27331_a() {
			return this.field_27337_b;
		}

		public int func_27330_b() {
			return this.field_27340_c;
		}

		public void Unread(char ch)
        {
            if (unreadFlag)
            {
                position--;
                if (position < 0)
                {
                    position = 0;
                }
            }

            pushbackBuffer.Insert(0, ch);
            unreadFlag = true;
        }

        public void Unread(char[] chars)
        {
            if (unreadFlag)
            {
                position -= chars.Length;
                if (position < 0)
                {
                    position = 0;
                }
            }

            pushbackBuffer.Insert(0, chars);
            unreadFlag = true;
        }

        public int Read()
        {
            if (unreadFlag)
            {
                unreadFlag = false;
                if (pushbackBuffer.Length > 0)
                {
                    position++;
                    return pushbackBuffer[0];
                }
            }

            int nextChar = reader.Read();
            TrackPosition(nextChar);
            return nextChar;
        }

        public int Read(char[] buffer, int index, int count)
        {
            if (unreadFlag)
            {
                unreadFlag = false;
                int availableChars = Math.Min(count, pushbackBuffer.Length);
                if (availableChars > 0)
                {
                    position += availableChars;
                    pushbackBuffer.CopyTo(0, buffer, index, availableChars);
                    pushbackBuffer.Remove(0, availableChars);
                    return availableChars;
                }
            }

            int bytesRead = reader.Read(buffer, index, count);
            TrackPosition(buffer, index, bytesRead);
            return bytesRead;
        }

        private void TrackPosition(int character)
        {
            if (character == 13)
            {
                position = 0;
                line++;
                unreadFlag = true;
            }
            else
            {
                if (character == 10 && !unreadFlag)
                {
                    position = 0;
                    line++;
                }
                else
                {
                    position++;
                }
            }
        }

        private void TrackPosition(char[] buffer, int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                TrackPosition(buffer[index + i]);
            }
        }

        public int GetPosition()
        {
            return position;
        }

        public int GetLine()
        {
            return line;
        }
	}

}

