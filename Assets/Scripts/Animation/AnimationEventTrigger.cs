using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : StateMachineBehaviour
{
    [SerializeField]
    private string _onStateEnterEventName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        animator.GetComponent<AnimationEventController>()?.NotifyEventTriggered(_onStateEnterEventName);
    }
}