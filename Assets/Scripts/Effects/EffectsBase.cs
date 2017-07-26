using UnityEngine;
using System.Collections;

public class EffectsBase : AObject {

    public AudioClip clip;

    public virtual void Activate() {

        if ( clip != null ) {

            AudioSource.PlayClipAtPoint( clip, position, 0.2f );
        }
    }

}