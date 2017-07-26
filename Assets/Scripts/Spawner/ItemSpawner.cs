using UnityEngine;
using System.Collections;

public class ItemSpawner : AObject {

	public ItemInfo info;

	private void Start() {

		Instantiate( info.groundView, position: this.position ).item = info.GetItem();
	}

	private void OnValidate() {

		name = string.Format( "Item Spawner [{0}]", info == null ? "null" : info.name );
	}

}