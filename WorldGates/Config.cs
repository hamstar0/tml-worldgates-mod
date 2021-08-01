using System;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace WorldGates {
	public partial class WorldGatesConfig : ModConfig {
		public static WorldGatesConfig Instance => ModContent.GetInstance<WorldGatesConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////
		
		[DefaultValue( true )]
		public bool StartNewPlayersWithPBG { get; set; } = true;
	}
}
