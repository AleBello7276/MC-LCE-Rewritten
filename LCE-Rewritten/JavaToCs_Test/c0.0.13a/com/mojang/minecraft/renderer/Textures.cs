


using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using StbImageSharp;

namespace com.mojang.minecraft.renderer
{
    // A helper class, meant to simplify loading textures.
    public class Textures
    {
        public readonly int Handle;

        public int loadTexture(string path, int mode)
        {
            // Generate handle
            int handle = GL.GenTexture();
            Console.WriteLine(path);

            // Bind the handle
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            StbImage.stbi_set_flip_vertically_on_load(1);

            // Here we open a stream to the file and pass it to StbImageSharp to load.
            using (Stream stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);


                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            // Now that our texture is loaded, we can set a few settings to affect how the image appears on rendering.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, mode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, mode);


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // Next, generate mipmaps
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return handle;
        }
    }
}