using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor.Animations;
#endif

[CreateAssetMenu(menuName = "Create/State controller")]
public class CharacterStateControllerInfo : ScriptableObject
{
    
#if UNITY_EDITOR
    [SerializeField]
    private AnimatorController _stateController;
#endif
    
    public bool UpdateAnimation = false;

    public bool IsDebug = false;

    public CharacterStateController GetStateController()
    {
#if UNITY_EDITOR
        var stateMachine = _stateController.layers[0].stateMachine;
        var stateBehaviourMapping = new Dictionary<AnimatorState, CharacterState>();
        var states = stateMachine.states.Select(_ => _.state).ToArray();
        var anyStateTransitions = stateMachine.anyStateTransitions.Select(_ => _.destinationState).ToArray();

        foreach (var each in states)
        {
            var behaviour = each.behaviours.First() as StateMachineStateInfoProvider;
            stateBehaviourMapping[each] = behaviour.GetState();
        }

        foreach (var each in states)
        {
            var currentState = stateBehaviourMapping[each];

            var targetStates = each.transitions
                .Select(_ => _.destinationState)
                .Concat(anyStateTransitions)
                .Select(_ => stateBehaviourMapping[_])
                .Where(_ => _ != currentState);

            if (IsDebug)
            {
                Debug.Log("Transitions from " + each + ":"  + targetStates.Aggregate(string.Empty, (total, _) => total + " " + _));                
            }

            stateBehaviourMapping[each].SetTransitionStates(targetStates);
        }

        var result = new CharacterStateController
        {
            IsDebug = IsDebug,
            UpdateAnimation = UpdateAnimation,
            states = stateBehaviourMapping.Values.ToArray(),
        };

        return result;
#else
        return null;
#endif
    }
}