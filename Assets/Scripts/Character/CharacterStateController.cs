using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class CharacterStateController
{
    public bool IsDebug = false;

    public bool UpdateAnimation = false;

    [HideInInspector]
    public Character character;

    public IList<CharacterState> states = null;

    private CharacterState currentState { get; set; }

    private IEnumerator evaluationBlock = null;

    private Queue<CharacterState> scheduledStates = new Queue<CharacterState>();

    public void Initialize(Character character)
    {
        this.character = character;

        foreach (var each in states)
        {
            each.Initialize(this);
        }
    }

    private void GetNextState()
    {
        var oldState = currentState;

        currentState = scheduledStates.Any() ? scheduledStates.Dequeue() : currentState?.GetNextState();

        if (currentState == null)
        {
            currentState = states.FirstOrDefault(that => that.CanBeSet());
        }

        if (IsDebug && oldState != currentState)
        {
            Debug.Log($"{oldState}->{currentState}");
        }
        
        if (UpdateAnimation)
        {
            oldState?.OnStateFinishAnimation();
        }

        UpdateEvaluationBlock();
    }

    private void UpdateEvaluationBlock()
    {
        evaluationBlock = new PMonad().Add(currentState.GetEvaluationBlock()).Add(GetNextState).ToEnumerator();
    }

    public void Tick(float deltaTime)
    {
        if (CheckInterrupPending())
        {
            return;
        }

        if (evaluationBlock != null)
        {
            currentState.SetDeltaTime(deltaTime);

            evaluationBlock.MoveNext();

            if (UpdateAnimation)
            {
                currentState.UpdateAnimator();
            }
        }
        else
        {
            GetNextState();
        }
    }

    public void TrySetState(CharacterState newState, bool allowEnterSameState = false)
    {
        if (newState != currentState || !allowEnterSameState)
        {
            if ((currentState != null && !currentState.CanSwitchTo(newState)) || !newState.CanBeSet())
            {
                return;
            }
        }

        if (IsDebug)
        {
            Debug.Log($"{currentState}->{newState}");
        }

        currentState = newState;

        UpdateEvaluationBlock();
    }

    public void ForceSetState(CharacterState newState)
    {
        if (IsDebug)
        {
            Debug.Log($"{currentState}->{newState}");
        }

        currentState = newState;

        UpdateEvaluationBlock();
    }

    public void TrySetState(CharacterStateInfo newStateInfo, bool allowEnterSameState = false)
    {
        TrySetState(GetStateByInfo(newStateInfo), allowEnterSameState);
    }

    public void SetScheduledStates(IEnumerable<CharacterState> states)
    {
        scheduledStates.Clear();

        foreach (var each in states)
        {
            scheduledStates.Enqueue(each);
        }
    }

    public CharacterState GetStateByInfo(CharacterStateInfo info)
    {
        return states.FirstOrDefault(where => where.Info == info);
    }

    public T GetState<T>() where T : CharacterState
    {
        return states.OfType<T>().FirstOrDefault();
    }

    private bool CheckInterrupPending()
    {
        var possibleStates = currentState != null ? currentState.PossibleStates : null;
        if (possibleStates == null)
        {
            return false;
        }
        
        foreach (var each in possibleStates)
        {
            if (each.CheckInterrupPending())
            {
                TrySetState(each);

                return true;
            }
        }

        return false;
    }

    //public void SetEvaluationBlock( IEnumerator evaluationBlock ) {

    //	this.evaluationBlock = evaluationBlock;
    //}
}