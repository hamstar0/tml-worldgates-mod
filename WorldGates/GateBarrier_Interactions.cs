using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular.Access;


namespace WorldGates {
	public partial class GateBarrier : AccessBarrier, IBarrierFactory {
		public override bool IsBarrierColliding( Barrier barrier ) {
			return barrier.HostType == BarrierHostType.Player
				&& base.IsBarrierColliding( barrier );
		}


		////////////////

		public override bool CanHitNPC( NPC intruder ) {
			if( WorldGatesMod.Instance.IsTricksterModLoaded ) {
				if( !GateBarrier.CanHitNPC_TricksterRef(intruder) ) {
					return false;
				}
			}

			return base.CanHitNPC( intruder );
		}

		////

		private static bool CanHitNPC_TricksterRef( NPC intruder ) {
			int tricksterType = ModLoader.GetMod("TheTrickster").NPCType( "TricksterNPC" );

			if( intruder.type == tricksterType ) {
				return false;	// TODO: Make this less hard coded
			}
			return true;
		}
	}
}