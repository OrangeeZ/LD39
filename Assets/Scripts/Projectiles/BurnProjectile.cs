using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class BurnProjectile : Projectile
{
	public Transform effect;
	public ParticleSystemMultiplier _scaler;
	public ScaledCurve curve;

	public float dieDelay = 1f;
	
	protected override void Release ()
	{
		effect.transform.parent = null;
		Destroy(effect.gameObject, dieDelay);

		base.Release ();
	}

	public override void OnContact (Collider other)
	{
		var burn = other.GetComponent<ICanBurn>();
		if (burn != null) {
			burn.Burn();
		}
	}

	protected override void Update ()
	{
		base.Update ();

		_scaler.SetScale(curve.Evaluate(LifeFraction));
	}
}

