using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LBAL_ModEditor
{
	public class Symbol
	{
		public Dictionary<string, object> Properties = new Dictionary<string, object>();
		public string mod_type { get => (string)Properties[nameof(mod_type)]; }
		public string type { get => (string)Properties[nameof(type)]; }
		public string display_name { get => (string)Properties[nameof(display_name)]; }
		public int value { get => (int)Properties[nameof(value)]; }
		public string description { get => (string)Properties[nameof(description)]; }
		public string rarity { get => (string)Properties[nameof(mod_type)]; }
		public string[] groups { get; set; }
		public string[] effects { get; set; }

		public Symbol(string name)
		{
			if (!name.Contains(".gd"))
				string.Concat(name, ".gd");

			string path = Path.Combine(Program.WorkingDirectory, "scripts", name);
			if (File.Exists(path))
			{
				string[] lines = File.ReadAllLines(path);
				PopulateValues(lines);
			}
			else
			{
				Console.WriteLine("No file exists by that name in this directory");
			}
		}

		private void PopulateValues(string[] fileLines)
		{
			Properties.Clear();

			Properties.Add(nameof(mod_type), "symbol");

			foreach (string line in fileLines)
			{
				string tmp = line.Trim(' ', '\t');

				if(tmp.StartsWith(nameof(type)))
					Properties.Add(nameof(type), GetStringValue(tmp));
				if (tmp.StartsWith(nameof(display_name)))
					Properties.Add(nameof(display_name), GetStringValue(tmp));
				if (tmp.StartsWith(nameof(description)))
					Properties.Add(nameof(description), GetStringValue(tmp));
				if (tmp.StartsWith(nameof(rarity)))
					Properties.Add(nameof(rarity), GetStringValue(tmp));
				if (tmp.StartsWith(nameof(value)) && !tmp.Contains("values"))
					Properties.Add(nameof(value), GetIntValue(tmp));
				if (tmp.StartsWith(nameof(groups)))
					Properties.Add(nameof(groups), GetStringArray(tmp));
				if (tmp.StartsWith(nameof(effects)))
					Properties.Add(nameof(effects), GetStringArray(tmp));
			}
		}

		private string GetPropertyName(string line)
		{
			return line.Split("=").First().Trim('\"', ' ', '\t');
		}

		private string[] GetStringArray(string line)
		{
			List<string> valList = line.Split("=").Last().Trim('\"', ' ', '\t', '[', ']').Split(',').ToList();
			return valList.Select(x => x.Trim('\"', ' ', '\t')).ToArray();
		}

		private string GetStringValue(string line)
		{
			return line.Split("=").Last().Trim('\"', ' ', '\t');
		}

		private int GetIntValue(string line)
		{
			return int.Parse(line.Split("=").Last().Trim('\"', ' ', '\t'));
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		public string ToGdFileFormat()
		{
			List<string> lines = new List<string> { "extends \"res://Mod Data.gd\"", string.Empty, "func _init():" };

			foreach (KeyValuePair<string, object> item in Properties)
			{
				lines.Add($"\t{item.Key}=\"{item.Value.ToString()}\"");
			}

			return string.Join('\n', lines);
		}

	}
}