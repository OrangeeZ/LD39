using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToggleRendererGroup : StateMachineBehaviour
{
    [SerializeField]
    private string _onEnterRenderGroupId;

    [SerializeField]
    private string _onExitRenderGroupId;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        var rendererGroupsController = animator.transform.parent.GetComponent<RendererGroupsController>();
        rendererGroupsController.SetRendererGroupActive(_onEnterRenderGroupId);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        var rendererGroupsController = animator.transform.parent.GetComponent<RendererGroupsController>();
        rendererGroupsController.SetRendererGroupActive(_onExitRenderGroupId);
    }
}