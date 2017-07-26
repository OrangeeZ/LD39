using System;
using UnityEngine;
using System.Collections;
using csv;

[CreateAssetMenu( menuName = "Create/Items/Healing item" )]
public class HealingItemInfo : ItemInfo, ICsvConfigurable {

	public float MaxHealthBonus = 1;
	public float HealingDuration;
	public float HealingPerSecond;

	public ModifierType ModifierType;

	public class HealingItem : Item {

		public HealingItem( ItemInfo info ) : base( info ) {

		}

		public override void Apply() {

			var info = this.info as HealingItemInfo;

			Character.Status.MaxHealth.Value += info.MaxHealthBonus;

			if ( info.HealingDuration > 0 ) {

				new PMonad().Add( HealingCoroutine( info.HealingDuration, info.HealingPerSecond ) ).Execute();
			}

			Character.Inventory.RemoveItem( this );
		}

		private IEnumerable HealingCoroutine( float duration, float speed ) {

			yield return null;

			var timer = new AutoTimer( duration );
			while ( timer.ValueNormalized != 1f ) {

				Character.Heal( speed * Time.deltaTime );

				yield return null;
			}
		}

	}

	public override Item GetItem() {

		return new HealingItem( this );
	}

	public void Configure( Values values ) {

		MaxHealthBonus = values.Get( "HpMax", 0f );
		HealingDuration = values.Get( "HealDuration", 0f );
		HealingPerSecond = values.Get( "HpPerSec", 0f );

		var id = values.Get( "id", string.Empty );
		if ( !id.IsNullOrEmpty() ) {

			ModifierType = id == "water" ? ModifierType.WaterHealthRestore : ModifierType.ManureHealthRestore;
		}

		Name = values.Get( "Name", string.Empty );
	}

}