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

		public Barrier DungeonGate { get; private set; }
		public Barrier JungleGate { get; private set; }
		public Barrier RockLayerGate { get; private set; }
		public Barrier LavaLayerGate { get; private set; }
		public Barrier UnderworldGate { get; private set; }
		public Barrier SkyGate { get; private set; }

		public IList<Barrier> WorldGates { get; private set; } = new List<Barrier>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnModsUnload() { }

		void ILoadable.OnPostModsLoad() { }
	}
}