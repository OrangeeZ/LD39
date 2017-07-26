using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {

	[SerializeField]
	private Text _value;

	[SerializeField]
	private float _distanceDelta;
	[SerializeField]
	private float _spreadDelta = 0.5f;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private Color _enemyColor = Color.yellow;
	[SerializeField]
	private Color _playerColor = Color.red;


	public void Initialize( float damage, bool isEnemy ) {

		_value.text = damage.ToString();
		_value.color = isEnemy ? _enemyColor : _playerColor;

		transform.position += new Vector3(Random.Range(-_spreadDelta, _spreadDelta), 0);

		StartCoroutine( PlayAnimation() );
	}

	private IEnumerator PlayAnimation() {

		var from = transform.position;
		var to = transform.position + Vector3.up * _distanceDelta;

		var fromColor = _value.color;
		var toColor = _value.color;
		toColor.a = 0;

		var timer = new AutoTimer( _duration );
		while ( timer.ValueNormalized < 1 ) {

			transform.position = Vector3.Lerp( from, to, timer.ValueNormalized );
			_value.color = Color.Lerp( fromColor, toColor, timer.ValueNormalized );

			yield return null;
		}

		Destroy( gameObject );
	}
}
