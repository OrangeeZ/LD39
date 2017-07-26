using UnityEngine;
using System.Collections;

public class CharacterSpriteAnimationController : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Sprite[] _sprites;

	[SerializeField]
	private Animator _animator;

	private int _currentIndex;

	public void UpdateDirection( int directionX, int directionY ) {

		if ( _animator != null ) {
			
			_animator.SetInteger( "X", directionX );
			_animator.SetInteger( "Y", directionY );

			return;
		}

		_currentIndex = ( directionY + 1 ) * 3 + ( directionX + 1 );

		_spriteRenderer.sprite = _sprites[_currentIndex];
	}

}