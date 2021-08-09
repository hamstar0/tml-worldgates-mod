using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


namespace WorldGates {
	public partial class WorldGatesMod : Mod {
		private bool IsNearWorldGate( out Barrier barrier ) {
			if( this.IsNearBarrier(this.DungeonGate as RectangularBarrier) ) {
				barrier = this.DungeonGate;
			} else if( this.IsNearBarrier(this.JungleGate as RectangularBarrier) ) {
				barrier = this.JungleGate;
			} else if( this.IsNearBarrier(this.RockLayerGate as RectangularBarrier) ) {
				barrier = this.RockLayerGate;
			} else if( this.IsNearBarrier(this.LavaLayerGate as RectangularBarrier) ) {
				barrier = this.LavaLayerGate;
			} else if( this.IsNearBarrier(this.UnderworldGate as RectangularBarrier) ) {
				barrier = this.UnderworldGate;
			} else {
				barrier = null;
			}

			return barrier != null;
		}

		public bool IsNearBarrier( RectangularBarrier barrier ) {
			var rectBarrier = barrier as RectangularBarrier;
			Rectangle rect = rectBarrier.WorldArea;

			rect.X -= 8 * 16;
			rect.Y -= 16 * 16;
			rect.Width += 32 * 16;
			rect.Height += 32 * 16;

			return rect.Intersects( Main.LocalPlayer.getRect() );
		}


		////////////////

		private void AlertForGateProximity( Barrier barrier ) {
			if( ModLoader.GetMod("Messages") != null ) {
				WorldGatesMod.AlertForGateProximity_Messages( barrier );
			} else {
				Main.NewText( "A magical barrier blocks your path. Use a P.B.G to cross.", new Color(147, 0, 255) );
			}
		}

		////

		private static void AlertForGateProximity_Messages( Barrier barrier ) {
			Messages.MessagesAPI.AddMessage(
				title: "Beware gate barriers!",
				description: "If you encounter a large and very long stream of mysterious energy in your travels,"
					+" chances are you've encountered one of your world's barrier gates. Do NOT try to cross these!"
					+" They are invariably destructive to all non-passive objects that cross their path."
					+"\n\nThe only safe way to cross is to use a P.B.G (Personal Barrier Generator) item with enough"
					+"juice to overpower the gate barrier, thus rendering it inert.",
				modOfOrigin: WorldGatesMod.Instance,
				parentMessage: Messages.MessagesAPI.ModInfoCategoryMsg
			);
		}
	}
}