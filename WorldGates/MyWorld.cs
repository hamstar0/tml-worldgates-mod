using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.World.Generation;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.World;


namespace WorldGates {
	class WorldGatesGenPass : GenPass {
		public WorldGatesGenPass() : base( "World Gates Dungeon Failsafe", 1f ) { }


		public override void Apply( GenerationProgress progress ) {
			for( int x=0; x<Main.maxTilesX; x++ ) {
				for( int y=WorldLocationLibraries.SkyLayerBottomTileY + 7; y>40; y-- ) {
					Tile tile = Main.tile[x, y];
					if( tile?.active() != true ) {
						continue;
					}
					if( tile.type != TileID.BlueDungeonBrick
						&& tile.type != TileID.GreenDungeonBrick
						&& tile.type != TileID.PinkDungeonBrick ) {
						continue;
					}

					// Dungeon tiles above a certain height
					tile.inActive( true );
				}
			}
		}
	}




	public partial class WorldGatesWorld : ModWorld {
		public override void ModifyWorldGenTasks( List<GenPass> tasks, ref float totalWeight ) {
			tasks.Add( new WorldGatesGenPass() );
		}
	}
}