using System;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;
using System.Threading;

namespace ExampleShader
{
	public class ExampleShader : ShaderBoxLib.ShaderProgram
	{
		public ExampleShader ()
		{
		}
		public static volatile int state = 0;

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
						Glfw.SwapBuffers (window);
						Glfw.PollEvents();
						break;
					case 2:
						GL.Clear (ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

						GL.ClearColor (0.0f, 1.0f, 0.0f, 1.0f);
						//GL.UseProgram (combinedPrograms);
						//GL.BindVertexArray (vao);
						//GL.DrawArrays (BeginMode.Triangles, 0, verticesF.Length/3);
						Glfw.SwapBuffers (window);
						Glfw.PollEvents();
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

	}
}

