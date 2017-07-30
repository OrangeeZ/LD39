using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParticleSystem : StateMachineBehaviour
{
    [SerializeField]
    private string _particleSystemName;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        animator.transform.parent.gameObject.GetComponent<ParticleSystemsController>().SetParticleSystemActive(_particleSystemName, true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        animator.transform.parent.gameObject.GetComponent<ParticleSystemsController>().SetParticleSystemActive(_particleSystemName, false);
    }
}