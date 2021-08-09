using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.World;
using SoulBarriers;
using SoulBarriers.Barriers;
using SoulBarriers.Barriers.BarrierTypes;


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

			Barrier createWorldBarrier( int hp, Rectangle tileArea, BarrierColor color ) {
				var worldArea = new Rectangle(
					x: tileArea.X * 16,
					y: tileArea.Y * 16,
					width: tileArea.Width * 16,
					height: tileArea.Height * 16
				);

				return SoulBarriersAPI.CreateWorldBarrier(
					worldArea: worldArea,
					strength: hp,
					maxRegenStrength: hp,
					strengthRegenPerTick: (float)((double)Int32.MaxValue * 0.5d) - 1f,
					color: color,
					isSaveable: false
				);
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
			
			this.DungeonGate = createWorldBarrier(
				hp: 20,
				tileArea: dungeonArea,
				color: BarrierColor.BigBlue
			);

			this.JungleGate = createWorldBarrier(
				hp: 30,
				tileArea: jungleArea,
				color: BarrierColor.Green
			);

			this.RockLayerGate = createWorldBarrier(
				hp: 25,
				tileArea: rockLayerArea,
				color: BarrierColor.White
			);
			
			this.LavaLayerGate = createWorldBarrier(
				hp: 40,
				tileArea: lavaLayerArea,
				color: BarrierColor.Yellow
			);
			
			this.UnderworldGate = createWorldBarrier(
				hp: 75,
				tileArea: underworldArea,
				color: BarrierColor.Red
			);
		}
	}
}