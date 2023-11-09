using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics.Wgl;





public class Tesselator
{
    private static readonly int MAX_MEMORY_USE = 4194304;
    private static readonly int MAX_FLOATS = 524288;
    private float[] buffer = new float[524288];
    private float[] array = new float[524288];
    private int vertices = 0;
    private float u;
    private float v;
    private float r;
    private float g;
    private float b;
    private bool hasColor = false;
    private bool hasTexture = false;
    private int len = 3;
    private int p = 0;
    private bool noColor = false;
    public static Tesselator instance = new Tesselator();


    private Tesselator()
    {
    }

    //Draw
    public void end()
    {
        if (this.vertices > 0)
        {
            Array.Clear(buffer);
            Array.Copy(array, buffer, p);
            if (this.hasTexture && this.hasColor)
            {
                GL.InterleavedArrays((InterleavedArrayFormat)10794, 0, this.buffer);
            }
            else if (this.hasTexture)
            {
                GL.InterleavedArrays((InterleavedArrayFormat)10791, 0, this.buffer);
            }
            else if (this.hasColor)
            {
                GL.InterleavedArrays((InterleavedArrayFormat)10788, 0, this.buffer);
            }
            else
            {
                GL.InterleavedArrays((InterleavedArrayFormat)10785, 0, this.buffer);
            }

            GL.EnableClientState((ArrayCap)32884);
            if (this.hasTexture)
            {
                GL.EnableClientState((ArrayCap)32888);
            }

            if (this.hasColor)
            {
                GL.EnableClientState((ArrayCap)32886);
            }


            GL.DrawArrays((PrimitiveType)7, 0, this.vertices);
            GL.DisableClientState((ArrayCap)32884);
            if (this.hasTexture)
            {
                GL.DisableClientState((ArrayCap)32888);
            }

            if (this.hasColor)
            {
                GL.DisableClientState((ArrayCap)32886);
            }
        }

        this.clear();
    }

    // Clear
    private void clear()
    {
        this.vertices = 0;
        Array.Clear(buffer);
        this.p = 0;
    }
    //Init
    public void begin()
    {
        this.clear();
        this.hasColor = false;
        this.hasTexture = false;
        this.noColor = false;
    }

    
    public void tex(float u, float v)
    {
        if (!this.hasTexture)
        {
            this.len += 2;
        }

        this.hasTexture = true;
        this.u = u;
        this.v = v;
    }

    public void color(int r, int g, int b)
    {
        this.color((byte)r, (byte)g, (byte)b);
    }

    //Add color
    public void color(byte r, byte g, byte b)
    {
        if (!this.noColor)
        {
            if (!this.hasColor)
            {
                this.len += 3;
            }

            this.hasColor = true;
            this.r = (float)(r & 255) / 255.0F;
            this.g = (float)(g & 255) / 255.0F;
            this.b = (float)(b & 255) / 255.0F;
        }
    }

    //Add a vertex with UV
    public void vertexUV(float x, float y, float z, float u, float v)
    {
        this.tex(u, v);
        this.vertex(x, y, z);
    }

    public void vertex(float x, float y, float z)
    {
        if (this.hasTexture)
        {
            this.array[this.p++] = this.u;
            this.array[this.p++] = this.v;
        }

        if (this.hasColor)
        {
            this.array[this.p++] = this.r;
            this.array[this.p++] = this.g;
            this.array[this.p++] = this.b;
        }

        this.array[this.p++] = x;
        this.array[this.p++] = y;
        this.array[this.p++] = z;
        ++this.vertices;
        if (this.vertices % 4 == 0 && this.p >= 524288 - this.len * 4)
        {
            this.end();
        }

    }

    public void color(int c)
    {
        int r = c >> 16 & 255;
        int g = c >> 8 & 255;
        int b = c & 255;
        this.color(r, g, b);
    }

    public void NoColor()
    {
        this.noColor = true;
    }
}