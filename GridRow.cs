using System;
using Newtonsoft.Json;

namespace LBAL_ModEditor
{
	[Serializable]
	public class GridRow
	{
		public string[] Symbols { get; set; }
		
		[JsonProperty(nameof(symbols1))]
		public string[] symbols1 
		{ 
			get => Symbols;
			set => Symbols = value;
		 }

		[JsonProperty(nameof(symbols2))]
		public string[] symbols2 
		{ 
			get => Symbols;
			set => Symbols = value;
		 }

		[JsonProperty(nameof(symbols3))]
		public string[] symbols3 
		{ 
			get => Symbols;
			set => Symbols = value;
		 }

		[JsonProperty(nameof(symbols4))]
		public string[] symbols4 
		{ 
			get => Symbols;
			set => Symbols = value;
		}

		public string Serialize(int row = 1)
		{
			string str = JsonConvert.SerializeObject(Symbols);
			return $"{{\"symbols1\": {str}}}";
		}
	}
}