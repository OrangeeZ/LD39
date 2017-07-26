using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFadeAnimation : ScreenAnimation {

    public float duration = 0.4f;

    public bool isReverse = false;

    [SerializeField]
    private Graphic[] _graphics;

    private void Reset() {

        this.GetComponent( out screen );
        this.GetComponentsInChildren( out _graphics );
    }

    private IEnumerable LerpValue( System.Action<float> setter, float from, float to, float duration ) {

        var timer = 0f;

        setter( from );

        yield return null;

        while ( timer < duration ) {

            setter( Mathf.Lerp( from, to, timer / duration ) );

            timer += Time.unscaledDeltaTime;

            yield return null;
        }

        setter( to );
    }

    public override IEnumerable GetAnimation( float? overrideDuration ) {

        var targetDuration = overrideDuration ?? duration;

        return new PMonad().Add( LerpValue( UpdateGraphicsOpacity, isReverse ? 1f : 0f, isReverse ? 0f : 1f, targetDuration ) ).ToEnumerable();
    }

    private void UpdateGraphicsOpacity( float opacityValue ) {

        foreach ( var each in _graphics ) {

            each.SetColor( a: opacityValue );
        }
    }

}