using UnityEngine;
using System.Collections;

public class DestroyOnFinishAnimationEvent : StateMachineBehaviour {

	public override void OnStateExit( Animator animator, AnimatorStateInfo stateInfo, int layerIndex ) {

		base.OnStateExit( animator, stateInfo, layerIndex );

		Destroy( animator.gameObject );
	}

	public override void OnStateUpdate( Animator animator, AnimatorStateInfo stateInfo, int layerIndex ) {

		base.OnStateUpdate( animator, stateInfo, layerIndex );

		if ( stateInfo.normalizedTime > 0.99f ) {

			Destroy( animator.gameObject );
		}
	}

}
