using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.World;


namespace WorldGates {
	public partial class GateBarrierPresets : ILoadable {
		public static int GetJungleStartCoordinate() {
			int dirtTop = WorldLocationLibraries.DirtLayerTopTileY;

			int checkColumn( int myTileX ) {
				for( int myTileY = 40; myTileY < dirtTop; myTileY++ ) {
					Tile tile = Main.tile[myTileX, myTileY];
					if( tile?.active() == true && tile.type == TileID.JungleGrass ) {
						return myTileY;
					}
				}
				return -1;
			}

			//

			if( Main.spawnTileX < (Main.maxTilesX / 2) ) {    // Dungeon on left, jungle on right
				for( int tileX = Main.maxTilesX / 2; tileX < Main.maxTilesX; tileX++ ) {
					if( checkColumn(tileX) != -1 ) {
						return tileX;
					}
				}
			} else {    // Dungeon on right, jungle on left
				for( int tileX = Main.maxTilesX / 2; tileX > 0; tileX-- ) {
					if( checkColumn(tileX) != -1 ) {
						return tileX;
					}
				}
			}

			return -1;
		}

		
		public static int GetLavaTileY() {
			int rockLayerScanStartY = (((int)Main.rockLayer + Main.maxTilesY) / 2) - 60;
			int minX = 40;
			int maxX = Main.maxTilesX - 40;

			if( maxX <= minX ) {
				minX = 1;
				maxX = Main.maxTilesX - 1;
			}

			Tile tile;

			for( int y=rockLayerScanStartY; y<WorldLocationLibraries.UnderworldLayerTopTileY; y++ ) {
				for( int x=minX; x<maxX; x++ ) {
					tile = Main.tile[x, y];
					if( tile == null ) {
						continue;
					}
					if( tile.liquid >= 1 && tile.lava() ) {
						return y;
					}
				}
			}

			return -1;
		}



		////////////////

		public void GetGatePositions(
					out Rectangle dungeonArea,
					out Rectangle jungleArea,
					out Rectangle rockLayerArea,
					out Rectangle lavaLayerArea,
					out Rectangle underworldArea,
					out Rectangle skyArea ) {
			int barrierThick = 10;

			bool isDungeonLeft = Main.dungeonX < (Main.maxTilesX / 2);
			int jungleX = GateBarrierPresets.GetJungleStartCoordinate();
			int lavaLine = GateBarrierPresets.GetLavaTileY();

			if( lavaLine == -1 ) {
				throw new ModLibsException( "No lava layer found in world." );
			}

			//
			
			Rectangle getTallBarrier( int tileX ) {
				return new Rectangle(
					x: tileX,
					y: 1,
					width: barrierThick,
					height: (int)Main.rockLayer
				);
			}
			
			Rectangle getFatBarrier( int tileY ) {
				return new Rectangle(
					x: 1,
					y: tileY,
					width: Main.maxTilesX - 2,
					height: barrierThick
				);
			}

			//

			dungeonArea = getTallBarrier( Main.dungeonX + (isDungeonLeft ? 50 : -50) );
			jungleArea = getTallBarrier( jungleX );
			rockLayerArea = getFatBarrier( WorldLocationLibraries.RockLayerTopTileY );
			lavaLayerArea = getFatBarrier( lavaLine );
			underworldArea = getFatBarrier( WorldLocationLibraries.UnderworldLayerTopTileY );
			skyArea = getFatBarrier( WorldLocationLibraries.SkyLayerBottomTileY );
			//LogLibraries.Log( "dungeonArea: "+dungeonArea );
			//LogLibraries.Log( "jungleArea: "+jungleArea );
			//LogLibraries.Log( "rockLayerArea: "+rockLayerArea );
			//LogLibraries.Log( "lavaLayerArea: "+lavaLayerArea );
			//LogLibraries.Log( "underworldArea: "+underworldArea );
		}
	}
}