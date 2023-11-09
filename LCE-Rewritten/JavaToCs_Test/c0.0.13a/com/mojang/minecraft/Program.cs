using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace com.mojang.minecraft
{
    class Program
    {
       public static void Main(String[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(1500, 800),
                Title = "Minecraft 0.0.13a",

                // IMPORTANT DO NOT TOUCH !!! :}
                Profile = ContextProfile.Compatability
            };

            using (var window = new Minecraft(GameWindowSettings.Default,
                                                    nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}