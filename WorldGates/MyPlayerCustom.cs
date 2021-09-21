using System;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.PlayerData;
using SoulBarriers.Barriers.BarrierTypes;
using WorldGates.Packets;


namespace WorldGates {
	public class WorldGatesCustomPlayer : CustomPlayerData {
		protected override void OnEnter( bool isCurrentPlayer, object data ) {
			if( Main.netMode == NetmodeID.Server ) {
				foreach( Barrier barrier in WorldGatesMod.Instance.WorldGates ) {
					GateBarrierCreatePacket.SendToClient( barrier as GateBarrier, this.PlayerWho );
				}
			}
		}
	}
}
