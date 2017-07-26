using UnityEngine;

public class SpriteRendererSorter : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private int _bias = 0;

	// Update is called once per frame
	private void Update() {

		if ( !transform.hasChanged ) {

			return;
		}

		transform.hasChanged = false;

		_spriteRenderer.sortingOrder = Mathf.RoundToInt( -( _bias + _spriteRenderer.transform.position.z * 100f ) );
	}

	private void Reset() {

		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

}