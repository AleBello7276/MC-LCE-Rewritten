using OpenTK.Graphics.OpenGL;

namespace net.minecraft.src
{
	public class OpenGlCapsChecker
	{
		private static bool tryCheckOcclusionCapable = true;

		public bool CheckARBOcclusion()
		{
			return tryCheckOcclusionCapable && GL.GetString(StringName.Extensions).Contains("GL_ARB_occlusion_query");
		}
	}
}






