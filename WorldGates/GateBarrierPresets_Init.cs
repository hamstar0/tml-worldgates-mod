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
		public void InitializeGates() {
			bool hasExistingGates = this.InitializeGatesFromExistingBarriers();

			if( hasExistingGates ) {
				return;
			}

			//

			this.GetGatePositions(
				out Rectangle dungeonArea,
				out Rectangle jungleArea,
				out Rectangle rockLayerArea,
				out Rectangle lavaLayerArea,
				out Rectangle underworldArea,
				out Rectangle skyArea
			);

			//

			this.UninitializeGates();

			this.InitializeGatesAnew(
				dungeonArea: dungeonArea,
				jungleArea: jungleArea,
				rockLayerArea: rockLayerArea,
				lavaLayerArea: lavaLayerArea,
				underworldArea: underworldArea,
				skyArea: skyArea
			);
		}


		////////////////

		public void InitializeGatesAnew(
					Rectangle dungeonArea,
					Rectangle jungleArea,
					Rectangle rockLayerArea,
					Rectangle lavaLayerArea,
					Rectangle underworldArea,
					Rectangle skyArea ) {
			var config = WorldGatesConfig.Instance;

			//
			
			Barrier dungeonGate = GateBarrier.CreateGateBarrier_Host(
				id: "DungeonGate",
				strength: config.Get<int>(nameof(config.DungeonGateHp)),
				tileArea: dungeonArea,
				color: Color.Blue,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			Barrier jungleGate = GateBarrier.CreateGateBarrier_Host(
				id: "JungleGate",
				strength: config.Get<int>(nameof(config.JungleGateHp)),
				tileArea: jungleArea,
				color: new Color( 128, 255, 0 ),
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			Barrier rockLayerGate = GateBarrier.CreateGateBarrier_Host(
				id: "RockLayerGate",
				strength: config.Get<int>(nameof(config.RockLayerGateHp)),
				tileArea: rockLayerArea,
				color: Color.White,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			Barrier lavaLayerGate = GateBarrier.CreateGateBarrier_Host(
				id: "LavaLayerGate",
				strength: config.Get<int>(nameof(config.LavaLayerGateHp)),
				tileArea: lavaLayerArea,
				color: Color.Yellow,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			Barrier underworldGate = GateBarrier.CreateGateBarrier_Host(
				id: "UnderworldGate",
				strength: config.Get<int>(nameof(config.UnderworldGateHp)),
				tileArea: underworldArea,
				color: Color.Red,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			Barrier skyGate = GateBarrier.CreateGateBarrier_Host(
				id: "SkyGate",
				strength: config.Get<int>(nameof(config.SkyGateHp)),
				tileArea: skyArea,
				color: Color.Cyan,
				syncFromServer: Main.netMode == NetmodeID.Server
			);

			//

			if( !this.WorldGates.ContainsKey( this.DungeonGateName ) ) {
				this.WorldGates[this.DungeonGateName] = dungeonGate;
			} else {
				throw new ModLibsException( "Existing dungeon gate found." );
			}
			if( !this.WorldGates.ContainsKey( this.JungleGateName ) ) {
				this.WorldGates[this.JungleGateName] = jungleGate;
			} else {
				throw new ModLibsException( "Existing jungle gate found." );
			}
			if( !this.WorldGates.ContainsKey( this.RockLayerGateName ) ) {
				this.WorldGates[this.RockLayerGateName] = rockLayerGate;
			} else {
				throw new ModLibsException( "Existing rock layer gate found." );
			}
			if( !this.WorldGates.ContainsKey( this.LavaLayerGateName ) ) {
				this.WorldGates[this.LavaLayerGateName] = lavaLayerGate;
			} else {
				throw new ModLibsException( "Existing lava layer gate found." );
			}
			if( !this.WorldGates.ContainsKey( this.UnderworldGateName ) ) {
				this.WorldGates[this.UnderworldGateName] = underworldGate;
			} else {
				throw new ModLibsException( "Existing underworld gate found." );
			}
			if( !this.WorldGates.ContainsKey( this.SkyGateName ) ) {
				this.WorldGates[this.SkyGateName] = skyGate;
			} else {
				throw new ModLibsException( "Existing sky gate found." );
			}
		}


		public bool InitializeGatesFromExistingBarriers() {
			Barrier[] worldBarriers = SoulBarriers.SoulBarriersAPI.GetWorldBarriers();

			Barrier dungeonGate = worldBarriers.FirstOrDefault( b => b.ID == this.DungeonGateName );
			Barrier jungleGate = worldBarriers.FirstOrDefault( b => b.ID == this.JungleGateName );
			Barrier rockLayerGate = worldBarriers.FirstOrDefault( b => b.ID == this.RockLayerGateName );
			Barrier lavaLayerGate = worldBarriers.FirstOrDefault( b => b.ID == this.LavaLayerGateName );
			Barrier underworldGate = worldBarriers.FirstOrDefault( b => b.ID == this.UnderworldGateName );
			Barrier skyGate = worldBarriers.FirstOrDefault( b => b.ID == this.SkyGateName );
			//this.DungeonGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( dungeonArea );
			//this.JungleGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( jungleArea );
			//this.RockLayerGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( rockLayerArea );
			//this.LavaLayerGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( lavaLayerArea );
			//this.UnderworldGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( underworldArea );
			//this.SkyGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( skyArea );

			//
			
			if( !(dungeonGate is GateBarrier) ) {
				throw new ModLibsException( "Discovered dungeon gate is not GateBarrier" );
			}
			if( !(jungleGate is GateBarrier) ) {
				throw new ModLibsException( "Discovered jungle gate is not GateBarrier" );
			}
			if( !(rockLayerGate is GateBarrier) ) {
				throw new ModLibsException( "Discovered rock layer gate is not GateBarrier" );
			}
			if( !(lavaLayerGate is GateBarrier) ) {
				throw new ModLibsException( "Discovered lava layer gate is not GateBarrier" );
			}
			if( !(underworldGate is GateBarrier) ) {
				throw new ModLibsException( "Discovered underworld layer gate is not GateBarrier" );
			}
			if( !(skyGate is GateBarrier) ) {
				throw new ModLibsException( "Discovered sky layer gate is not GateBarrier" );
			}

			//
			
			if( !this.WorldGates.ContainsKey(this.DungeonGateName) && dungeonGate != null ) {
				this.WorldGates[this.DungeonGateName] = dungeonGate;
			} else {
				throw new ModLibsException( "Pre-existing dungeon gate found." );
			}
			if( !this.WorldGates.ContainsKey(this.JungleGateName) && jungleGate != null ) {
				this.WorldGates[this.JungleGateName] = jungleGate;
			} else {
				throw new ModLibsException( "Pre-existing jungle gate found." );
			}
			if( !this.WorldGates.ContainsKey(this.RockLayerGateName) && rockLayerGate != null ) {
				this.WorldGates[this.RockLayerGateName] = rockLayerGate;
			} else {
				throw new ModLibsException( "Pre-existing rock layer gate found." );
			}
			if( !this.WorldGates.ContainsKey(this.LavaLayerGateName) && lavaLayerGate != null ) {
				this.WorldGates[this.LavaLayerGateName] = lavaLayerGate;
			} else {
				throw new ModLibsException( "Pre-existing lava layer gate found." );
			}
			if( !this.WorldGates.ContainsKey(this.UnderworldGateName) && underworldGate != null ) {
				this.WorldGates[this.UnderworldGateName] = underworldGate;
			} else {
				throw new ModLibsException( "Pre-existing underworld gate found." );
			}
			if( !this.WorldGates.ContainsKey(this.SkyGateName) && skyGate != null ) {
				this.WorldGates[this.SkyGateName] = skyGate;
			} else {
				throw new ModLibsException( "Pre-existing sky gate found." );
			}

			//

			return dungeonGate != null
				&& jungleGate != null
				&& rockLayerGate != null
				&& lavaLayerGate != null
				&& underworldGate != null
				&& skyGate != null;
		}


		////////////////

		internal void UninitializeGates() {
			foreach( Barrier barrier in this.WorldGates.Values ) {
				SoulBarriers.SoulBarriersAPI.RemoveWorldBarrier( ((GateBarrier)barrier).TileArea );
			}

			this.WorldGates.Clear();
		}
	}
}