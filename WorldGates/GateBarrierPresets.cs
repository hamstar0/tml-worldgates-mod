using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers.BarrierTypes;


namespace WorldGates {
	public partial class GateBarrierPresets : ILoadable {
		public static GateBarrierPresets Instance => ModContent.GetInstance<GateBarrierPresets>();



		////////////////

		public readonly string DungeonGateName = "DungeonGate";
		public readonly string JungleGateName = "JungleGate";
		public readonly string RockLayerGateName = "RockLayerGate";
		public readonly string LavaLayerGateName = "LavaLayerGate";
		public readonly string UnderworldGateName = "UnderworldGate";
		public readonly string SkyGateName = "SkyGate";

		public IDictionary<string, Barrier> WorldGates { get; } = new Dictionary<string, Barrier>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }
	}
}