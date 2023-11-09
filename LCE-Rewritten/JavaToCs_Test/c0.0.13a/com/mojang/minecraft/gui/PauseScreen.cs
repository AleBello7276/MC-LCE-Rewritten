


using com.mojang.minecraft.level;
using com.mojang.minecraft;
using OpenTK.Input;



namespace com.mojang.minecraft.gui
{
    public class PauseScreen : Screen
    {



        private List<Button> buttons = new List<Button>();

        public override void init()
        {
            Console.WriteLine("HOlo");
            this.buttons.Add(new Button(0, this.width / 2 - 100, this.height / 3 + 0, 200, 20, "Generate new level"));
            this.buttons.Add(new Button(1, this.width / 2 - 100, this.height / 3 + 32, 200, 20, "Save level.."));
            this.buttons.Add(new Button(2, this.width / 2 - 100, this.height / 3 + 64, 200, 20, "Load level.."));
            this.buttons.Add(new Button(3, this.width / 2 - 100, this.height / 3 + 96, 200, 20, "Back to game"));
            this.buttons.Add(new Button(4, this.width / 2 - 200, this.height / 3 + -45, 20, 20, "1"));
            this.buttons.Add(new Button(5, this.width / 2 - 165, this.height / 3 + -45, 20, 20, "2"));
            this.buttons.Add(new Button(6, this.width / 2 - 200, this.height / 3 + -5, 20, 20, "3"));
            this.buttons.Add(new Button(7, this.width / 2 - 165, this.height / 3 + -5, 20, 20, "4"));
            this.buttons.Add(new Button(8, this.width / 2 - 200, this.height / 3 + 35, 20, 20, "5"));
            this.buttons.Add(new Button(9, this.width / 2 - 165, this.height / 3 + 35, 20, 20, "6"));
            this.buttons.Add(new Button(10, this.width / 2 - 200, this.height / 3 + 75, 20, 20, "7"));
            this.buttons.Add(new Button(11, this.width / 2 - 165, this.height / 3 + 75, 20, 20, "8"));
            this.buttons.Add(new Button(12, this.width / 2 - 200, this.height / 3 + 115, 20, 20, "44"));
            this.buttons.Add(new Button(13, this.width / 2 - 165, this.height / 3 + 115, 20, 20, "100"));

        }

        protected override void keyPressed(char eventCharacter, int eventKey)
        {

        }

        protected override void mouseClicked(int x, int y, int buttonNum)
        {
            if (buttonNum == 0)
            {
                for (int i = 0; i < this.buttons.Count; ++i)
                {
                    Button button = (Button)this.buttons[i];
                    if (x >= button.x && y >= button.y && x < button.x + button.w && y < button.y + button.h)
                    {
                        this.buttonClicked(button);
                        this.buttonClickedMod(button);

                    }
                }
            }

        }

        private void buttonClicked(Button button)
        {
            if (button.id == 0)
            {
                this.minecraft.generateNewLevel();
                this.minecraft.setScreen(null);
                this.minecraft.grabMouse();
            }

            if (button.id == 3)
            {
                this.minecraft.setScreen(null);
                this.minecraft.grabMouse();
            }


        }




        //Mod - Ale
        private void buttonClickedMod(Button button)
        {
            if (button.id == 4)
            {
                //Console.WriteLine("1\n");
                minecraft.paintTexture = 1;
            }
            else if (button.id == 5)
            {
                //Console.WriteLine("2\n");
                minecraft.paintTexture = 2;
            }
            else if (button.id == 6)
            {
                //Console.WriteLine("3\n");
                minecraft.paintTexture = 3;
            }
            else if (button.id == 7)
            {
                //Console.WriteLine("4\n");
                minecraft.paintTexture = 4;
            }
            else if (button.id == 8)
            {
                //Console.WriteLine("5\n");
                minecraft.paintTexture = 5;
            }
            else if (button.id == 9)
            {
                //Console.WriteLine("6\n");
                minecraft.paintTexture = 6;
            }
            else if (button.id == 10)
            {
                //Console.WriteLine("7\n");
                minecraft.paintTexture = 7;
            }
            else if (button.id == 11)
            {
                //Console.WriteLine("8\n");
                minecraft.paintTexture = 8;
            }
            else if (button.id == 12)
            {
                //Console.WriteLine("44\n");
                minecraft.paintTexture = 44;
            }
            else if (button.id == 13)
            {
                //Console.WriteLine("100\n");
                minecraft.paintTexture = 100;
            }
        }





        public override void render(int xm, int ym)
        {
            this.fillGradient(0, 0, this.width, this.height, 537199872, -1607454624);

            for (int i = 0; i < this.buttons.Count; ++i)
            {
                Button button = (Button)this.buttons[i];
                this.fill(button.x - 1, button.y - 1, button.x + button.w + 1, button.y + button.h + 1, -16777216);
                if (xm >= button.x && ym >= button.y && xm < button.x + button.w && ym < button.y + button.h)
                {
                    this.fill(button.x - 1, button.y - 1, button.x + button.w + 1, button.y + button.h + 1, -6250336);
                    this.fill(button.x, button.y, button.x + button.w, button.y + button.h, -8355680);
                    this.drawCenteredString(button.msg, button.x + button.w / 2, button.y + (button.h - 8) / 2, 16777120);
                }
                else
                {
                    this.fill(button.x, button.y, button.x + button.w, button.y + button.h, -9408400);
                    this.drawCenteredString(button.msg, button.x + button.w / 2, button.y + (button.h - 8) / 2, 14737632);
                }
            }

        }
    }

}

