using System;
using Gtk;
using System.Reflection;
using ShaderCrateLib;
using System.Collections.Generic;

public partial class MainWindow: Gtk.Window
{
	ShaderProgram shaderProg;
	//needs updated to be dynamic
	String SaveDirectory = "/home/marsh/Documents/Github/ShaderBox/ShaderBox/ExampleShader/Shaders";
	string saveName = "";
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		this.KeyPressEvent += Compile;
		this.textviewVertex.Buffer.Changed += doOnTextChangedVertex;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	void Compile(object sender, KeyPressEventArgs e)
	{
		if(e.Event.Key == Gdk.Key.F8)
		{
			Console.WriteLine ("kb shorcut compiling");
			compileShadersM ();
		}

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
			if(fileName.EndsWith(".dll"))
			{
				Assembly asm = Assembly.LoadFile (fileName);
				//var enumer = asm.GetType().GetEnumerator();
				//enumer.MoveNext();
				//Console.WriteLine(enumer.Current);
				shaderProg = (ShaderProgram)asm.CreateInstance(asm.GetTypes()[0].ToString());

//				shaderProg.Start();
//				shaderProg.Stop();
				shaderProg.LoadSuccessful();
				shaderProg.OneTimeSetup();
				textviewVertex.Buffer.Text = shaderProg.StartingVertexShader();
				textviewFragment.Buffer.Text = shaderProg.StartingFragmentShader();
				label1.Text = "success";
			}
			else if(fileName.EndsWith("vertexShader.glsl") && shaderProg != null)
			{
				var vertexProg = System.IO.File.ReadAllText(fileName);
				textviewVertex.Buffer.Text = vertexProg;
				shaderProg.setVertexShader(vertexProg);
			}
			else if(fileName.EndsWith("fragmentShader.glsl") && shaderProg != null)
			{
				var fragmentProg = System.IO.File.ReadAllText(fileName);
				textviewFragment.Buffer.Text = fragmentProg;
				shaderProg.setFragmentShader(fragmentProg);
			}
			else if(!fileName.Contains("."))
			{
				SaveDirectory = fileName;
			}
		}
		catch(Exception e0)
		{
			Console.WriteLine(e0.ToString()); 
		}
	}

	protected void OnCompileShadersClicked (object sender, EventArgs e)
	{
		compileShadersM ();
	}
//	void setSyntaxHighlighting(TextBuffer buffer)
//	{
//		var blue = new TextTag ();
//		blue.ForegroundGdk = new Gdk.Color (0, 0, 255);
//		buffer.ApplyTag (blue, buffer.StartIter, buffer.EndIter);
//	}

	void compileShadersM()
	{
		label1.Text = shaderProg.compileShaders (textviewVertex.Buffer.Text, textviewFragment.Buffer.Text);
		//decide when to apply shaders
		shaderProg.setShaders (textviewVertex.Buffer.Text, textviewFragment.Buffer.Text);
	}
	protected void OnSaveAsClicked (object sender, EventArgs e)
	{
		System.IO.File.WriteAllText (SaveDirectory + "/" + saveName +"vertexShader.glsl", shaderProg.getVertexShader ());
		System.IO.File.WriteAllText (SaveDirectory + "/" + saveName +"fragmentShader.glsl", shaderProg.getFragmentShader ());

	}

	protected void OnFilePathChanged (object sender, EventArgs e)
	{
		Console.WriteLine (FilePath.Text);
		saveName = FilePath.Text;
	}

	void doOnTextChangedVertex(object sender, EventArgs e)
	{
		Console.WriteLine ("do on text changed");
		TextTag tag = new TextTag ("blue");
		tag.Foreground = "0 0 255";
		textviewVertex.Buffer.ApplyTag ("blue", textviewVertex.Buffer.StartIter, textviewVertex.Buffer.EndIter);
	}
}
