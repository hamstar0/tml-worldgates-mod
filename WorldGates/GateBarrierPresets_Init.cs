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
			if( this.InitializeAllGatesFromExistingBarriers(out string result) ) {
				LogLibraries.Alert( result );
				return;
			}

			//

			this.UninitializeGates();

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
			if( this.WorldGates.ContainsKey( this.DungeonGateName ) ) {
				throw new ModLibsException( "Existing dungeon gate found." );
			}
			if( this.WorldGates.ContainsKey( this.JungleGateName ) ) {
				throw new ModLibsException( "Existing jungle gate found." );
			}
			if( this.WorldGates.ContainsKey( this.RockLayerGateName ) ) {
				throw new ModLibsException( "Existing rock layer gate found." );
			}
			if( this.WorldGates.ContainsKey( this.LavaLayerGateName ) ) {
				throw new ModLibsException( "Existing lava layer gate found." );
			}
			if( this.WorldGates.ContainsKey( this.UnderworldGateName ) ) {
				throw new ModLibsException( "Existing underworld gate found." );
			}
			if( this.WorldGates.ContainsKey( this.SkyGateName ) ) {
				throw new ModLibsException( "Existing sky gate found." );
			}

			//

			var config = WorldGatesConfig.Instance;

			//

			this.WorldGates[this.DungeonGateName] = GateBarrier.CreateGateBarrier_Host(
				id: this.DungeonGateName,
				strength: config.Get<int>(nameof(config.DungeonGateHp)),
				tileArea: dungeonArea,
				color: Color.Blue,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.WorldGates[this.JungleGateName] = GateBarrier.CreateGateBarrier_Host(
				id: this.JungleGateName,
				strength: config.Get<int>(nameof(config.JungleGateHp)),
				tileArea: jungleArea,
				color: new Color( 128, 255, 0 ),
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.WorldGates[this.RockLayerGateName] = GateBarrier.CreateGateBarrier_Host(
				id: this.RockLayerGateName,
				strength: config.Get<int>(nameof(config.RockLayerGateHp)),
				tileArea: rockLayerArea,
				color: Color.White,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.WorldGates[this.LavaLayerGateName] = GateBarrier.CreateGateBarrier_Host(
				id: this.LavaLayerGateName,
				strength: config.Get<int>(nameof(config.LavaLayerGateHp)),
				tileArea: lavaLayerArea,
				color: Color.Yellow,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.WorldGates[this.UnderworldGateName] = GateBarrier.CreateGateBarrier_Host(
				id: this.UnderworldGateName,
				strength: config.Get<int>(nameof(config.UnderworldGateHp)),
				tileArea: underworldArea,
				color: Color.Red,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.WorldGates[this.SkyGateName] = GateBarrier.CreateGateBarrier_Host(
				id: this.SkyGateName,
				strength: config.Get<int>(nameof(config.SkyGateHp)),
				tileArea: skyArea,
				color: Color.Cyan,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
		}


		public bool InitializeAllGatesFromExistingBarriers( out string result ) {
			Barrier[] worldBarriers = SoulBarriers.SoulBarriersAPI.GetWorldBarriers();
			IDictionary<string, Barrier> myWG = this.WorldGates;

			Barrier dungeonGate = worldBarriers.FirstOrDefault( b => b.ID == this.DungeonGateName );
			Barrier jungleGate = worldBarriers.FirstOrDefault( b => b.ID == this.JungleGateName );
			Barrier rockLayerGate = worldBarriers.FirstOrDefault( b => b.ID == this.RockLayerGateName );
			Barrier lavaLayerGate = worldBarriers.FirstOrDefault( b => b.ID == this.LavaLayerGateName );
			Barrier underworldGate = worldBarriers.FirstOrDefault( b => b.ID == this.UnderworldGateName );
			Barrier skyGate = worldBarriers.FirstOrDefault( b => b.ID == this.SkyGateName );

			//
			
			if( dungeonGate == null ) {
				result = "Missing (at least) "+this.DungeonGateName;
				return false;
			}
			if( jungleGate == null ) {
				result = "Missing (at least) "+this.JungleGateName;
				return false;
			}
			if( rockLayerGate == null ) {
				result = "Missing (at least) "+this.RockLayerGateName;
				return false;
			}
			if( lavaLayerGate == null ) {
				result = "Missing (at least) "+this.LavaLayerGateName;
				return false;
			}
			if( underworldGate == null ) {
				result = "Missing (at least) "+this.UnderworldGateName;
				return false;
			}
			if( skyGate == null ) {
				result = "Missing "+this.SkyGateName;
				return false;
			}

			//

			if( !(dungeonGate is GateBarrier) ) {
				throw new ModLibsException( $"Discovered dungeon gate ({dungeonGate?.GetType().Name}) is not GateBarrier" );
			}
			if( !(jungleGate is GateBarrier) ) {
				throw new ModLibsException( $"Discovered jungle gate ({jungleGate?.GetType().Name}) is not GateBarrier" );
			}
			if( !(rockLayerGate is GateBarrier) ) {
				throw new ModLibsException( $"Discovered rock layer ({rockLayerGate?.GetType().Name}) gate is not GateBarrier" );
			}
			if( !(lavaLayerGate is GateBarrier) ) {
				throw new ModLibsException( $"Discovered lava layer ({lavaLayerGate?.GetType().Name}) gate is not GateBarrier" );
			}
			if( !(underworldGate is GateBarrier) ) {
				throw new ModLibsException( $"Discovered underworld ({underworldGate?.GetType().Name}) layer gate is not GateBarrier" );
			}
			if( !(skyGate is GateBarrier) ) {
				throw new ModLibsException( $"Discovered sky layer ({skyGate?.GetType().Name}) gate is not GateBarrier" );
			}

			//
			
			if( myWG.ContainsKey(this.DungeonGateName) && myWG[this.DungeonGateName] != dungeonGate ) {
				throw new ModLibsException( "Pre-existing, extra dungeon gate found." );
			}
			if( myWG.ContainsKey(this.JungleGateName) && myWG[this.JungleGateName] != jungleGate ) {
				throw new ModLibsException( "Pre-existing, extra jungle gate found." );
			}
			if( myWG.ContainsKey(this.RockLayerGateName) && myWG[this.RockLayerGateName] != rockLayerGate ) {
				throw new ModLibsException( "Pre-existing, extra rock layer gate found." );
			}
			if( myWG.ContainsKey(this.LavaLayerGateName) && myWG[this.LavaLayerGateName] != lavaLayerGate ) {
				throw new ModLibsException( "Pre-existing, extra lava layer gate found." );
			}
			if( myWG.ContainsKey(this.UnderworldGateName) && myWG[this.UnderworldGateName] != underworldGate ) {
				throw new ModLibsException( "Pre-existing, extra underworld gate found." );
			}
			if( myWG.ContainsKey(this.SkyGateName) && myWG[this.SkyGateName] != skyGate ) {
				throw new ModLibsException( "Pre-existing, extra sky gate found." );
			}

			//

			this.WorldGates[this.DungeonGateName] = dungeonGate;
			this.WorldGates[this.JungleGateName] = jungleGate;
			this.WorldGates[this.RockLayerGateName] = rockLayerGate;
			this.WorldGates[this.LavaLayerGateName] = lavaLayerGate;
			this.WorldGates[this.UnderworldGateName] = underworldGate;
			this.WorldGates[this.SkyGateName] = skyGate;

			//

			result = "All gates discovered";
			return true;
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