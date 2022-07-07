using System;
using System.Linq;
using SharpGL;
using GlmSharp;
using System.Threading;
using SharpTracer.Engine.Scene;
using SharpTracer.Engine.Scene.RenderGeometry;
using SharpEngine.Engine;
using SharpTracer.Model;

namespace SharpTracer.Engine.GLAbstraction
{
    public class GLLayer
	{
		public static OpenGL GL;
		public static Shaders Shaders
		{ get => _shaders; set => _shaders = value; }
		// other attributes


		static GLLayer()
		{
		}

		~GLLayer()
		{
		}

		public static bool Initialise(Renderer r, OpenGL GL)
		{
			Console.Error.WriteLine("Initialising openGL");
			GLLayer.GL = GL;
			_shaders = new Shaders(GL);

			// Set GL options
			GL.Enable(OpenGL.GL_PROGRAM_POINT_SIZE);
			GL.Disable(OpenGL.GL_DITHER);
			GL.Enable(OpenGL.GL_CULL_FACE);
			GL.Disable(OpenGL.GL_DEPTH_TEST);
			GL.EnableClientState(OpenGL.GL_VERTEX_ARRAY);

			GL.Enable(OpenGL.GL_BLEND);
			GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

			GL.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_FASTEST);
			GL.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_LINE);

			return true;
		}

		public static void UpdateWindowSize(int width, int height)
		{
			_width = width;
			_height = height;
			_aspect = AspectRatio();
		}

		public static void BeginFrame(Renderer state)
		{
			if(GL is null) return;
			_renderGuard.WaitOne();
			GL.ClearColor(0.025f, 0.05f, 0.10f,1);
			GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

			//draw background
		}

		public static void Render(Renderer renderer, Camera camera, Entity entity)
		{
			// Render a Graphic object
			Geometry geometry = entity.Geometry;

			// set the shader
			GL.UseProgram(_shaders.Program(entity.Material.Shader).ProgramID); //get the shader from the current image

			// get the matrices for the scene
			float[] projmat;
			if(renderer.Orthographic)
			{
				projmat = mat4.Identity.ToArray();
				projmat[0] = AspectRatio();
			}
			else
				projmat = ProjectionMatrix(AspectRatio(), camera).ToArray();
			float[] viewmat = camera.Matrix.ToArray();
			float[] modlmat = ModelMatrix(entity, geometry).ToArray();
			float[] data = new float[16];
		

			// set the matrices for the scene 
			GL.UniformMatrix4(_shaders.Program(entity.Material.Shader).ViewlMatrix, 1, false, viewmat);
			GL.UniformMatrix4(_shaders.Program(entity.Material.Shader).ProjlMatrix, 1, false, projmat);
			GL.UniformMatrix4(_shaders.Program(entity.Material.Shader).ModelMatrix, 1, false, modlmat);
			GL.UniformMatrix4(_shaders.Program(entity.Material.Shader).DataMatrix, 1, false, data);

			// Draw the object
			GL.BindVertexArray(geometry.VAO[0]);
			GL.ActiveTexture(OpenGL.GL_TEXTURE0);
			GL.BindTexture(OpenGL.GL_TEXTURE_2D, entity.Material.Texture[0]);
			GL.ActiveTexture(OpenGL.GL_TEXTURE1);
			GL.BindTexture(OpenGL.GL_TEXTURE_2D, entity.Material.Texture[1]);
			GL.ActiveTexture(OpenGL.GL_TEXTURE2);
			GL.BindTexture(OpenGL.GL_TEXTURE_2D, entity.Material.Texture[2]);

			int tex0 = GL.GetUniformLocation(_shaders.Program(entity.Material.Shader).ProgramID, "texture1");
			GL.Uniform1(tex0, 0);
			int tex1 = GL.GetUniformLocation(_shaders.Program(entity.Material.Shader).ProgramID, "texture2");
			GL.Uniform1(tex1, 1);
			int tex2 = GL.GetUniformLocation(_shaders.Program(entity.Material.Shader).ProgramID, "texture3");
			GL.Uniform1(tex2, 1);

            switch (entity.DisplayMode)
            {
                case DisplayType.DISPLAY_POINTS:
                    GL.DrawElements(OpenGL.GL_POINTS, geometry.IndexCount, null); break;
                case DisplayType.DISPLAY_WIRES:
                    GL.DrawElements(OpenGL.GL_LINES, geometry.IndexCount, null); break;
                case DisplayType.DISPLAY_SOLID:
                    GL.DrawElements(OpenGL.GL_TRIANGLES, geometry.IndexCount, null); break;

            }
		}

		public static void EndFrame()
		{
			//draw instruments

			_renderGuard.ReleaseMutex();
		}

		public static void Shutdown()
		{

		}

		public static void SetTexture(uint texture)
		{
			GL.BindTexture(OpenGL.GL_TEXTURE_2D, texture);
		}

		static mat4 ModelMatrix(Entity entity, Geometry image)
		{
			mat4 result = new mat4();
			mat4 rotation = mat4.Identity;
			mat4 translation = mat4.Identity;
			mat4 scale = mat4.Identity;

			scale.m00 = entity.Transform.Scale.x;
			scale.m11 = entity.Transform.Scale.y;
			scale.m22 = entity.Transform.Scale.z;

			rotation = entity.Transform.Orientation.ToMat4;

			translation.Column3 = new vec4(entity.Transform.Translation, 1.0f);

			result = translation * rotation * scale;
			
			return result;
		}

		static mat4 ProjectionMatrix(float aspectRatio, Camera camera)
		{
			// Far plane is at 50.0f, near plane is at 1.0f.
			float nrclip = 1f;
			float farclip = 10;

			float AR = AspectRatio();
			float Rng = nrclip - farclip;
			float tanHalfFOV = (float)Math.Tan(camera.FOV / 2.0f);

			float[] cells = {	1/(tanHalfFOV) * AR,	0.0f,						0.0f,									0.0f,
									0.0f,								1/tanHalfFOV,		0.0f,									0.0f,
									0.0f,								0.0f,						(-nrclip - farclip) / Rng,	(2* farclip * nrclip) / Rng,
									0.0f,								0.0f,						-1.0f,									0.0f };

			mat4 matrix = new mat4(
				cells[0], cells[1], cells[2], cells[3],
				cells[4], cells[5], cells[6], cells[7],
				cells[8], cells[9], cells[10], cells[11],
				cells[12], cells[13], cells[14], cells[15]);

			return matrix;
		}

		#region Private
		private static float AspectRatio()
		{
			return (float)_height / (float)_width;
		}
		private static void CheckGL_Error()
		{
			uint err = GL.GetError();
			string error = "";
			while (err != OpenGL.GL_NO_ERROR)
			{

				switch (err)
				{
					case OpenGL.GL_INVALID_OPERATION: error += "INVALID_OPERATION"; break;
					case OpenGL.GL_INVALID_ENUM: error += "INVALID_ENUM"; break;
					case OpenGL.GL_INVALID_VALUE: error += "INVALID_VALUE"; break;
					case OpenGL.GL_OUT_OF_MEMORY: error += "OUT_OF_MEMORY"; break;
				}
				err = GL.GetError();
			}
			Console.Error.WriteLine(error);
		}


		Entity _background = new Entity("Background", new MeshPlane(), new Material());
		Entity _gizmo = new Entity("Compass", new Gizmo(), new Material());
		Entity _centroid = new Entity("Centroid", new Gizmo(), new Material(), null);

		private static Shaders _shaders;
		private static int _width;
		private static int _height;
		private static float _aspect;
		public static Mutex _renderGuard = new Mutex();
		#endregion
	}
}
