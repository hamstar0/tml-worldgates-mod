using System;
using System.Collections.Generic;
using System.Linq;


namespace WorldGates {
	public class WorldGatesAPI {
		public static IEnumerable<GateBarrier> GetGateBarriers() {
			return SoulBarriers.SoulBarriersAPI.GetWorldBarriers()
				.Where( b => b is GateBarrier )
				.Select( b => b as GateBarrier );
		}
	}
}
