using System.Buffers;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using com.mojang.minecraft.phys;

namespace com.mojang.minecraft.renderer
{
    public class Frustum
    {

        public float[,] m_Frustum = new float[6, 4];
        public static readonly int RIGHT = 0;
        public static readonly int LEFT = 1;
        public static readonly int BOTTOM = 2;
        public static readonly int TOP = 3;
        public static readonly int BACK = 4;
        public static readonly int FRONT = 5;
        public static readonly int A = 0;
        public static readonly int B = 1;
        public static readonly int C = 2;
        public static readonly int D = 3;
        private static Frustum frustum = new Frustum();
        private float[] _proj = new float[16];
        private float[] _modl = new float[16];
        private float[] _clip = new float[16];
        float[] proj = new float[16];
        float[] modl = new float[16];
        float[] clip = new float[16];

        private Frustum()
        {
        }

        public static Frustum GetFrustum()
        {
            frustum.CalculateFrustum();
            return frustum;
        }

        private void NormalizePlane(float[,] frustum, int side)
        {
            float magnitude = (float)Math.Sqrt((double)frustum[side, 0] * frustum[side, 0] + frustum[side, 1] * frustum[side, 1] + frustum[side, 2] * frustum[side, 2]);
            frustum[side, 0] /= magnitude;
            frustum[side, 1] /= magnitude;
            frustum[side, 2] /= magnitude;
            frustum[side, 3] /= magnitude;
        }

