using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu(menuName = "Environment/Object spot info")]
public class EnvironmentObjectSpotInfo : ScriptableObject {

	[SerializeField]
	private GameObject[] _leftSidePrefabs;

	[SerializeField]
	private GameObject[] _rightSidePrefabs;

	[SerializeField]
	private GameObject[] _universalPrefabs;

	public GameObject GetRandomPrefab() {

		return _leftSidePrefabs.Concat( _rightSidePrefabs ).Concat( _universalPrefabs ).RandomElement();
	}

	public GameObject GetRandomLeftSidePrefab() {

		return _leftSidePrefabs.Concat( _universalPrefabs ).RandomElement();
	}

	public GameObject GetRandomRightSidePrefab() {

		return _rightSidePrefabs.Concat( _universalPrefabs ).RandomElement();
	}

}
