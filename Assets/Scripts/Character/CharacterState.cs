using System.Collections.Generic;
using System.Linq;
using System.Collections;

public abstract class CharacterState
{
    public readonly CharacterStateInfo Info;

    public IList<CharacterState> PossibleStates => _possibleStates;

    private readonly List<CharacterState> _possibleStates = new List<CharacterState>();

    protected Character character => stateController.character;

    protected CharacterStateController stateController;

    protected float DeltaTime;

    protected CharacterState(CharacterStateInfo info)
    {
        Info = info;
    }

    public void SetDeltaTime(float deltaTime)
    {
        DeltaTime = deltaTime;
    }

    public void SetTransitionStates(IEnumerable<CharacterState> states)
    {
        _possibleStates.AddRange(states);
    }

    public CharacterState GetNextState()
    {
        return _possibleStates.FirstOrDefault(which => which.CanBeSet());
    }

    public virtual void Initialize(CharacterStateController stateController)
    {
        this.stateController = stateController;
    }

    public virtual bool CanSwitchTo(CharacterState nextState)
    {
        return _possibleStates.Contains(nextState);
    }

    public virtual bool CanBeSet()
    {
        return true;
    }

    public virtual bool CheckInterrupPending()
    {
        return false;
    }

    public virtual IEnumerable GetEvaluationBlock()
    {
        yield return null;
    }

    public void UpdateAnimator()
    {
        if (character.Pawn != null)
        {
            OnAnimationUpdate(character.Pawn);
        }
        //stateController.character.Pawn.animatedView.Do( OnAnimationUpdate );
    }

    public void OnStateFinishAnimation()
    {
        var pawn = character.Pawn;

        if (Info.IsAnimationExclusive)
        {
            pawn?._animationController?.SetBool(Info.animatorStateName, false);
        }
        else
        {
            pawn?._animationController?.SetBoolInclusive(Info.animatorStateName, false);
        }

        if (Info.IsWeaponAnimationExclusive)
        {
            pawn?._weaponAnimationController?.SetBool(Info.weaponAnimatorStateName, false);
        }
        else
        {
            pawn?._weaponAnimationController?.SetBoolInclusive(Info.weaponAnimatorStateName, false);
        }
    }

    protected virtual void OnAnimationUpdate(CharacterPawn pawn)
    {
        if (Info.IsAnimationExclusive)
        {
            pawn?._animationController?.SetBool(Info.animatorStateName, true);
        }
        else
        {
            pawn?._animationController?.SetBoolInclusive(Info.animatorStateName, true);
        }

        if (Info.IsWeaponAnimationExclusive)
        {
            pawn?._weaponAnimationController?.SetBool(Info.weaponAnimatorStateName, true);
        }
        else
        {
            pawn?._weaponAnimationController?.SetBoolInclusive(Info.weaponAnimatorStateName, true);
        }
    }
}

public class CharacterState<T> : CharacterState where T : CharacterStateInfo
{
    protected readonly T typedInfo;

    public CharacterState(CharacterStateInfo info) : base(info)
    {
        typedInfo = info as T;
    }
}