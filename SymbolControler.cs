using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace LBAL_ModEditor
{
    public class SymbolControler
	{
		private string _workingDirectory;
		private Symbol _symbol;

		public SymbolControler(string workingDirectory)
			: this(workingDirectory, null)
		{}

		public SymbolControler(string workingDirectory, Symbol symbol)
		{
			_workingDirectory = workingDirectory;
			_symbol = symbol;
		}

		public Symbol SelectSymbol()
		{
			string path = Path.Combine(_workingDirectory, "scripts");
			string[] files = Directory.GetFiles(path);
			List<string> menu = new List<string>();
			for (int i = 0; i < files.Length; i++)
			{
				menu.Add($"\t{i} - {files[i].Split(Path.DirectorySeparatorChar).Last()}");
			}

			Console.WriteLine($"\n\t~ Choose Symbol ~");
			int choice = Input.Instance.GetChoiceInt(menu, "\tEdit: ");

			return new Symbol(files[choice].Split(Path.DirectorySeparatorChar).Last());
		}

		public void CreateSymbol()
		{

		}

		public void EditSymbol()
		{
			Console.WriteLine($"\n\t~ Choose Property ~");

			List<string> propertyMenu = GetPropertyMenu();
			int choice = Input.Instance.GetChoiceInt(propertyMenu, "\tEdit: ");
			string editingPropery = _symbol.Properties.Keys.ToArray()[choice];

			Console.WriteLine($"\n\t~ Editing '{_symbol.type}' - '{editingPropery}' ~");
			Console.WriteLine($"\tOld Value: {_symbol.Properties[editingPropery]}");
			Console.Write("\tNew Value: ");
			string newValue = Input.Instance.GetInput();

			_symbol.Properties[editingPropery] = newValue;
			Save();
		}

		private List<string> GetPropertyMenu()
		{
			List<string> propertyOptions = _symbol.Properties.Keys.ToList();
			List<string> propertyMenu = new List<string>();
			for (int i = 0; i < propertyOptions.Count; i++)
			{
				string propertyName = propertyOptions[i];
				string description = _symbol.Properties[propertyName].ToString();
				description = description.Length <= 50 ? description : description.Substring(0, 45) + " ...";
				propertyMenu.Add($"\t{i} - {propertyName} (\"{description}\")");
			}
			return propertyMenu;
		}

		private void Save()
		{
			string path = Path.Combine(_workingDirectory, "scripts", $"{_symbol.type}.gd");
			File.WriteAllText(path, _symbol.ToGdFileFormat());
		}

	}
}