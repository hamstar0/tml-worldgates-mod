using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using SoulBarriers.Barriers.BarrierTypes;
using SoulBarriers.Barriers.BarrierTypes.Rectangular;


namespace WorldGates {
	public partial class WorldGatesMod : Mod {
		private bool IsNearWorldGate( out Barrier barrier ) {
			var presets = GateBarrierPresets.Instance;

			if( presets.DungeonGate == null ) {
				LogLibraries.AlertOnce( "World gates not loaded." );

				barrier = null;
				return false;
			}

			//

			if( this.IsNearRectangularBarrier(presets.DungeonGate as RectangularBarrier) ) {
				barrier = presets.DungeonGate;
			} else if( this.IsNearRectangularBarrier(presets.JungleGate as RectangularBarrier) ) {
				barrier = presets.JungleGate;
			} else if( this.IsNearRectangularBarrier(presets.RockLayerGate as RectangularBarrier) ) {
				barrier = presets.RockLayerGate;
			} else if( this.IsNearRectangularBarrier(presets.LavaLayerGate as RectangularBarrier) ) {
				barrier = presets.LavaLayerGate;
			} else if( this.IsNearRectangularBarrier(presets.UnderworldGate as RectangularBarrier) ) {
				barrier = presets.UnderworldGate;
			} else {
				barrier = null;
			}

			return barrier != null;
		}

		public bool IsNearRectangularBarrier( RectangularBarrier barrier ) {
			Rectangle rect = barrier.TileArea;

			rect.X -= 8 * 16;
			rect.Y -= 16 * 16;
			rect.Width += 32 * 16;
			rect.Height += 32 * 16;

			return rect.Intersects( Main.LocalPlayer.getRect() );
		}


		////////////////

		private void AlertForGateProximity( Barrier barrier ) {
			var config = WorldGatesConfig.Instance;
			if( !config.Get<bool>(nameof(config.AlertAboutGates)) ) {
				return;
			}

			if( ModLoader.GetMod("Messages") != null ) {
				WorldGatesMod.AlertForGateProximity_Messages( barrier );
			} else {
				if( config.Get<bool>(nameof(config.WarnAboutGatesAndPBG)) ) {
					Main.NewText(
						"A magical gate barrier blocks your path. Use a P.B.G to cross.",
						new Color( 147, 0, 255 )
					);
				} else {
					Main.NewText(
						"A magical gate barrier blocks your path.",
						new Color( 147, 0, 255 )
					);
				}
			}
		}

		////

		private static void AlertForGateProximity_Messages( Barrier barrier ) {
			Messages.MessagesAPI.AddMessagesInitializeEvent( () => {
				WorldGatesMod.AlertForGateProximity_Messages_Event( barrier );
			} );
		}

		private static void AlertForGateProximity_Messages_Event( Barrier barrier ) {
			var config = WorldGatesConfig.Instance;

			if( config.Get<bool>( nameof(config.WarnAboutGatesAndPBG) ) ) {
				string id = "WorldGates_VsPBG";

				Messages.MessagesAPI.AddMessage(
					title: "Beware world gate barriers!",
					description: "The only safe way to cross these is to use a P.B.G (Personal Barrier Generator)"
						+" item with enough juice to over-power the gate, thus rendering it inert.",
					modOfOrigin: WorldGatesMod.Instance,
					alertPlayer: Messages.MessagesAPI.IsUnread( id ),
					isImportant: false,
					parentMessage: Messages.MessagesAPI.HintsTipsCategoryMsg,
					id: id
				);
			}
			
			if( config.Get<bool>(nameof(config.AlertAboutGates)) ) {
				string id = "WorldGates_Overview";

				Messages.MessagesAPI.AddMessage(
					title: "World Gates",
					description: "If in your travels you encounter a large and very long stream of mysterious energy,"
						+" chances are you've encountered one of your world's barrier gates. Do NOT try to cross"
						+" these without the right means! They are invariably destructive to all non-passive objects"
						+" that come into contact with."
						+"\n \n"
						+"These gate barriers appear to have been made to repel anyone who isn't an island native."
						+" Due to their lethality, they were probably not made for the benefit of outsiders,"
						+" but it's unclear if they exist necessarily to keep something out, or in. They do"
						+" appear to get stronger with depth, however...",
					modOfOrigin: WorldGatesMod.Instance,
					alertPlayer: Messages.MessagesAPI.IsUnread( id ),
					isImportant: false,
					parentMessage: Messages.MessagesAPI.GameInfoCategoryMsg,
					id: id
				);
			}
		}
	}
}