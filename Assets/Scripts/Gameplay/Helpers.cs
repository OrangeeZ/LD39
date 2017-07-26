using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Packages.EventSystem;

public static class Helpers {

	public struct SplashDamage : IEventBase {

		public Vector3 position;
		public float radius;

	}

	public static IEnumerable<Character> GetCharactersInCone( Vector3 origin, Vector3 direction, float distance, float coneAngle ) {

		var coneAngleRads = coneAngle * Mathf.Deg2Rad;

		var charactersInRange = Character.Instances.Where( _ => Vector3.Distance( origin, _.Pawn.position ) < distance )
			.Where( _ => Vector3.Dot( ( origin - _.Pawn.position ).normalized, direction ) < coneAngleRads );

		return charactersInRange;
		//var charactersInCone =
	}

	public static void DoSplashDamage( Vector3 point, float radius, float amount, int teamToSkip ) {

		new PMonad().Add( GradualDestroy( point, radius, amount, teamToSkip ) ).Execute();
	}

	private static IEnumerable GradualDestroy( Vector3 point, float radius, float amount, int teamToSkip ) {

		var maxObjectsPerIteration = 2;
		var objectCounter = 0;

		var affectedCharacters = Character.Instances.Where( _ => ( _.Pawn.position - point ).magnitude <= radius ).ToArray();
		foreach ( var each in affectedCharacters ) {

			if ( each.TeamId == teamToSkip ) {

				continue;
			}

			each.Damage( amount );

			objectCounter++;

			if ( objectCounter >= maxObjectsPerIteration ) {

				objectCounter = 0;

				yield return null;
			}
		}

		//var affectedBuildings = Building.instances.Where( _ => _.sphereCollider.Intersects( point, radius ) ).ToList();
		//affectedBuildings.Sort( ( a, b ) => ( a.transform.position - point ).magnitude.CompareTo( ( b.transform.position - point ).magnitude ) );
		//foreach ( var each in affectedBuildings ) {

		//	each.Hit( amount );

		//	objectCounter++;

		//	if ( objectCounter >= maxObjectsPerIteration ) {

		//		objectCounter = 0;

		//		yield return null;
		//	}
		//}
	}

}