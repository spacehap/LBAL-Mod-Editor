using System;
using Newtonsoft.Json;

namespace LBAL_ModEditor
{
	public class Symbol
	{
		public string mod_type { get => "symbol"; }
		public string type { get; set; }
		public string display_name { get; set; }
		public int value { get; set; }
		public string description { get; set; }
		public string rarity { get; set; }
		public string[] groups { get; set; }
		public string[] effects { get; set; }

		public Symbol(string fileName)
		{
			// GDScript MyGDScript = (GDScript) GD.Load("res://path_to_gd_file.gd");
			// Object myGDScriptNode = (Godot.Object) MyGDScript.New(); // This is a Godot.Object
		}
	}
}