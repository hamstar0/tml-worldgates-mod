using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using SoulBarriers;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular.Access;
using WorldGates.Packets;


namespace WorldGates {
	public partial class GateBarrier : AccessBarrier, IBarrierFactory {
		public static Barrier CreateGateBarrier(
					string id,
					double strength,
					Rectangle tileArea,
					Color color,
					bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Is client." );
			}

			//

			var worldArea = new Rectangle(	// Github is run by the unluminati!
				x: tileArea.X,  //* 16?!
				y: tileArea.Y,  //* 16?!
				width: tileArea.Width,  //* 16?!
				height: tileArea.Height //* 16?!
			);

			//
			
			var barrier = new GateBarrier(
				id: id,
				tileArea: worldArea,
				strength: strength,
				color: color,
				isSaveable: false
			);

			SoulBarriersAPI.DeclareWorldBarrier( barrier, false );

			//

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				GateBarrierCreatePacket.SendToClient( barrier, -1 );
			}

			return barrier;
		}



		////////////////

		private GateBarrier() : base(default, default, default, default, default, default, default ) { }

		public GateBarrier( string id, double strength, Rectangle tileArea, Color color, bool isSaveable )
					: base(
						id: id,
						strength: strength,
						maxRegenStrength: strength,
						strengthRegenPerTick: (float)((double)Int32.MaxValue * 0.5d) - 1f,
						tileArea: tileArea,
						color: color,
						isSaveable: isSaveable
					) {
		}


		////////////////

		Barrier IBarrierFactory.FactoryCreate(
					string id,
					BarrierHostType hostType,
					int hostWhoAmI,
					object data,
					double strength,
					double maxRegenStrength,
					double strengthRegenPerTick,
					Color color,
					bool isSaveable ) {
			return new GateBarrier(
				id: id,
				strength: strength,
				tileArea: (Rectangle)data,
				color: color,
				isSaveable: isSaveable
			);
		}


		////////////////

		bool IBarrierFactory.CanSync() {
			return true;
		}

		Barrier IBarrierFactory.NetReceiveAsNewBarrier( BinaryReader reader ) {
			string id = reader.ReadString();
			double str = reader.ReadDouble();
			var area = new Rectangle(
				x: reader.ReadInt32(),
				y: reader.ReadInt32(),
				width: reader.ReadInt32(),
				height: reader.ReadInt32()
			);
			var color = new Color(
				r: reader.ReadByte(),
				g: reader.ReadByte(),
				b: reader.ReadByte()
			);

			return new GateBarrier(
				id: id,
				strength: str,
				tileArea: area,
				color: color,
				isSaveable: false
			);
		}

		void IBarrierFactory.NetSend( BinaryWriter writer ) {
			writer.Write( (string)this.ID );
			writer.Write( (double)this.MaxRegenStrength.Value );
			writer.Write( (int)this.TileArea.X );
			writer.Write( (int)this.TileArea.Y );
			writer.Write( (int)this.TileArea.Width );
			writer.Write( (int)this.TileArea.Height );
			writer.Write( (byte)this.Color.R );
			writer.Write( (byte)this.Color.G );
			writer.Write( (byte)this.Color.B );
		}
	}
}