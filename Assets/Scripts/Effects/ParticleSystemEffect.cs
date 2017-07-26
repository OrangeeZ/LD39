using UnityEngine;
using System.Collections;
using System.Linq;

public class ParticleSystemEffect : EffectsBase {

    [SerializeField]
    private ParticleSystem _particleSystemPrefab;

    [SerializeField]
    private Transform[] _emissionPoints;

    public override void Activate() {

        base.Activate();

        _emissionPoints
            .Select( _ => Instantiate( _particleSystemPrefab, _.position, _.rotation ) as ParticleSystem )
            .MapImmediate( _ => _.Play() );
    }

    [ContextMenu( "Hook" )]
    private void Hook() {

        _emissionPoints = transform.OfType<Transform>().ToArray();
    }

}