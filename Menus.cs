using System;
using System.Collections.Generic;

namespace LBAL_ModEditor
{
	public static class Menus
	{
		public static List<string> Main = new List<string>
		{
			"\n\n~~~~ Menu ~~~~",
			"* - Exit",
			"42 - Settings",
			"0 - Toggle debugging",
			"1 - Edit Debugging Grid",
			"2 - Create Symbol",
			"3 - Edit Symbol",
			"99 - Capture Input"
		};

		public static List<string> EditGrid = new List<string>{
				"\n---- Edit Menu ----",
				"* - Exit",
				"(x,y)[SYMBOL_NAME] - Override symbol at position (x,y)",
				"clear - Make Entire Grid Empty",
				"coin - Make Entire Grid Coin",
				"save - Save and Exit",
			};
	}
}