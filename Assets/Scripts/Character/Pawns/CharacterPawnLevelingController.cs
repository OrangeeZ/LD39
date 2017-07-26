using UnityEngine;
using System.Collections;

public class CharacterPawnLevelingController : MonoBehaviour {

	//[SerializeField]
	//private float _scalePerLevel = 0.05f;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private GameObject[] _levelingObjects;

	private int _currentLevel;

	private void Start() {

		foreach ( var each in _levelingObjects ) {

			each.SetActive( value: false );
		}
	}

	public void AddLevel( float scaleBonus ) {

		if ( _currentLevel >= _levelingObjects.Length ) {

			return;
		}

		StartCoroutine( ScaleAnimation( _levelingObjects[_currentLevel], Vector3.zero, _levelingObjects[_currentLevel].transform.localScale ) );
		StartCoroutine( ScaleAnimation( gameObject, gameObject.transform.localScale, gameObject.transform.localScale + Vector3.one * scaleBonus ) );

		_currentLevel++;
	}

	private IEnumerator ScaleAnimation( GameObject target, Vector3 from, Vector3 to ) {

		target.SetActive( true );

		var initialScale = target.transform.localScale;
		var timer = new AutoTimer( _duration );

		while ( timer.ValueNormalized != 1f ) {

			target.transform.localScale = Vector3.Lerp( from, to, Mathf.Sqrt( timer.ValueNormalized ) );

			yield return null;
		}
	}

}