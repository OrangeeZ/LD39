using UnityEngine;
using System.Collections;
using System;

public class ProjectileColorer : MonoBehaviour
{
	[Serializable]
	public class ColorPreset
	{
		public ParticleSystem particles;
		public Color enemyColor;
		public Color allyColor;
	}

	public ColorPreset[] presets;

	public void Apply(bool isEnemy)
	{
		foreach (var item in presets) {
			item.particles.startColor = isEnemy ? item.enemyColor : item.allyColor;
		}
	}
}

