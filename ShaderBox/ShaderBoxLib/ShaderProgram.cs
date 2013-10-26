using System;
using Pencil.Gaming;
using Pencil.Gaming.Graphics;

namespace ShaderCrateLib
{
	public abstract class ShaderProgram
	{
		bool shaderReady = true;

		public ShaderProgram ()
		{
		}

		public bool ShaderReady {
			get {
				return shaderReady;
			}
		}

		public virtual void Start()
		{
			Console.WriteLine ("start called");
		}
		public virtual void Stop()
		{
			Console.WriteLine ("Stop called");
		}
		public virtual void Pause()
		{
			Console.WriteLine ("Pause called");
		}
		public void LoadSuccessful()
		{
			Console.WriteLine ("loading successful");
		}
		public virtual void OneTimeSetup()
		{
			Console.WriteLine ("one time setup called");
		}
		public virtual string StartingVertexShader()
		{
			return "";
		}
		public virtual string StartingFragmentShader ()
		{
			return "";
		}
		public string compileShaders(string vertex, string fragment)
		{
			if(!Glfw.Init ())
			{
				return "problem initializing glfw";
			}
			Glfw.SetErrorCallback ((code, des) => {
				Console.Error.WriteLine ("ERROR ({0}): {1}", code, des);
			});
			Glfw.WindowHint (WindowHint.ContextVersionMajor, 4);
			Glfw.WindowHint (WindowHint.ContextVersionMinor, 0);
			Glfw.WindowHint (WindowHint.OpenGLForwardCompat, 1);

			GlfwWindowPtr window = Glfw.CreateWindow(800, 600, "GlFW 3 Window", GlfwMonitorPtr.Null, GlfwWindowPtr.Null);
			if (window.Equals(GlfwWindowPtr.Null)) { // Does this line actually work???
				Console.Error.WriteLine("ERROR: Failed to create GlFW window, shutting down");
				Environment.Exit(1);
			}

			Glfw.MakeContextCurrent(window);
			string output = "";
			var vs = GL.CreateShader (ShaderType.VertexShader);
			GL.ShaderSource (vs, vertex);
			GL.CompileShader (vs);
			output += (GL.GetShaderInfoLog (vs));
			var fs = GL.CreateShader (ShaderType.FragmentShader);
			GL.ShaderSource (fs, fragment);
			GL.CompileShader (fs);
			output += (GL.GetShaderInfoLog (fs));

			var combinedPrograms = GL.CreateProgram ();
			GL.AttachShader (combinedPrograms,fs);
			GL.AttachShader (combinedPrograms, vs);
			GL.LinkProgram (combinedPrograms);
			GL.ValidateProgram (combinedPrograms);
//			int success;
//			GL.GetProgram (combinedPrograms, ProgramParameter.ValidateStatus, out success);
//			if(success == 0)
//			{
//				output += "validation failed";
//				shaderReady = false;
//			}
//			else
//			{
//				shaderReady = true;
//				output += "validation succeeded";
			GL.DeleteProgram (combinedPrograms);
			Glfw.DestroyWindow (window);
//			}
			return output;
		}
		public virtual void setShaders(string vertex, string fragment)
		{

		}
		public abstract string getFragmentShader ();
		public abstract string getVertexShader ();
		public abstract void setFragmentShader (string frag);
		public abstract void setVertexShader (string vert);
	}
}

