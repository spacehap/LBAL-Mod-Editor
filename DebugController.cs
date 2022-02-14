using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace LBAL_ModEditor
{
    public class DebugController
	{
		public const string FILENAME = "LBAL-Sandbox-Data.save";
		public GameState GameState { get; set; }
		public GridRow[] Rows { get; set; }

		private string _filePath;
		private const int _maxNameLength = 15;



		public DebugController(string filePath)
		{
			_filePath = filePath;
			LoadGameState();
			LoadDebugRows();
		}

		public DebugController(GameState gameState, GridRow[] rows)
		{
			GameState = gameState;
			Rows = rows;
		}


		private void LoadGameState()
		{
			string[] debuggerFileLines = File.ReadAllLines(_filePath);
			GameState = JsonConvert.DeserializeObject<GameState>(debuggerFileLines[0]);
		}

		private void LoadDebugRows()
		{
			Rows = new GridRow[4];
			string[] debuggerFileLines = File.ReadAllLines(_filePath);
			for (int i = 1; i < debuggerFileLines.Length; i++)
			{
				Rows[i - 1] = JsonConvert.DeserializeObject<GridRow>(debuggerFileLines[i]);
			}
		}


		public bool ToggleDebugging()
		{
			GameState.sandbox = !GameState.sandbox;
			Save(GameState);
			return GameState.sandbox;
		}


		public void EditDebuggingGrid()
		{
			bool shouldContinue = true;

			do
			{
				Console.WriteLine();
				PrintSymbolGrid(Rows);

				string inputStr = Input.Instance.GetChoiceString(Menus.EditGrid);

				const string overridePattern = "\\([0-4],[0-4]\\)\\w.*";
				Regex overrideRegex = new Regex(overridePattern);
				int x = 0, y = 0;
				string newSymbolName = string.Empty;
				if (overrideRegex.IsMatch(inputStr))
				{
					x = int.Parse(inputStr.Substring(1, 1));
					y = int.Parse(inputStr.Substring(3, 1));
					newSymbolName = inputStr.Substring(5, inputStr.Length - 5);

					inputStr = "override";
				}

				switch (inputStr)
				{
					case "override":
						Console.WriteLine($"x:{x}, y:{y}, name:{newSymbolName}");
						Rows[x].Symbols[y] = newSymbolName;
						break;
					case "clear":
					case "c":
					case "clr":
						SetAllSymbols(Rows, "empty");
						break;
					case "coin":
						SetAllSymbols(Rows, "coin");
						break;
					case "save":
					case "s":
						Save(Rows);
						shouldContinue = false;
						break;
					case "*":
						shouldContinue = false;
						break;
					default:
						break;
				}

			} while (shouldContinue);

		}

		public void SetAllSymbols(GridRow[] rows, string symbol)
		{
			for (int x = 0; x < rows.Length; x++)
			{
				for (int y = 0; y < rows.Length; y++)
				{
					string item = rows[x].Symbols[y] = symbol;
				}
			}
		}


		private void PrintSymbolGrid(GridRow[] rows)
		{
			for (int x = 0; x < rows.Length; x++)
			{
				List<string> formattedItems = new List<string>();
				for (int y = 0; y < rows.Length; y++)
				{
					string item = rows[x].Symbols[y];
					string tmpFmtdItem = item.Substring(0, item.Length > _maxNameLength ? _maxNameLength : item.Length)
												.PadRight(_maxNameLength + 5);
					formattedItems.Add($"({x},{y}) {tmpFmtdItem}");
				}
				Console.WriteLine(string.Join("|\t", formattedItems));
			}
		}


		private void Save(GameState gameState)
		{
			string[] debuggerFileLines = File.ReadAllLines(_filePath);
			debuggerFileLines[0] = JsonConvert.SerializeObject(gameState);
			File.Delete(_filePath);
			File.WriteAllLines(_filePath, debuggerFileLines);
		}

		private void Save(GridRow[] rows)
		{
			string[] debuggerFileLines = File.ReadAllLines(_filePath);
			for (int i = 1; i < rows.Length; i++)
			{
				debuggerFileLines[i] = rows[i - 1].Serialize(i);
			}
			File.Delete(_filePath);
			File.WriteAllLines(_filePath, debuggerFileLines);
		}


	}
}