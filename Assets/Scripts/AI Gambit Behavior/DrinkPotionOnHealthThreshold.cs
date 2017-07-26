using System;
using System.Linq;
using UniRx;
using UnityEngine;
using System.Collections;

namespace AI.Gambits {

	//[Category( "Gambits" )]
	[CreateAssetMenu( menuName = "Create/Gambits/Drink potion on health threshold" )]
	public class DrinkPotionOnHealthThreshold : GambitInfo {

		[Range( 0, 1f )]
		[SerializeField]
		private float threshold = .5f;

		private class DrinkPotionGambit : Gambit {

			private readonly DrinkPotionOnHealthThreshold info;

			private bool canActivate = false;

			public DrinkPotionGambit( Character character, DrinkPotionOnHealthThreshold info )
				: base( character ) {

				this.info = info;
				character.Health.Subscribe( UpdateCanActivate );
			}

			public override bool Execute() {

				if ( !canActivate ) {

					return false;
				}

				var item = character.Inventory.GetItems().OfType<HealingItemInfo.HealingItem>().FirstOrDefault();

				if ( item == null ) {

					return false;
				}

				item.Apply();

				return true;
			}

			private void UpdateCanActivate( float health ) {

				canActivate = health / character.Status.MaxHealth.Value <= info.threshold;
			}
		}

		public override Gambit GetGambit( Character target ) {

			return new DrinkPotionGambit( target, this );
		}
	}
}