        private void CalculateFrustum()
        {
            Array.Clear(_proj);
            Array.Clear(_modl);
            Array.Clear(_clip);
            GL.GetFloat((GetPName)2983, this._proj); 
            GL.GetFloat((GetPName)2982, this._modl); 
            Array.Copy(_proj, proj, 16);
            Array.Copy(_modl, modl, 16);



            this.clip[0] = this.modl[0] * this.proj[0] + this.modl[1] * this.proj[4] + this.modl[2] * this.proj[8] + this.modl[3] * this.proj[12];
            this.clip[1] = this.modl[0] * this.proj[1] + this.modl[1] * this.proj[5] + this.modl[2] * this.proj[9] + this.modl[3] * this.proj[13];
            this.clip[2] = this.modl[0] * this.proj[2] + this.modl[1] * this.proj[6] + this.modl[2] * this.proj[10] + this.modl[3] * this.proj[14];
            this.clip[3] = this.modl[0] * this.proj[3] + this.modl[1] * this.proj[7] + this.modl[2] * this.proj[11] + this.modl[3] * this.proj[15];
            this.clip[4] = this.modl[4] * this.proj[0] + this.modl[5] * this.proj[4] + this.modl[6] * this.proj[8] + this.modl[7] * this.proj[12];
            this.clip[5] = this.modl[4] * this.proj[1] + this.modl[5] * this.proj[5] + this.modl[6] * this.proj[9] + this.modl[7] * this.proj[13];
            this.clip[6] = this.modl[4] * this.proj[2] + this.modl[5] * this.proj[6] + this.modl[6] * this.proj[10] + this.modl[7] * this.proj[14];
            this.clip[7] = this.modl[4] * this.proj[3] + this.modl[5] * this.proj[7] + this.modl[6] * this.proj[11] + this.modl[7] * this.proj[15];
            this.clip[8] = this.modl[8] * this.proj[0] + this.modl[9] * this.proj[4] + this.modl[10] * this.proj[8] + this.modl[11] * this.proj[12];
            this.clip[9] = this.modl[8] * this.proj[1] + this.modl[9] * this.proj[5] + this.modl[10] * this.proj[9] + this.modl[11] * this.proj[13];
            this.clip[10] = this.modl[8] * this.proj[2] + this.modl[9] * this.proj[6] + this.modl[10] * this.proj[10] + this.modl[11] * this.proj[14];
            this.clip[11] = this.modl[8] * this.proj[3] + this.modl[9] * this.proj[7] + this.modl[10] * this.proj[11] + this.modl[11] * this.proj[15];
            this.clip[12] = this.modl[12] * this.proj[0] + this.modl[13] * this.proj[4] + this.modl[14] * this.proj[8] + this.modl[15] * this.proj[12];
            this.clip[13] = this.modl[12] * this.proj[1] + this.modl[13] * this.proj[5] + this.modl[14] * this.proj[9] + this.modl[15] * this.proj[13];
            this.clip[14] = this.modl[12] * this.proj[2] + this.modl[13] * this.proj[6] + this.modl[14] * this.proj[10] + this.modl[15] * this.proj[14];
            this.clip[15] = this.modl[12] * this.proj[3] + this.modl[13] * this.proj[7] + this.modl[14] * this.proj[11] + this.modl[15] * this.proj[15];

            this.m_Frustum[0, 0] = this.clip[3] - this.clip[0];
            this.m_Frustum[0, 1] = this.clip[7] - this.clip[4];
            this.m_Frustum[0, 2] = this.clip[11] - this.clip[8];
            this.m_Frustum[0, 3] = this.clip[15] - this.clip[12];
            this.NormalizePlane(this.m_Frustum, 0);


            this.m_Frustum[1, 0] = this.clip[3] + this.clip[0];
            this.m_Frustum[1, 1] = this.clip[7] + this.clip[4];
            this.m_Frustum[1, 2] = this.clip[11] + this.clip[8];
            this.m_Frustum[1, 3] = this.clip[15] + this.clip[12];
            this.NormalizePlane(this.m_Frustum, 1);

            this.m_Frustum[2, 0] = this.clip[3] + this.clip[1];
            this.m_Frustum[2, 1] = this.clip[7] + this.clip[5];
            this.m_Frustum[2, 2] = this.clip[11] + this.clip[9];
            this.m_Frustum[2, 3] = this.clip[15] + this.clip[13];
            this.NormalizePlane(this.m_Frustum, 2);


            this.m_Frustum[3, 0] = this.clip[3] - this.clip[1];
            this.m_Frustum[3, 1] = this.clip[7] - this.clip[5];
            this.m_Frustum[3, 2] = this.clip[11] - this.clip[9];
            this.m_Frustum[3, 3] = this.clip[15] - this.clip[13];
            this.NormalizePlane(this.m_Frustum, 3);

            this.m_Frustum[4, 0] = this.clip[3] - this.clip[2];
            this.m_Frustum[4, 1] = this.clip[7] - this.clip[6];
            this.m_Frustum[4, 2] = this.clip[11] - this.clip[10];
            this.m_Frustum[4, 3] = this.clip[15] - this.clip[14];
            this.NormalizePlane(this.m_Frustum, 4);

            this.m_Frustum[5, 0] = this.clip[3] + this.clip[2];
            this.m_Frustum[5, 1] = this.clip[7] + this.clip[6];
            this.m_Frustum[5, 2] = this.clip[11] + this.clip[10];
            this.m_Frustum[5, 3] = this.clip[15] + this.clip[14];
            this.NormalizePlane(this.m_Frustum, 5);
        }

        public bool PointInFrustum(float x, float y, float z)
        {
            for (int i = 0; i < 6; i++)
            {
                if (this.m_Frustum[i, 0] * x + this.m_Frustum[i, 1] * y + this.m_Frustum[i, 2] * z + this.m_Frustum[i, 3] <= 0.0f)
                {
                    return false;
                }
            }

            return true;
        }

