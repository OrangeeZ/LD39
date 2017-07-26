using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RandomFactory<T> {
	
	private float _lowwater;   // Random.Range( X, ..)
	private float _hightwater; // Random.Range( .., X)
	private float[] _ranges;   // array of ranges for search in
	private T[] _objects;      // a store for objects


	public RandomFactory () {}


	public void Initialize (Dictionary<T, float> chanceTable, float chanceToGetNothing = -1.0f)
	{
		// drop keys with value <= 0f
		var verifiedChanceTable = new Dictionary<T, float> ();
		foreach (var pair in chanceTable) {
			if (pair.Value > 0f) {
				verifiedChanceTable.Add(pair.Key, pair.Value);
			}
			else {
				//FIXME: notify that Key-Value pair was droped
				//       or raise an exception?
			}
		}

		// allocate memory
		var length = verifiedChanceTable.Count;
		_ranges = new float[length];
		_objects = new T[length];

		// set ranges and objects:
		int i = 0;
		float sum = 0f;
		foreach (var pair in verifiedChanceTable) {
			sum += pair.Value;
			_ranges[i] = sum;
			_objects[i] = pair.Key;
			i++;
		}

		// set values for Random.Range(...)
		_lowwater = (chanceToGetNothing > 0f) ? 0f - chanceToGetNothing : 0f;
		_hightwater = sum;
	}


	public void Initialize (RandomFactory<T> factory,
	                        Dictionary<T, float> updatedChances = default(Dictionary<T, float>),
	                        float chanceToGetNothing = -1.0f)
	{
		// Updated chances specified: generate chanceTable and intialize with it
		if (updatedChances != default(Dictionary<T, float>)) {

			// create new dict based on factory
			var chanceTable = new Dictionary<T, float> ();
			for (int i = 0; i < factory._ranges.Length; i++) {
				chanceTable.Add(factory._objects[i], factory._ranges[i]);
			}

			// update dict with updateChances
			foreach (var pair in updatedChances ) {
				chanceTable[pair.Key] = pair.Value;
			}

			// call Initialize with that dict
			this.Initialize(chanceTable, chanceToGetNothing);
		}
		// Updated chances NOT specified: just copy data from a factory
		else { 
			_lowwater = (chanceToGetNothing > 0f) ? 0f - chanceToGetNothing : factory._lowwater;
			_hightwater = factory._hightwater;
			factory._ranges.CopyTo(_ranges, factory._ranges.Length);
			factory._objects.CopyTo(_objects, factory._objects.Length);
		}
	}


	public T Get()
	{
		var chance = Random.Range (_lowwater, _hightwater);

		// Check a chance to get nothing
		if (chance <= 0f) {
			return default(T);
		}

		// Binary search here
		int low = 0;
		int hig = _ranges.Length - 1;
		int mid = 0;
		while (low <= hig) {
			mid = low + (hig - low) / 2;
			if (chance == _ranges[mid]) {
				return _objects[mid];
			} else if (chance < _ranges[mid]) {
				hig = mid - 1;
			} else {
				low = mid + 1;
			}
		}

		// Don't try to understand why I return object by low index
		return _objects[low];
	}
}