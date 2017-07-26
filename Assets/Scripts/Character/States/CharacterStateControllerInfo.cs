using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;

[CreateAssetMenu( menuName = "Create/State controller" )]
public class CharacterStateControllerInfo : ScriptableObject {

	[Serializable]
	private class StateTransitionEntry {

		public CharacterStateInfo stateInfo;
		public bool[] transitionMask;
	}


    public bool updateAnimation = false;

    public bool isDebug = false;

	[SerializeField]
	private List<StateTransitionEntry> entries;

	public CharacterStateController GetStateController() {

		var result = new CharacterStateController {

			debug = isDebug,
            updateAnimation = updateAnimation,
			states = entries.Select( _ => _.stateInfo.GetState() ).ToArray(),
		};

		foreach ( var each in result.states ) {

			var transitionEntry = entries[result.states.IndexOf( each )];

			each.SetTransitionStates( result.states
				.EquiZip( transitionEntry.transitionMask, ( a, b ) => new { item1 = a, item2 = b } )
				.Where( _ => _.item2 && _.item1 != each )
				.Select( _ => _.item1 ) );
		}

		return result;
	}
}
