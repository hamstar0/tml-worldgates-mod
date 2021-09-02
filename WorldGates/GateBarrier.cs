using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using SoulBarriers;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular.Access;


namespace WorldGates {
	public class GateBarrier : AccessBarrier {
		public static Barrier CreateGateBarrier( double strength, Rectangle tileArea, Color color ) {
			var worldArea = new Rectangle(
				x: tileArea.X * 16,
				y: tileArea.Y * 16,
				width: tileArea.Width * 16,
				height: tileArea.Height * 16
			);
			
			var barrier = new GateBarrier(
				worldArea: worldArea,
				strength: strength,
				color: color,
				isSaveable: false
			);

			SoulBarriersAPI.DeclareWorldBarrier( barrier );

			return barrier;
		}



		////////////////

		public GateBarrier( double strength, Rectangle worldArea, Color color, bool isSaveable )
					: base(
						strength: strength,
						maxRegenStrength: strength,
						strengthRegenPerTick: (float)((double)Int32.MaxValue * 0.5d) - 1f,
						worldArea: worldArea,
						color: color,
						isSaveable: isSaveable
					) {
			this.OnPreBarrierBarrierCollision.Add( ( barrier ) => {
				return !(barrier is AccessBarrier) && barrier.IsActive;
			} );

			this.OnBarrierBarrierCollision.Add( ( barrier ) => {
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
					Main.NewText( "Access granted.", Color.Lime );
					Main.PlaySound( SoundID.Item94 );
				}
			} );
		}
	}
}