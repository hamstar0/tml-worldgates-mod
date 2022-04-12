using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.World;
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


		public void InitializeGates(
					Rectangle dungeonArea,
					Rectangle jungleArea,
					Rectangle rockLayerArea,
					Rectangle lavaLayerArea,
					Rectangle underworldArea,
					Rectangle skyArea ) {
			var config = WorldGatesConfig.Instance;

			//
			
			this.DungeonGate = GateBarrier.CreateGateBarrier(
				id: "DungeonGate",
				strength: config.Get<int>(nameof(config.DungeonGateHp)),
				tileArea: dungeonArea,
				color: Color.Blue,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.JungleGate = GateBarrier.CreateGateBarrier(
				id: "JungleGate",
				strength: config.Get<int>(nameof(config.JungleGateHp)),
				tileArea: jungleArea,
				color: new Color( 128, 255, 0 ),
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.RockLayerGate = GateBarrier.CreateGateBarrier(
				id: "RockLayerGate",
				strength: config.Get<int>(nameof(config.RockLayerGateHp)),
				tileArea: rockLayerArea,
				color: Color.White,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.LavaLayerGate = GateBarrier.CreateGateBarrier(
				id: "LavaLayerGate",
				strength: config.Get<int>(nameof(config.LavaLayerGateHp)),
				tileArea: lavaLayerArea,
				color: Color.Yellow,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.UnderworldGate = GateBarrier.CreateGateBarrier(
				id: "UnderworldGate",
				strength: config.Get<int>(nameof(config.UnderworldGateHp)),
				tileArea: underworldArea,
				color: Color.Red,
				syncFromServer: Main.netMode == NetmodeID.Server
			);
			this.SkyGate = GateBarrier.CreateGateBarrier(
				id: "SkyGate",
				strength: config.Get<int>(nameof(config.SkyGateHp)),
				tileArea: skyArea,
				color: Color.Cyan,
				syncFromServer: Main.netMode == NetmodeID.Server
			);

			//

			this.WorldGates.Add( this.DungeonGate );
			this.WorldGates.Add( this.JungleGate );
			this.WorldGates.Add( this.RockLayerGate );
			this.WorldGates.Add( this.LavaLayerGate );
			this.WorldGates.Add( this.UnderworldGate );
			this.WorldGates.Add( this.SkyGate );
		}


		public bool RegisterExistingGates(
					Rectangle dungeonArea,
					Rectangle jungleArea,
					Rectangle rockLayerArea,
					Rectangle lavaLayerArea,
					Rectangle underworldArea,
					Rectangle skyArea ) {
			this.DungeonGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( dungeonArea );
			this.JungleGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( jungleArea );
			this.RockLayerGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( rockLayerArea );
			this.LavaLayerGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( lavaLayerArea );
			this.UnderworldGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( underworldArea );
			this.SkyGate = SoulBarriers.SoulBarriersAPI.GetWorldBarrier( skyArea );

			//
			
			if( this.DungeonGate != null ) {
				this.WorldGates.Add( this.DungeonGate );
			}
			if( this.JungleGate != null ) {
				this.WorldGates.Add( this.JungleGate );
			}
			if( this.RockLayerGate != null ) {
				this.WorldGates.Add( this.RockLayerGate );
			}
			if( this.LavaLayerGate != null ) {
				this.WorldGates.Add( this.LavaLayerGate );
			}
			if( this.UnderworldGate != null ) {
				this.WorldGates.Add( this.UnderworldGate );
			}
			if( this.SkyGate != null ) {
				this.WorldGates.Add( this.SkyGate );
			}

			return this.DungeonGate != null
				&& this.JungleGate != null
				&& this.RockLayerGate != null
				&& this.LavaLayerGate != null
				&& this.UnderworldGate != null
				&& this.SkyGate != null;
		}


		////////////////

		internal void ClearAllWorldGates() {
			foreach( Barrier barrier in this.WorldGates ) {
				SoulBarriers.SoulBarriersAPI.RemoveWorldBarrier( ((GateBarrier)barrier).TileArea );
			}

			this.WorldGates.Clear();
		}
	}
}