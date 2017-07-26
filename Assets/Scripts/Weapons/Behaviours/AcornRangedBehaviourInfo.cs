using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu( menuName = "Create/Weapon/Behaviours/Acorn" )]
public class AcornRangedBehaviourInfo : RangedWeaponBehaviourInfo {

	private class AcornRangedBehaviour : RangedWeaponBehaviour {

		private float _nextAttackTime;
		private RangedWeaponInfo.RangedWeapon _ownerWeapon;
		private IInventory _ownerInventory;

		public override bool IsReloading {
			get { return Time.timeSinceLevelLoad < _nextAttackTime || _ownerInventory.GetItemCount<AcornAmmoItemInfo.AcornAmmo>() == 0; }
			protected set { }
		}

		public override void Initialize( IInventory ownerInventory, RangedWeaponInfo.RangedWeapon ownerWeapon ) {

			_ownerWeapon = ownerWeapon;
			_ownerInventory = ownerInventory;
		}

		public override bool TryShoot() {

			if ( !IsReloading ) {
				
				_nextAttackTime = Time.timeSinceLevelLoad + _ownerWeapon.BaseAttackSpeed;

				_ownerInventory.RemoveItem<AcornAmmoItemInfo.AcornAmmo>();
			    return true;
			}
            return false;
		}

	}

	public override RangedWeaponBehaviour GetBehaviour() {
		
		return new AcornRangedBehaviour();
	}

}
