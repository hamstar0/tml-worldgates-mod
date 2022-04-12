using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Services.Timers;


namespace WorldGates {
	class WorldGatesWorld : ModWorld {
		private bool IsInitialized;



		////////////////

		public override void Initialize() {
			this.IsInitialized = false;
		}

		////////////////

		public override void Load( TagCompound tag ) {
			if( !tag.ContainsKey("gates_initialized") ) {
				Timers.RunNow( () => {  //<- be sure SoulBarriers gets to load, first
					this.LoadGates();

					this.IsInitialized = true;
				} );
			}
		}

		private void LoadGates() {
			var presets = GateBarrierPresets.Instance;

			//

			bool hasExistingGates = presets.InitializeAllGatesFromExistingBarriers();

			//

			if( !hasExistingGates ) {
				presets.GetGatePositions(
					out Rectangle dungeonArea,
					out Rectangle jungleArea,
					out Rectangle rockLayerArea,
					out Rectangle lavaLayerArea,
					out Rectangle underworldArea,
					out Rectangle skyArea
				);

				presets.InitializeAllGates(
					dungeonArea: dungeonArea,
					jungleArea: jungleArea,
					rockLayerArea: rockLayerArea,
					lavaLayerArea: lavaLayerArea,
					underworldArea: underworldArea,
					skyArea: skyArea
				);
			}
		}

		////

		public override TagCompound Save() {
			return new TagCompound { { "gates_initialized", this.IsInitialized } };
		}


		////////////////

		public override void PostWorldGen() {
			var presets = GateBarrierPresets.Instance;

			//

			presets.GetGatePositions(
				out Rectangle dungeonArea,
				out Rectangle jungleArea,
				out Rectangle rockLayerArea,
				out Rectangle lavaLayerArea,
				out Rectangle underworldArea,
				out Rectangle skyArea
			);

			//

			presets.InitializeAllGates(
				dungeonArea: dungeonArea,
				jungleArea: jungleArea,
				rockLayerArea: rockLayerArea,
				lavaLayerArea: lavaLayerArea,
				underworldArea: underworldArea,
				skyArea: skyArea
			);
		}
	}
}