using UnityEngine;
using System.Collections;

public class ICanBurn : MonoBehaviour
{
	public Burning defaultBurning;
	public float scaleMultiplier = 1;
	
	public void Burn()
	{
		Burn(defaultBurning);
	}

	public void Burn(Burning source)
	{
		var burning = GetComponent<Burning>();
		if (burning != null) {
			burning.stacks++;
			float factor = 1f / burning.stacks;
			burning.spreadChance = Mathf.Lerp(burning.spreadChance, source.spreadChance, factor);
			burning.enabled = true;
		} else {
			burning = gameObject.AddComponent<Burning>();

			burning.visualScaleMax = source.visualScaleMax;
			burning.scaleMultiplier = scaleMultiplier;
			burning.maxStacks = source.maxStacks;
			burning.stackDuration = source.stackDuration;
			burning.visualPrefab = source.visualPrefab;

			burning.spreadRange = source.spreadRange;
			burning.spreadChance = source.spreadChance * source.spreadLowering;
			burning.stacks = 1f;
		}
	}
}

