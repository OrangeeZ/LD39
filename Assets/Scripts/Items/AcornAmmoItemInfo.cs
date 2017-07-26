using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Create/Weapons/Ammo/Acorn")]
public class AcornAmmoItemInfo : ItemInfo {

	public class AcornAmmo : Item {

		public AcornAmmo( ItemInfo info ) : base( info ) {

		}

		public override void Apply() {
		}

	}

	public override Item GetItem() {

		return new AcornAmmo( this );
	}

}