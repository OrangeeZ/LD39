using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[CreateAssetMenu(menuName = "Character States/Dead Before Bite")]
public class DeadBeforeBiteStateInfo : CharacterStateInfo
{
    private class State : CharacterState<DeadBeforeBiteStateInfo>
    {
        private bool _didEnterState = false;

        public State(CharacterStateInfo info) : base(info)
        {
        }

        // public override void Initialize(CharacterStateController stateController)
        // {
        //     base.Initialize(stateController);

        //     stateController.character.Health.Subscribe(OnHealthChange);
        // }

        public override bool CheckInterrupPending()
        {
            return CanBeSet();
        }

        public override bool CanBeSet()
        {
            return !_didEnterState && character.Health.Value <= character.Status.Info.BiteStateHealthThreshold;
        }

        public override IEnumerable GetEvaluationBlock()
        {
            character.Pawn.ClearDestination();
            character.EnableWeaponStateController = false;

            _didEnterState = true;

            character.SetInvincible(true);

            var timer = new AutoTimer(character.Status.Info.BiteStateDuration);
            while (timer.ValueNormalized < 1)
            {
                DisplayBiteTimer.Instance.Display(character, timer.ValueNormalized);

                yield return null;
            }

            character.EnableWeaponStateController = true;

            character.SetInvincible(false);

            character.Heal(character.Status.Info.MaxHealth);
        }

        // private void OnHealthChange(float newHealthValue)
        // {
        //     if (CanBeSet())
        //     {
        //         stateController.TrySetState(this);
        //     }
        // }
    }

    public override CharacterState GetState()
    {
        return new State(this);
    }
}