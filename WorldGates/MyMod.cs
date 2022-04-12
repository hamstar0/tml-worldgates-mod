using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers.BarrierTypes;


namespace WorldGates {
	public partial class WorldGatesMod : Mod {
		public static string GithubUserName => "hamstar0";

		public static string GithubProjectName => "tml-worldgates-mod";


		////////////////

		public static WorldGatesMod Instance => ModContent.GetInstance<WorldGatesMod>();



		////////////////

		private bool HasAlertedToGates = false;


		////////////////

		public bool IsTricksterModLoaded { get; private set; } = false;



		////////////////

		public override void Load() {
			this.IsTricksterModLoaded = ModLoader.GetMod( "TheTrickster" ) != null;
		}


		////////////////

		public override void PostUpdateEverything() {
			if( this.HasAlertedToGates ) {
				return;
			}

			if( this.IsNearWorldGate(out Barrier barrier) ) {
				this.AlertForGateProximity( barrier );

				this.HasAlertedToGates = true;
			}
		}
	}
}