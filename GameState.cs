using System;
using Newtonsoft.Json;

namespace LBAL_ModEditor
{
	[Serializable]
	public class GameState
	{
		[JsonProperty(nameof(sandbox))]
		public bool sandbox { get; set; }

		[JsonProperty(nameof(coins))]
		public int coins { get; set; }

		[JsonProperty(nameof(reroll_tokens))]
		public int reroll_tokens { get; set; }

		[JsonProperty(nameof(removal_tokens))]
		public int removal_tokens { get; set; }

		[JsonProperty(nameof(essence_tokens))]
		public int essence_tokens { get; set; }

		[JsonProperty(nameof(items))]
		public object[] items { get; set; }

	}
}