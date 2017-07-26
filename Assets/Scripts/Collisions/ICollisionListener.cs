using UnityEngine;
using System.Collections;

public interface ICollisionListener<in T> {

	void OnCollisionEntered( T other );
}
