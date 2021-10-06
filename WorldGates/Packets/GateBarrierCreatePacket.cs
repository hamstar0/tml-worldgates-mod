using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Network.SimplePacket;
using SoulBarriers;


namespace WorldGates.Packets {
	class GateBarrierCreatePacket : SimplePacketPayload {
		public static void SendToClient( GateBarrier barrier, int playerWho ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModLibsException( "Not server." );
			}

			var packet = new GateBarrierCreatePacket( barrier );

			SimplePacket.SendToClient( packet, playerWho );
		}



		////////////////

		public int HostType;

		public int HostWhoAmI;

		public Rectangle TileArea;

		public double Strength;

		public double MaxRegenStrength;

		public double StrengthRegenPerTick;

		public byte ColorR;
		public byte ColorG;
		public byte ColorB;



		////////////////

		private GateBarrierCreatePacket() { }

		private GateBarrierCreatePacket( GateBarrier barrier ) {
			this.HostType = (int)barrier.HostType;
			this.HostWhoAmI = barrier.HostWhoAmI;
			this.TileArea = barrier.TileArea;
			this.Strength = barrier.Strength;
			this.MaxRegenStrength = barrier.MaxRegenStrength.HasValue ? -1d : barrier.MaxRegenStrength.Value;
			this.StrengthRegenPerTick = barrier.StrengthRegenPerTick;
			this.ColorR = barrier.Color.R;
			this.ColorG = barrier.Color.G;
			this.ColorB = barrier.Color.B;
		}

		////////////////

		public override void ReceiveOnClient() {
			var barrier = new GateBarrier(
				tileArea: this.TileArea,
				strength: this.Strength,
				color: new Color(this.ColorR, this.ColorG, this.ColorB),
				isSaveable: false
			);

			SoulBarriersAPI.DeclareWorldBarrierUnsynced( barrier );
		}

		public override void ReceiveOnServer( int fromWho ) {
			throw new NotImplementedException( "Server isn't synced new barriers." );
		}
	}
}