namespace com.mojang.minecraft.gui
{
   public class Button {
      public int x;
      public int y;
      public int w;
      public int h;
      public String msg;
      public int id;

      public Button(int id, int x, int y, int w, int h, String msg) {
         this.id = id;
         this.x = x;
         this.y = y;
         this.w = w;
         this.h = h;
         this.msg = msg;
      }
   }   
}



