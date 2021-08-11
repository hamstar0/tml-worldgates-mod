using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.World;
using SoulBarriers.Barriers;


namespace WorldGates {
	public partial class WorldGatesMod : Mod {
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
				maxX = Main.maxTilesY - 1;
			}

			Tile tile;

			for( int y=rockLayerScanStartY; y<WorldLocationLibraries.UnderworldLayerTopTileY; y++ ) {
				for( int x=minX; x<maxX; x++ ) {
					tile = Main.tile[x, y];
					if( tile.liquid >= 1 && tile.lava() ) {
						return y;
					}
				}
			}

			return -1;
		}



		////////////////

		private void InitializeGates() {
			int barrierThick = 10;

			bool isDungeonLeft = Main.dungeonX < (Main.maxTilesX / 2);
			int jungleX = WorldGatesMod.GetJungleStartCoordinate();
			int lavaLine = WorldGatesMod.GetLavaTileY();

			if( lavaLine == -1 ) {
				throw new ModLibsException( "No lava layer found in world." );
			}

			//

			var dungeonArea = new Rectangle(
				x: isDungeonLeft
					? Main.dungeonX + 50
					: Main.dungeonX - 50,
				y: 1,
				width: barrierThick,
				height: (int)Main.rockLayer
			);
			var jungleArea = new Rectangle(
				x: jungleX,
				y: 1,
				width: barrierThick,
				height: (int)Main.rockLayer
			);
			var rockLayerArea = new Rectangle(
				x: 1,
				y: WorldLocationLibraries.RockLayerTopTileY,
				width: Main.maxTilesX - 2,
				height: barrierThick
			);
			var lavaLayerArea = new Rectangle(
				x: 1,
				y: lavaLine,
				width: Main.maxTilesX - 2,
				height: barrierThick
			);
			var underworldArea = new Rectangle(
				x: 1,
				y: WorldLocationLibraries.UnderworldLayerTopTileY,
				width: Main.maxTilesX - 2,
				height: barrierThick
			);
			//LogLibraries.Log( "dungeonArea: "+dungeonArea );
			//LogLibraries.Log( "jungleArea: "+jungleArea );
			//LogLibraries.Log( "rockLayerArea: "+rockLayerArea );
			//LogLibraries.Log( "lavaLayerArea: "+lavaLayerArea );
			//LogLibraries.Log( "underworldArea: "+underworldArea );

			//
			
			this.DungeonGate = GateBarrier.CreateGateBarrier( 20, dungeonArea, BarrierColor.BigBlue );
			this.JungleGate = GateBarrier.CreateGateBarrier( 30, jungleArea, BarrierColor.Green );
			this.RockLayerGate = GateBarrier.CreateGateBarrier( 25, rockLayerArea, BarrierColor.White );
			this.LavaLayerGate = GateBarrier.CreateGateBarrier( 40, lavaLayerArea, BarrierColor.Yellow );
			this.UnderworldGate = GateBarrier.CreateGateBarrier( 75, underworldArea, BarrierColor.Red );
		}
	}
}