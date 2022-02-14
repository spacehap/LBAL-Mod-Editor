using System;
using System.IO;
using System.Collections.Generic;

namespace LBAL_ModEditor
{
    public class Input
    {
		private static Input _instance;
		public static Input Instance 
		{
			get
			{
				if (_instance == null)
					_instance = new Input();

				return _instance;
			}
		}

		private Input() {}

		private bool _capturingInput = false;
		private List<string> _debuggingInputs = new List<string> { "3", "0" };
		private int _intputCount = 0;


		public string GetInput(string inputMessage = "Input: ")
		{
			Console.Out.Flush();
			Console.Write(inputMessage);
			
			bool hasDebugInputs = _intputCount < _debuggingInputs.Count;
			string inputStr = Program.IsDebugging && hasDebugInputs ? _debuggingInputs[_intputCount] : Console.ReadLine();
			_intputCount++;

			if (_capturingInput)
				_debuggingInputs.Add(inputStr);

			return inputStr;
		}

		public string GetChoiceString(List<string> menuDisplay, string inputMessage = "Input: ")
		{
			foreach (var item in menuDisplay)
			{
				Console.WriteLine(item);
			}
			return GetInput();
		}

		public int GetChoiceInt(List<string> menuDisplay, string inputMessage = "Input: ")
		{
			return int.Parse(GetChoiceString(menuDisplay, inputMessage));
		}

	}
}