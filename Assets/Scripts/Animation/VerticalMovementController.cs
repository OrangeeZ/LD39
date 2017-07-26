using UnityEngine;
using System.Collections;

namespace AnimationHelper {

	[ExecuteInEditMode]
	public class VerticalMovementController : MonoBehaviour {

		public float minY;

		public float maxY;

		public float normalizedValue = 0f;

		void OnValidate() {

			UpdatePosition();
		}

		void Update() {

			UpdatePosition();
		}

		private void UpdatePosition() {

			var oldPosition = transform.localPosition;
			transform.localPosition = new Vector3( oldPosition.x, Mathf.Lerp( minY, maxY, normalizedValue ) * transform.localScale.y, oldPosition.z );
		}
	}
}