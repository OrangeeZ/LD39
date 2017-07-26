using UnityEngine;
using System.Collections;

public class AutoSpriteSorter : MonoBehaviour {

	[SerializeField]
	private Bounds _warFogOccluderBounds;

	[SerializeField]
	private float _warFogOccluderAngle = 45f;

	[ContextMenu( "Sort" )]
	private void Sort() {
#if UNITY_EDITOR
		var sprites = GetComponentsInChildren<SpriteRenderer>();
		foreach ( var each in sprites ) {

			each.sortingOrder = Mathf.RoundToInt( -each.transform.position.z * 100f );

			if ( each.name.ToLower().Contains( "wall" ) ) {
				
				DestroyImmediate( each.GetComponent<WarFog.Occluder>() );

				var occluder = each.gameObject.AddComponent<WarFog.Occluder>();
				occluder.SetLocalBounds( _warFogOccluderBounds );
				occluder.SetAdditionalAngle( _warFogOccluderAngle );
			}

			UnityEditor.EditorUtility.SetDirty( each );
		}

		foreach ( var each in GetComponentsInChildren<TileMovement>() ) {
			
			DestroyImmediate( each );
		}

		foreach ( var each in GetComponentsInChildren<IsometricMapGenerator>() ) {

			DestroyImmediate( each );
		}
#endif
	}

}