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
using SoulBarriers.Barriers;

namespace WorldGates {
	public class GateBarrier : AccessBarrier {
		public static Barrier CreateGateBarrier(
					string id,
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
				id: id,
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
			bool preCollide( Barrier barrier, ref double damage ) {
				return !( barrier is AccessBarrier ) && barrier.IsActive;
			}

			void postCollide( Barrier otherBarrier, bool isDefaultHit, double damage ) {
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
			}

			//

			this.OnPreBarrierBarrierCollision.Add( preCollide );
			this.OnPostBarrierBarrierCollision.Add( postCollide );
		}


		////////////////

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