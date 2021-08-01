using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using SoulBarriers.Items;


namespace WorldGates {
	public class WorldGatesPlayer : ModPlayer {
		public override void SetupStartInventory( IList<Item> items, bool mediumcoreDeath ) {
			if( !mediumcoreDeath ) {
				var config = WorldGatesConfig.Instance;

				if( config.Get<bool>( nameof(config.StartNewPlayersWithPBG) ) ) {
					var pbg = new Item();
					pbg.SetDefaults( ModContent.ItemType<PBGItem>() );

					items.Add( pbg );
				}
			}
		}
	}
}
