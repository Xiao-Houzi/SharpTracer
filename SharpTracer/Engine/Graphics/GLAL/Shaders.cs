using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpGL;
using SharpGL.SceneGraph.Shaders;

namespace SharpEngine.Engine.Graphics
{
	public class Shaders
	{
		OpenGL GL;
		// Shader parameters
		// matrices

		private Dictionary<String, ShaderData> _program;

		public ShaderData this[String name]
		{
			get
			{
				return _program[name];
			}
		}

		public Shaders(OpenGL GL)
		{
			this.GL = GL;
			_program = new Dictionary<string, ShaderData>();
		}

		~Shaders()
		{
			//foreach(ShaderData p in _program.Values)
			//	GL.DeleteProgram(p.ProgramID);
		}

		public ShaderData Program(String programName)
		{
			return _program[programName];
		}

		uint CompileShader( uint type, String source)
		{
			uint shader = GL.CreateShader(type);

			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);

			int[] compileResult = new int[1] { 0 };
			GL.GetShader(shader, OpenGL.GL_COMPILE_STATUS, compileResult);

			if(compileResult[0] == 0)
			{
				int[] infoLogLength = new int[1] { 0 };
				GL.GetShader(shader, OpenGL.GL_INFO_LOG_LENGTH, infoLogLength);

				StringBuilder infoLog = new StringBuilder(infoLogLength[0]);
				//gl.GetShaderInfoLog(shader, bufsz, length, infoLog);
				GL.GetShaderInfoLog(shader, infoLogLength[0], IntPtr.Zero, infoLog);

				String errorMessage = "Shader compilation failed: ";
				errorMessage += infoLog;

				throw new Exception(errorMessage);
			}
			return shader;
		}

		internal void LoadShaders(Renderer r)
		{
			uint id;
			ShaderData sd;

			foreach(KeyValuePair<string, Tuple<string, string>> shader in r.shaders)
			{
				if ((id =CompileProgram(shader.Key, shader.Value.Item1, shader.Value.Item2)) < 999)
				{
					sd = new ShaderData(id);

					sd.ModelMatrix = GL.GetUniformLocation(id, "uModelMatrix");
					sd.ViewlMatrix = GL.GetUniformLocation(id, "uViewMatrix");
					sd.ProjlMatrix = GL.GetUniformLocation(id, "uProjMatrix");

					sd.DataMatrix = GL.GetUniformLocation(id, "uData");

					_program.Add(shader.Key, sd);
				}
			}
		}

		public uint CompileProgram( String programName, String vertexshaderSource, String fragmentshaderSource)
		{
			uint programBuffer = GL.CreateProgram();
			if(programBuffer == 0)
			{
				throw new Exception("Program creation failed");
			}

			uint vs = CompileShader( OpenGL.GL_VERTEX_SHADER, vertexshaderSource);
			uint fs = CompileShader( OpenGL.GL_FRAGMENT_SHADER, fragmentshaderSource);

			if(vs == 0 || fs == 0)
			{
				GL.DeleteShader(fs);
				GL.DeleteShader(vs);
				GL.DeleteProgram(programBuffer);
				return 999;
			}

			GL.AttachShader(programBuffer, vs);
			GL.DeleteShader(vs);

			GL.AttachShader(programBuffer, fs);
			GL.DeleteShader(fs);

			GL.LinkProgram(programBuffer);

			int[] linkStatus = new int[1] { 0 };
			GL.GetProgram(programBuffer, OpenGL.GL_LINK_STATUS, linkStatus);

			if(linkStatus[0] != (int)OpenGL.GL_TRUE)
			{
				int[] infoLogLength = new int[1] { 0 };
				GL.GetProgram(programBuffer, OpenGL.GL_INFO_LOG_LENGTH, infoLogLength);

				StringBuilder infoLog = new StringBuilder();
				IntPtr length = new IntPtr();
				int bufsz = 0;

				GL.GetProgramInfoLog(programBuffer, bufsz, length, infoLog);

				String errorMessage = "Program link failed: ";
				errorMessage += infoLog;

				throw new Exception(errorMessage);
			}
			else { }

			return programBuffer;
		}

	}
}
