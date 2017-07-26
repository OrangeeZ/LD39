using System;
using UnityEngine;

namespace AI.Gambits {

	public class GambitInfo : ScriptableObject {

		public virtual Gambit GetGambit( Character target ) {

			throw new NotImplementedException();
		}
	}
}