        public bool SphereInFrustum(float x, float y, float z, float radius)
        {
            for (int i = 0; i < 6; i++)
            {
                if (this.m_Frustum[i, 0] * x + this.m_Frustum[i, 1] * y + this.m_Frustum[i, 2] * z + this.m_Frustum[i, 3] <= -radius)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CubeFullyInFrustum(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            for (int i = 0; i < 6; ++i)
            {
                if (!(this.m_Frustum[i, 0] * x1 + this.m_Frustum[i, 1] * y1 + this.m_Frustum[i, 2] * z1 + this.m_Frustum[i, 3] > 0.0F))
                {
                    return false;
                }

                if (!(this.m_Frustum[i, 0] * x2 + this.m_Frustum[i, 1] * y1 + this.m_Frustum[i, 2] * z1 + this.m_Frustum[i, 3] > 0.0F))
                {
                    return false;
                }

                if (!(this.m_Frustum[i, 0] * x1 + this.m_Frustum[i, 1] * y2 + this.m_Frustum[i, 2] * z1 + this.m_Frustum[i, 3] > 0.0F))
                {
                    return false;
                }

                if (!(this.m_Frustum[i, 0] * x2 + this.m_Frustum[i, 1] * y2 + this.m_Frustum[i, 2] * z1 + this.m_Frustum[i, 3] > 0.0F))
                {
                    return false;
                }

                if (!(this.m_Frustum[i, 0] * x1 + this.m_Frustum[i, 1] * y1 + this.m_Frustum[i, 2] * z2 + this.m_Frustum[i, 3] > 0.0F))
                {
                    return false;
                }

                if (!(this.m_Frustum[i, 0] * x2 + this.m_Frustum[i, 1] * y1 + this.m_Frustum[i, 2] * z2 + this.m_Frustum[i, 3] > 0.0F))
                {
                    return false;
                }

                if (!(this.m_Frustum[i, 0] * x1 + this.m_Frustum[i, 1] * y2 + this.m_Frustum[i, 2] * z2 + this.m_Frustum[i, 3] > 0.0F))
                {
                    return false;
                }

                if (!(this.m_Frustum[i, 0] * x2 + this.m_Frustum[i, 1] * y2 + this.m_Frustum[i, 2] * z2 + this.m_Frustum[i, 3] > 0.0F))
                {
                    return false;
                }
            }

            return true;
        }

        public bool CubeInFrustum(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            for (int i = 0; i < 6; ++i)
            {
                if (!(this.m_Frustum[i, 0] * x1 + this.m_Frustum[i, 1] * y1 + this.m_Frustum[i, 2] * z1 + this.m_Frustum[i, 3] > 0.0F) && !(this.m_Frustum[i, 0] * x2 + this.m_Frustum[i, 1] * y1 + this.m_Frustum[i, 2] * z1 + this.m_Frustum[i, 3] > 0.0F) && !(this.m_Frustum[i, 0] * x1 + this.m_Frustum[i, 1] * y2 + this.m_Frustum[i, 2] * z1 + this.m_Frustum[i, 3] > 0.0F) && !(this.m_Frustum[i, 0] * x2 + this.m_Frustum[i, 1] * y2 + this.m_Frustum[i, 2] * z1 + this.m_Frustum[i, 3] > 0.0F) && !(this.m_Frustum[i, 0] * x1 + this.m_Frustum[i, 1] * y1 + this.m_Frustum[i, 2] * z2 + this.m_Frustum[i, 3] > 0.0F) && !(this.m_Frustum[i, 0] * x2 + this.m_Frustum[i, 1] * y1 + this.m_Frustum[i, 2] * z2 + this.m_Frustum[i, 3] > 0.0F) && !(this.m_Frustum[i, 0] * x1 + this.m_Frustum[i, 1] * y2 + this.m_Frustum[i, 2] * z2 + this.m_Frustum[i, 3] > 0.0F) && !(this.m_Frustum[i, 0] * x2 + this.m_Frustum[i, 1] * y2 + this.m_Frustum[i, 2] * z2 + this.m_Frustum[i, 3] > 0.0F))
                {
                    return false;
                }
            }

            return true;
        }

        public bool isVisible(AABB aabb)
        {
            return this.CubeInFrustum(aabb.x0, aabb.y0, aabb.z0, aabb.x1, aabb.y1, aabb.z1);
        }



    }
}
