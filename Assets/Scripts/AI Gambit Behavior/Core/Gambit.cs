using System;
using UnityEngine;
using System.Collections;

namespace AI.Gambits {

	public abstract class Gambit {

		protected readonly Character character;

		protected Gambit( Character character ) {

			this.character = character;
		}

		public virtual bool Execute() {

			throw new NotImplementedException();
		}
	}

	public abstract class Gambit<T> : Gambit where T : GambitInfo {

		protected readonly T info;

		protected Gambit( Character character, T info ) : base( character ) {

			this.info = info;
		}
	}
}