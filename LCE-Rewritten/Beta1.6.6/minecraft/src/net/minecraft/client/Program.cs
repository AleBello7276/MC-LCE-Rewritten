using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using net.minecraft.util;

namespace net.minecraft.client
{
    class Program
    {
       public static void Main(String[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new Vector2i(1500, 800),
                Title = "Minecraft Minecraft Beta 1.6.6",

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