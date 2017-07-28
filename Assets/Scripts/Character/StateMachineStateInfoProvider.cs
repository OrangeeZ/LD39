using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineStateInfoProvider : StateMachineBehaviour
{
    [SerializeField]
    private CharacterStateInfo _stateInfo;
    
    public CharacterState GetState()
    {
        return _stateInfo.GetState();
    }
}