using System.Linq;
using UnityEngine;

public static class TargetSelector {

	public static Character SelectTarget( Character currentCharacter, Vector3 direction ) {
		
		var characterToDirectionMap = Character.Instances
			.Where( _ => _ != currentCharacter )
			.Where( _ => Vector3.Distance( _.Pawn.position, currentCharacter.Pawn.position ) < 15f )
			.Select( _ => new {character = _, direction = Vector3.Dot( _.Pawn.GetDirectionTo( currentCharacter.Pawn ), direction )} )
			.Where( _ => _.direction >= 0.85f )
			.ToList();

		characterToDirectionMap.Sort( ( a, b ) => ( b.character.Pawn.position - currentCharacter.Pawn.position ).magnitude.CompareTo( ( a.character.Pawn.position - currentCharacter.Pawn.position ).magnitude ) );
		characterToDirectionMap.Sort( ( a, b ) => a.direction.CompareTo( b.direction ) );

		return characterToDirectionMap.Any() ? characterToDirectionMap.First().character : null;
	}

}