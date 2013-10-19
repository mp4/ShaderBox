using System;

namespace ShaderBoxLib
{
	public class ShaderProgram
	{
		public ShaderProgram ()
		{
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
	}
}

