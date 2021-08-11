using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Services.Hooks.LoadHooks;
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

		public Barrier DungeonGate { get; private set; }
		public Barrier JungleGate { get; private set; }
		public Barrier RockLayerGate { get; private set; }
		public Barrier LavaLayerGate { get; private set; }
		public Barrier UnderworldGate { get; private set; }



		////////////////

		public override void PostSetupContent() {
			LoadHooks.AddPostWorldLoadEachHook( () => {
				this.InitializeGates();
			} );
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