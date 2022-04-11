using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ModLibsCore.Libraries.Debug;


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
				WorldGatesMod.Instance.InitializeGates();
			}

			this.IsInitialized = true;
		}

		public override TagCompound Save() {
			return new TagCompound { { "gates_initialized", this.IsInitialized } };
		}
	}
}