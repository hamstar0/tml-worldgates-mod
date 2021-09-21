using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.World;


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

		private void InitializeGates() {
			var config = WorldGatesConfig.Instance;
			int barrierThick = 10;

			bool isDungeonLeft = Main.dungeonX < (Main.maxTilesX / 2);
			int jungleX = WorldGatesMod.GetJungleStartCoordinate();
			int lavaLine = WorldGatesMod.GetLavaTileY();

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

			Rectangle dungeonArea = getTallBarrier( Main.dungeonX + (isDungeonLeft ? 50 : -50) );
			Rectangle jungleArea = getTallBarrier( jungleX );
			Rectangle rockLayerArea = getFatBarrier( WorldLocationLibraries.RockLayerTopTileY );
			Rectangle lavaLayerArea = getFatBarrier( lavaLine );
			Rectangle underworldArea = getFatBarrier( WorldLocationLibraries.UnderworldLayerTopTileY );
			Rectangle skyArea = getFatBarrier( WorldLocationLibraries.SkyLayerBottomTileY );
			//LogLibraries.Log( "dungeonArea: "+dungeonArea );
			//LogLibraries.Log( "jungleArea: "+jungleArea );
			//LogLibraries.Log( "rockLayerArea: "+rockLayerArea );
			//LogLibraries.Log( "lavaLayerArea: "+lavaLayerArea );
			//LogLibraries.Log( "underworldArea: "+underworldArea );

			//
			
			this.DungeonGate = GateBarrier.CreateGateBarrier(
				config.Get<int>(nameof(config.DungeonGateHp)),
				dungeonArea,
				Color.Blue,
				Main.netMode == NetmodeID.Server
			);
			this.JungleGate = GateBarrier.CreateGateBarrier(
				config.Get<int>(nameof(config.JungleGateHp)),
				jungleArea,
				new Color( 128, 255, 0 ),
				Main.netMode == NetmodeID.Server
			);
			this.RockLayerGate = GateBarrier.CreateGateBarrier(
				config.Get<int>(nameof(config.RockLayerGateHp)),
				rockLayerArea,
				Color.White,
				Main.netMode == NetmodeID.Server
			);
			this.LavaLayerGate = GateBarrier.CreateGateBarrier(
				config.Get<int>(nameof(config.LavaLayerGateHp)),
				lavaLayerArea,
				Color.Yellow,
				Main.netMode == NetmodeID.Server
			);
			this.UnderworldGate = GateBarrier.CreateGateBarrier(
				config.Get<int>(nameof(config.UnderworldGateHp)),
				underworldArea,
				Color.Red,
				Main.netMode == NetmodeID.Server
			);
			this.SkyGate = GateBarrier.CreateGateBarrier(
				config.Get<int>(nameof(config.SkyGateHp)),
				skyArea,
				Color.Cyan,
				Main.netMode == NetmodeID.Server
			);

			//

			this.WorldGates.Add( this.DungeonGate );
			this.WorldGates.Add( this.JungleGate );
			this.WorldGates.Add( this.RockLayerGate );
			this.WorldGates.Add( this.LavaLayerGate );
			this.WorldGates.Add( this.UnderworldGate );
			this.WorldGates.Add( this.SkyGate );
		}
	}
}