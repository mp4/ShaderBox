using System;
using Gtk;
using System.Reflection;
using ShaderCrateLib;
using System.Collections.Generic;

public partial class MainWindow: Gtk.Window
{
	ShaderProgram shaderProg;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnFilechooserwidget1SelectionChanged (object sender, EventArgs e)
	{
		Console.WriteLine(((FileChooser)sender).CurrentFolder + ":sC");
	}
	//stop
	protected void OnButton3Clicked (object sender, EventArgs e)
	{
		shaderProg.Stop ();
	}
	//pause
	protected void OnButton2Clicked (object sender, EventArgs e)
	{
		shaderProg.Pause ();
	}
	//play
	protected void OnButton1Clicked (object sender, EventArgs e)
	{
		shaderProg.Start ();
	}

	protected void OnFilechooserwidget1FileActivated (object sender, EventArgs e)
	{
		Console.WriteLine(((FileChooser)sender).CurrentFolder +":fA");
	}

	protected void OnFilechooserwidget1CurrentFolderChanged (object sender, EventArgs e)
	{
		Console.WriteLine(((FileChooser)sender).CurrentFolder + ":fC");
	}

	protected void OnFilechooserwidget1UpdatePreview (object sender, EventArgs e)
	{
		Console.WriteLine(((FileChooser)sender).Filename);
		string fileName = ((FileChooser)sender).Filename;
		try
		{
			Assembly asm = Assembly.LoadFile (fileName);
			if(fileName.EndsWith(".dll"))
			{
				var enumer = asm.DefinedTypes.GetEnumerator();
				enumer.MoveNext();
				Console.WriteLine(enumer.Current);
				shaderProg = (ShaderProgram)asm.CreateInstance(enumer.Current.ToString());

//				shaderProg.Start();
//				shaderProg.Stop();
				shaderProg.LoadSuccessful();
				shaderProg.OneTimeSetup();
				textviewVertex.Buffer.Text = shaderProg.StartingVertexShader();
				textviewFragment.Buffer.Text = shaderProg.StartingFragmentShader();
				label1.Text = "success";
			}
		}
		catch(Exception e0)
		{
			Console.WriteLine(e0.ToString()); 
		}
	}

	protected void OnCompileShadersClicked (object sender, EventArgs e)
	{
		label1.Text = shaderProg.compileShaders (textviewVertex.Buffer.Text, textviewFragment.Buffer.Text);
		//decide when to apply shaders
		shaderProg.setShaders (textviewVertex.Buffer.Text, textviewFragment.Buffer.Text);
	}
//	void setSyntaxHighlighting(TextBuffer buffer)
//	{
//		var blue = new TextTag ();
//		blue.ForegroundGdk = new Gdk.Color (0, 0, 255);
//		buffer.ApplyTag (blue, buffer.StartIter, buffer.EndIter);
//	}
}
