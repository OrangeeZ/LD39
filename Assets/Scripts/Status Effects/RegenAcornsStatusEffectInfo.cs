using UnityEngine;
using System.Collections;

[CreateAssetMenu( menuName = "Create/Status effects/Regen acorns" )]
public class RegenAcornsStatusEffectInfo : CharacterStatusEffectInfo {

	public AcornAmmoItemInfo AcornAmmoItemInfo;

	public override void Add( Character target ) {

		base.Add( target );

		Debug.Log( target );

		new PMonad().Add( RegenAcorns( target ) ).Execute();
	}

	private IEnumerable RegenAcorns( Character target ) {

		var timer = default ( AutoTimer );

		while ( true ) {

			var acornRegenValue = target.Status.ModifierCalculator.CalculateFinalValue( ModifierType.BaseAcornRegen, 0f );
			if ( acornRegenValue <= 0 ) {

				yield return null;

				continue;
			}

			if ( timer != null ) {

				if ( timer.ValueNormalized == 1f ) {

					var acornCount = target.Inventory.GetItemCount<AcornAmmoItemInfo.AcornAmmo>();
					if ( acornCount >= target.Status.ModifierCalculator.CalculateFinalValue( ModifierType.MaxAcorns, 0f ) ) {

						yield return null;
						continue;
					}

					target.Inventory.AddItem( AcornAmmoItemInfo.GetItem() );

					timer = null;
				}

			} else {

				var regenDuration = 1f / acornRegenValue;
				
				timer = new AutoTimer( regenDuration );
			}

			yield return null;
		}
	}

}