using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace LBAL_ModEditor
{
	class Program
	{
		private const int _maxLoopCount = 10;
		
		private static bool _amDebugging = false;
		private static bool _capturingInput = false;
		private static List<string> _debuggingInputs = new List<string> { "3", "0" };
		private static int _intputCount = 0;


		private static readonly string _debuggerFilePath = Path.Combine(GetLbalDirectory(), DebugController.FILENAME);
		private static readonly string _modsDirectory = Path.Combine(GetLbalDirectory(), "mods");
		public static string WorkingDirectory = Path.Combine(_modsDirectory, Directory.EnumerateDirectories(_modsDirectory).First());


		static void Main(string[] args)
		{
			int loopCountdown = _maxLoopCount;
			bool shouldContinue = true;
			int input = -1;
			do 
			{
				loopCountdown--;
				Console.Out.Flush();

				SymbolControler symbolCtlr = new SymbolControler(WorkingDirectory);
				DebugController debugCtlr = new DebugController(_debuggerFilePath);

				Console.WriteLine();
				switch (input)
				{
					case -1:
						if (!_amDebugging) { Console.Clear(); }
						Console.WriteLine("*~~~~* Welcome to the Luck be a Landlord Mod Editor *~~~~*");
						Console.WriteLine($"- Current working with ... {WorkingDirectory.Split(Path.DirectorySeparatorChar).Last()}");
						break;
					case 42:
						Console.WriteLine("~~~~ Settings ~~~~");
						// TODO: ChangeWorkingDirectory();
						break;
					case 0:
						bool sandboxVal = debugCtlr.ToggleDebugging();
						Console.WriteLine($"~~~~ Debugging Toggled to {null} ~~~~");
						break;
					case 1:
						Console.WriteLine("~~~~ Edit Debugging Grid ~~~~");
						debugCtlr.EditDebuggingGrid();
						break;
					case 2:
						Console.WriteLine("~~~~ Create Symbol ~~~~");
						symbolCtlr.CreateSymbol();
						break;
					case 3:
						Console.WriteLine("~~~~ Edit Symbol ~~~~");
						symbolCtlr.SelectSymbol();
						symbolCtlr.EditSymbol();
						break;
					case 99:
						Console.WriteLine("~~~~ Capturing Input ~~~~");
						_debuggingInputs.Clear();
						_capturingInput = true;
						break;
					default:
						Console.WriteLine("Not Implemented");
						break;
				}
				
				string inputStr = Input.Instance.GetChoiceString(Menus.Main, "Choose Operation: ");
				shouldContinue = int.TryParse(inputStr, out input);

			} while (shouldContinue && loopCountdown > 0);

			if(loopCountdown == 0)
			{
				Console.WriteLine("!!Max Loop Count Exceeded!!!");
			}
		}

		private static string GetLbalDirectory()
		{
			string directoryPart = @"AppData\Roaming\Godot\app_userdata\Luck be a Landlord";
			string userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			return Path.Combine(userDir, directoryPart);
		}

	}
}
