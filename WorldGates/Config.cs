﻿using System;
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

		////

		[Range( 0, Int32.MaxValue )]
		[DefaultValue( 20 )]
		public int DungeonGateHp { get; set; } = 20;

		[Range( 0, Int32.MaxValue )]
		[DefaultValue( 25 )]
		public int JungleGateHp { get; set; } = 25;

		[Range( 0, Int32.MaxValue )]
		[DefaultValue( 30 )]
		public int RockLayerGateHp { get; set; } = 30;

		[Range( 0, Int32.MaxValue )]
		[DefaultValue( 40 )]
		public int LavaLayerGateHp { get; set; } = 40;

		[Range( 0, Int32.MaxValue )]
		[DefaultValue( 75 )]
		public int UnderworldGateHp { get; set; } = 75;

		[Range( 0, Int32.MaxValue )]
		[DefaultValue( 125 )]
		public int SkyGateHp { get; set; } = 125;
	}
}
