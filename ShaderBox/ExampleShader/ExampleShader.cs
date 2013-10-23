using System;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;
using System.Threading;
using System.Runtime.InteropServices;

namespace ExampleShader
{
	public class ExampleShader : ShaderCrateLib.ShaderProgram
	{
		public ExampleShader ()
		{
		}
		public static volatile int state = 0;
		public static volatile string vertex = "";
		public static volatile string fragment = "";
		public override void OneTimeSetup ()
		{
			Thread x = new Thread (() => {
				if (!Glfw.Init ()) {
					Console.Error.WriteLine ("ERROR: Could not initialize GlFW, shutting down.");
					Environment.Exit (1);
				}
				Glfw.SetErrorCallback ((code, des) => {
					Console.Error.WriteLine ("ERROR ({0}): {1}", code, des);
				});
				Glfw.WindowHint (WindowHint.ContextVersionMajor, 4);
				Glfw.WindowHint (WindowHint.ContextVersionMinor, 0);
				Glfw.WindowHint (WindowHint.OpenGLForwardCompat, 1);
				//should have one more line
				GlfwWindowPtr window = Glfw.CreateWindow (800, 600, "GlFW 3 Window", GlfwMonitorPtr.Null, GlfwWindowPtr.Null);
				if (window.Equals (GlfwWindowPtr.Null)) { // Does this line actually work???
					Console.Error.WriteLine ("ERROR: Failed to create GlFW window, shutting down");
					Environment.Exit (1);
				}

				Glfw.MakeContextCurrent (window);
				Console.WriteLine ("render:" + GL.GetString (StringName.Renderer));
				Console.WriteLine ("version:" + GL.GetString (StringName.Version));

				GL.Enable (EnableCap.DepthTest);
				GL.DepthFunc (DepthFunction.Less);

				float[] points = {
					-1.0f, 1.0f, -0.1f,
					-1.0f, -1.0f, -0.1f,
					1.0f, -1.0f, -0.1f ,
					1.0f, 1.0f, -0.1f,
					-1.0f, 1.0f, -0.1f,
					1.0f, -1.0f, -0.1f
				};
				var vbo = GL.GenBuffer ();
				GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

				GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Marshal.SizeOf(typeof(float))
				                                                   *points.Length), points, BufferUsageHint.StaticDraw);

				var vao = GL.GenVertexArray();
				GL.BindVertexArray(vao);
				GL.EnableVertexAttribArray(0);

				GL.BindBuffer (BufferTarget.ArrayBuffer, vbo);
				GL.VertexAttribPointer (0, 3, VertexAttribPointerType.Float, false, 0, IntPtr.Zero);
				string vertex_shader = "#version 400\nin vec3 vp;\n" +
					"uniform float iGlobalTime;\n"+
						"uniform vec2 iResolution;\n"+
						"void main () {"+
						//"  gl_Position = vec4 (vp, 1.0);"+
						"gl_Position = vec4 (vp, 1.0);"+
						"};";
				string fragment_shader = "#version 400\nout vec4 frag_colour;\n"+
					"uniform float iGlobalTime;\n"+
						"uniform vec2 iResolution;\n"+
					"void main () {"+
						"  frag_colour = vec4 (0.5, 0.0, 0.5, 1.0);"+
						"}";;

				var vs = GL.CreateShader (ShaderType.VertexShader);
				GL.ShaderSource (vs, vertex_shader);
				GL.CompileShader (vs);
				Console.WriteLine(GL.GetShaderInfoLog (vs));
				var fs = GL.CreateShader (ShaderType.FragmentShader);
				GL.ShaderSource (fs, fragment_shader);
				GL.CompileShader (fs);
				Console.WriteLine (GL.GetShaderInfoLog (fs));

				var combinedPrograms = GL.CreateProgram ();
				GL.AttachShader (combinedPrograms,fs);
				GL.AttachShader (combinedPrograms, vs);
				GL.LinkProgram (combinedPrograms);
				GL.ValidateProgram (combinedPrograms);

				var IGlobalTime = GL.GetUniformLocation(combinedPrograms, "iGlobalTime");
				GL.Uniform1(IGlobalTime, (float)DateTime.Now.Millisecond);

				var iResolustion = GL.GetUniformLocation(combinedPrograms, "iResolution");
				GL.Uniform2(iResolustion,new Pencil.Gaming.MathUtils.Vector2(800, 600));

				GL.UseProgram (combinedPrograms);

				while(!Glfw.WindowShouldClose(window))
				{
					switch(state)
					{
					case 0:
						break;
					case 1:
						GL.Clear (ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

						GL.ClearColor (1.0f, 0.0f, 0.0f, 1.0f);
						//GL.UseProgram (combinedPrograms);
						//GL.BindVertexArray (vao);
						//GL.DrawArrays (BeginMode.Triangles, 0, verticesF.Length/3);
						GL.UseProgram (combinedPrograms);

						IGlobalTime = GL.GetUniformLocation(combinedPrograms, "iGlobalTime");
						GL.Uniform1(IGlobalTime, (float)DateTime.Now.Millisecond);

						GL.BindVertexArray (vao);
						GL.DrawArrays (BeginMode.Triangles, 0, points.Length/3);
						Glfw.SwapBuffers (window);
						Glfw.PollEvents();
						break;
					case 2:
						GL.Clear (ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

						GL.ClearColor (1.0f, 0.0f, 0.0f, 1.0f);
						//GL.UseProgram (combinedPrograms);
						//GL.BindVertexArray (vao);
						//GL.DrawArrays (BeginMode.Triangles, 0, verticesF.Length/3);
						GL.UseProgram (combinedPrograms);

						GL.BindVertexArray (vao);
						GL.DrawArrays (BeginMode.Triangles, 0, points.Length/3);
						Glfw.SwapBuffers (window);
						Glfw.PollEvents();
						break;
					case 3:
						var vs1 = GL.CreateShader (ShaderType.VertexShader);
						GL.ShaderSource (vs1, vertex);
						GL.CompileShader (vs1);
						Console.WriteLine(GL.GetShaderInfoLog (vs1));
						var fs1 = GL.CreateShader (ShaderType.FragmentShader);
						GL.ShaderSource (fs1, fragment);
						GL.CompileShader (fs1);
						Console.WriteLine (GL.GetShaderInfoLog (fs1));

						var combinedPrograms1 = GL.CreateProgram ();
						GL.AttachShader (combinedPrograms1,fs1);
						GL.AttachShader (combinedPrograms1, vs1);
						GL.LinkProgram (combinedPrograms1);
						GL.ValidateProgram (combinedPrograms1);

						IGlobalTime = GL.GetUniformLocation(combinedPrograms1, "iGlobalTime");
						GL.Uniform1(IGlobalTime, (float)DateTime.Now.Millisecond);

						iResolustion = GL.GetUniformLocation(combinedPrograms, "iResolution");
						GL.Uniform2(iResolustion, new Pencil.Gaming.MathUtils.Vector2(800, 600));

						GL.DeleteProgram(combinedPrograms);

						combinedPrograms = combinedPrograms1;
						state = 1;
						break;
					}
				}
			});
			x.IsBackground = true;
			x.Start ();
		}

		public override void Pause ()
		{
			state = 2;
			base.Pause ();
		}

		public override void Start ()
		{
			state = 1;
		}

		public override void Stop ()
		{
			state = 0;
			base.Stop ();
		}
		public override string StartingVertexShader ()
		{
			return  "#version 400\nin vec3 vp;\n" +
				"uniform float iGlobalTime;\n"+
				"uniform vec2 iResolution;\n"+
				"void main () {"+
					//"  gl_Position = vec4 (vp, 1.0);"+
					"gl_Position = vec4 (vp, 1.0);"+
					"};";
		}
		public override string StartingFragmentShader ()
		{
			return "#version 400\nout vec4 frag_colour;\n" +
				"uniform float iGlobalTime;\n"+
					"uniform vec2 iResolution;\n"+
			"void main () {" +
			"  frag_colour = vec4 (0.5, 0.0, 0.5, 1.0);" +
			"}";
		}
		public override void setShaders (string vertex, string fragment)
		{
			if(ShaderReady)
			{
				ExampleShader.vertex = vertex;
				ExampleShader.fragment = fragment;
				state = 3;
			}

		}
	}
}

