using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


namespace LBAL_ModEditor
{
	class Program
	{
		private static int _intputCount = 0;
		private const int _maxNameLength = 15; 
		private const int _maxLoopCount = 10;
		private const string _debuggerFilePath = @"C:\Users\thepa\AppData\Roaming\Godot\app_userdata\Luck be a Landlord\LBAL-Sandbox-Data.save";
			

		static void Main(string[] args)
		{
			int loopCountdown = _maxLoopCount;
			bool shouldContinue = true;
			int input = -1;
			do 
			{
				loopCountdown--;
				Console.Out.Flush();

				switch(input)
				{
					case -1:
						Console.WriteLine("Welcome to the Luck be a Landlord Mod Editor");
						break;
					case 0:
						bool sandboxVal = ToggleDebugging();
						Console.WriteLine($"~~~~ Debugging Toggled to {sandboxVal} ~~~~");
						break;
					case 1:
						Console.WriteLine("~~~~ Edit Debugging Grid ~~~~");
						EditDebuggingGrid();
						break;
					case 2:
						Console.WriteLine("~~~~ Create Symbol ~~~~");
						EditDebuggingGrid();
						break;
					case 3:
						Console.WriteLine("~~~~ Edit Symbol ~~~~");
						EditDebuggingGrid();
						break;
					default:
						Console.WriteLine("Not Implemented");
						break;
				}
				
				
				Console.WriteLine("~~~~ Menu ~~~~");
				Console.WriteLine("* - Exit");
				Console.WriteLine("0 - Toggle debugging");
				Console.WriteLine("1 - Edit Debugging Grid");
				Console.WriteLine("2 - Create Symbol");
				Console.WriteLine("3 - Edit Symbol");
							
				Console.Write("Choose Operation: ");
				string inputStr = GetInput();
				shouldContinue = int.TryParse(inputStr, out input);
				Console.WriteLine("\n" + input.ToString());

			} while(shouldContinue && loopCountdown > 0);

			if(loopCountdown == 0)
			{
				Console.WriteLine("!!Max Loop Count Exceeded!!!");
			}
		}
		
		private static string GetInput()
		{
			string[] inputs = new string[] {"1", "(0,1)overridenSymbol"};
			string inputStr = _intputCount<inputs.Length ? inputs[_intputCount] : null;
			inputStr = inputStr??Console.ReadLine();
			_intputCount++;
			return inputStr;
		}

		private static bool ToggleDebugging()
		{
			GameState gameState = GetGameState();
			gameState.sandbox = !gameState.sandbox;
			Save(gameState);
			return gameState.sandbox;
		}

		private static void EditDebuggingGrid()
		{
			GridRow[] rows = GetDebugRows();
			bool shouldContinue = true;

			do {
				Console.WriteLine();
				PrintSymbolGrid(rows);
				
				Console.WriteLine("\n---- Edit Menu ----");
				Console.WriteLine("* - Exit");
				Console.WriteLine("(x,y)[SYMBOL_NAME] - Override symbol at position (x,y)");
				Console.WriteLine("clear - Make Entire Grid Empty");
				Console.WriteLine("coin - Make Entire Grid Coin");
				Console.WriteLine("save - Save and Exit");

				Console.Write("Input: ");
				string inputStr = GetInput();

				const string overridePattern = "\\([0-4],[0-4]\\)\\w.*";
				Regex overrideRegex = new Regex(overridePattern);
				int x=0,y=0;
				string newSymbolName = string.Empty;
				if(overrideRegex.IsMatch(inputStr))
				{
					x = int.Parse(inputStr.Substring(1,1));
					y = int.Parse(inputStr.Substring(3,1));
					newSymbolName = inputStr.Substring(5, inputStr.Length-5);
					
					inputStr = "override";
				}

				switch (inputStr)
				{
					case "override":
						Console.WriteLine($"x:{x}, y:{y}, name:{newSymbolName}");
						rows[x].Symbols[y] = newSymbolName;
						break;
					case "clear":
					case "c":
					case "clr":
						SetAllSymbols(rows, "empty");
						break;
					case "coin":
						SetAllSymbols(rows, "coin");
						break;
					case "save":
					case "s":
						Save(rows);
						shouldContinue = false;
						break;
					case "*":
						shouldContinue = false;
						break;
					default:
						break;
				}

			} while(shouldContinue);
			
		}
		
		private static void SetAllSymbols(GridRow[] rows, string symbol)
		{
			for (int x = 0; x < rows.Length; x++)
			{
				for (int y = 0; y < rows.Length; y++)
				{
					string item = rows[x].Symbols[y] = symbol;
				}
			}
		}
		
		private static void PrintSymbolGrid(GridRow[] rows)
		{
			for (int x = 0; x < rows.Length; x++)
			{
				List<string> formattedItems =  new List<string>();
				for (int y = 0; y < rows.Length; y++)
				{
					string item = rows[x].Symbols[y];
					string tmpFmtdItem = item.Substring(0, item.Length>_maxNameLength ? _maxNameLength : item.Length)
												.PadRight(_maxNameLength+5);
					formattedItems.Add($"({x},{y}) {tmpFmtdItem}");
				}
				Console.WriteLine(string.Join("|\t", formattedItems));
			}
		}

		private static void CreateSymbol()
		{

		}

		private static GameState GetGameState()
		{
			string[] debuggerFileLines = File.ReadAllLines(_debuggerFilePath);
			return JsonConvert.DeserializeObject<GameState>(debuggerFileLines[0]);
		}

		private static GridRow[] GetDebugRows()
		{
			GridRow[] rows = new GridRow[4];
			string[] debuggerFileLines = File.ReadAllLines(_debuggerFilePath);
			for (int i = 1; i < debuggerFileLines.Length; i++)
			{
				rows[i-1] = JsonConvert.DeserializeObject<GridRow>(debuggerFileLines[i]);
			}
			return rows;
		}

		private static void Save(GameState gameState)
		{
			string[] debuggerFileLines = File.ReadAllLines(_debuggerFilePath);
			debuggerFileLines[0] = JsonConvert.SerializeObject(gameState);
			File.Delete(_debuggerFilePath);
			File.WriteAllLines(_debuggerFilePath, debuggerFileLines);
		}

		private static void Save(GridRow[] rows)
		{
			string[] debuggerFileLines = File.ReadAllLines(_debuggerFilePath);
			for (int i = 1; i < rows.Length; i++)
			{
				debuggerFileLines[i] = rows[i-1].Serialize(i);
			}
			File.Delete(_debuggerFilePath);
			File.WriteAllLines(_debuggerFilePath, debuggerFileLines);
		}
	}
}
