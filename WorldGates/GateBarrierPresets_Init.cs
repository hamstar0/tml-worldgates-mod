using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers.BarrierTypes;


namespace WorldGates {
	public partial class GateBarrierPresets : ILoadable {
		public void InitializeAllGates(
					Rectangle dungeonArea,
					Rectangle jungleArea,
					Rectangle rockLayerArea,
					Rectangle lavaLayerArea,
					Rectangle underworldArea,
					Rectangle skyArea ) {
			var config = WorldGatesConfig.Instance;

			//
			
			this.DungeonGate = GateBarrier.CreateGateBarrier(
				id: "DungeonGate",
				strength: config.Get<int>(nameof(config.DungeonGateHp)),
				tileArea: dungeonArea,
				color: Color.Blue,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.JungleGate = GateBarrier.CreateGateBarrier(
				id: "JungleGate",
				strength: config.Get<int>(nameof(config.JungleGateHp)),
				tileArea: jungleArea,
				color: new Color( 128, 255, 0 ),
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.RockLayerGate = GateBarrier.CreateGateBarrier(
				id: "RockLayerGate",
				strength: config.Get<int>(nameof(config.RockLayerGateHp)),
				tileArea: rockLayerArea,
				color: Color.White,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.LavaLayerGate = GateBarrier.CreateGateBarrier(
				id: "LavaLayerGate",
				strength: config.Get<int>(nameof(config.LavaLayerGateHp)),
				tileArea: lavaLayerArea,
				color: Color.Yellow,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.UnderworldGate = GateBarrier.CreateGateBarrier(
				id: "UnderworldGate",
				strength: config.Get<int>(nameof(config.UnderworldGateHp)),
				tileArea: underworldArea,
				color: Color.Red,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.SkyGate = GateBarrier.CreateGateBarrier(
				id: "SkyGate",
				strength: config.Get<int>(nameof(config.SkyGateHp)),
				tileArea: skyArea,
				color: Color.Cyan,
				syncFromServer: Main.netMode == NetmodeID.Server
			);

			//

			this.WorldGates.Add( this.DungeonGate );
			this.WorldGates.Add( this.JungleGate );
			this.WorldGates.Add( this.RockLayerGate );
			this.WorldGates.Add( this.LavaLayerGate );
			this.WorldGates.Add( this.UnderworldGate );
			this.WorldGates.Add( this.SkyGate );
		}


		public bool InitializeAllGatesFromExistingBarriers() {
					//Rectangle dungeonArea,
					//Rectangle jungleArea,
					//Rectangle rockLayerArea,
					//Rectangle lavaLayerArea,
					//Rectangle underworldArea,
					//Rectangle skyArea ) {
			var worldBarriers = SoulBarriers.SoulBarriersAPI.GetWorldBarriers();

			this.DungeonGate = worldBarriers.FirstOrDefault( b => b.ID == "DungeonGate" );
			this.JungleGate = worldBarriers.FirstOrDefault( b => b.ID == "JungleGate" );
			this.RockLayerGate = worldBarriers.FirstOrDefault( b => b.ID == "RockLayerGate" );
			this.LavaLayerGate = worldBarriers.FirstOrDefault( b => b.ID == "LavaLayerGate" );
			this.UnderworldGate = worldBarriers.FirstOrDefault( b => b.ID == "UnderworldGate" );
			this.SkyGate = worldBarriers.FirstOrDefault( b => b.ID == "SkyGate" );
			//this.DungeonGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( dungeonArea );
			//this.JungleGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( jungleArea );
			//this.RockLayerGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( rockLayerArea );
			//this.LavaLayerGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( lavaLayerArea );
			//this.UnderworldGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( underworldArea );
			//this.SkyGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( skyArea );

			//
			
			if( this.DungeonGate != null ) {
				this.WorldGates.Add( this.DungeonGate );
			}
			if( this.JungleGate != null ) {
				this.WorldGates.Add( this.JungleGate );
			}
			if( this.RockLayerGate != null ) {
				this.WorldGates.Add( this.RockLayerGate );
			}
			if( this.LavaLayerGate != null ) {
				this.WorldGates.Add( this.LavaLayerGate );
			}
			if( this.UnderworldGate != null ) {
				this.WorldGates.Add( this.UnderworldGate );
			}
			if( this.SkyGate != null ) {
				this.WorldGates.Add( this.SkyGate );
			}

			return this.DungeonGate != null
				&& this.JungleGate != null
				&& this.RockLayerGate != null
				&& this.LavaLayerGate != null
				&& this.UnderworldGate != null
				&& this.SkyGate != null;
		}


		////////////////

		internal void UninitializeGates() {
			foreach( Barrier barrier in this.WorldGates ) {
				SoulBarriers.SoulBarriersAPI.RemoveWorldBarrier( ((GateBarrier)barrier).TileArea );
			}

			this.WorldGates.Clear();
		}
	}
}