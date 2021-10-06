using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using SoulBarriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular.Access;
using WorldGates.Packets;


namespace WorldGates {
	public class GateBarrier : AccessBarrier {
		public static Barrier CreateGateBarrier(
					double strength,
					Rectangle tileArea,
					Color color,
					bool syncFromServer ) {
			if( syncFromServer && Main.netMode == NetmodeID.MultiplayerClient ) {
				throw new ModLibsException( "Is client." );
			}

			var worldArea = new Rectangle(	// Github is run by the unluminati!
				x: tileArea.X,  //* 16?!
				y: tileArea.Y,  //* 16?!
				width: tileArea.Width,  //* 16?!
				height: tileArea.Height //* 16?!
			);
			
			var barrier = new GateBarrier(
				tileArea: worldArea,
				strength: strength,
				color: color,
				isSaveable: false
			);

			SoulBarriersAPI.DeclareWorldBarrierUnsynced( barrier );

			//

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				GateBarrierCreatePacket.SendToClient( barrier, -1 );
			}

			return barrier;
		}



		////////////////
		
		public GateBarrier( double strength, Rectangle tileArea, Color color, bool isSaveable )
					: base(
						strength: strength,
						maxRegenStrength: strength,
						strengthRegenPerTick: (float)((double)Int32.MaxValue * 0.5d) - 1f,
						tileArea: tileArea,
						color: color,
						isSaveable: isSaveable
					) {
			this.OnPreBarrierBarrierCollision.Add( ( barrier ) => {
				return !(barrier is AccessBarrier) && barrier.IsActive;
			} );

			this.OnBarrierBarrierCollision.Add( ( barrier ) => {
//LogLibraries.Log( "B V B OnBarrierBarrierCollision 2 - " + this.Strength );
				if( this.Strength > 0d ) {
					string str;
					if( (this.Strength % 1d) > 0d ) {
						str = ((int)this.Strength + (this.Strength % 1d)).ToString("N2");
					} else {
						str = ((int)this.Strength).ToString();
					}

					Main.NewText( "Gate barrier is too strong. +"+str+" strength needed to breach.", Color.Yellow );
					Main.PlaySound( SoundID.NPCHit53 );
				} else {
					//Main.NewText( "Access granted.", Color.Lime );
					//Main.PlaySound( SoundID.Item94 );
				}
			} );
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
			if( intruder.type == ModContent.NPCType<TheTrickster.NPCs.TricksterNPC>() ) {
				return false;	// TODO: Make this less hard coded
			}
			return true;
		}
	}
}