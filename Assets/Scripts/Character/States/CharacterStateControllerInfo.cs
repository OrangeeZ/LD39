using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Create/State controller")]
public class CharacterStateControllerInfo : ScriptableObject
{
    [SerializeField]
    private AnimatorController _stateController;

    public bool UpdateAnimation = false;

    public bool IsDebug = false;

    public CharacterStateController GetStateController()
    {
        var stateMachine = _stateController.layers[0].stateMachine;
        var stateBehaviourMapping = new Dictionary<AnimatorState, CharacterState>();
        var states = stateMachine.states.Select(_ => _.state).ToArray();
        
        foreach (var each in states)
        {
            var behaviour = each.behaviours.First() as StateMachineStateInfoProvider;
            stateBehaviourMapping[each] = behaviour.GetState();
        }

        foreach (var each in states)
        {
            var targetStates = each.transitions
                .Select(_ => _.destinationState)
                .Select(_ => stateBehaviourMapping[_]);

            Debug.Log("Transitions from " + each + ":");
            Debug.Log(targetStates.Aggregate(string.Empty, (total, _) => total + " " + _));
            
            stateBehaviourMapping[each].SetTransitionStates(targetStates);
        }

        var result = new CharacterStateController
        {
            IsDebug = IsDebug,
            UpdateAnimation = UpdateAnimation,
            states = stateBehaviourMapping.Values.ToArray(),
        };

        Debug.Log(stateBehaviourMapping.Values.Aggregate(string.Empty, (total, each) => total + " " + each));

        return result;
    }
}