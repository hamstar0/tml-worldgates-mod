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
		
		[DefaultValue( true )]
		public bool AlertAboutGates { get; set; } = true;
		
		[DefaultValue( true )]
		public bool WarnAboutGatesAndPBG { get; set; } = true;
	}
}
