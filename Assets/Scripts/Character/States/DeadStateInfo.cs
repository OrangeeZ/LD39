using System.Collections;
using Packages.EventSystem;
using UniRx;
using UnityEngine;

[CreateAssetMenu(menuName = "Create/States/Dead")]
public class DeadStateInfo : CharacterStateInfo
{
    private class State : CharacterState<DeadStateInfo>
    {
        public State(CharacterStateInfo info) : base(info)
        {
        }

        // public override void Initialize(CharacterStateController stateController)
        // {
        //     base.Initialize(stateController);

        //     character.Health.Where(_ => _ <= 0).Subscribe(_ => stateController.TrySetState(this));
        // }

        public override bool CheckInterrupPending()
        {
            return CanBeSet();
        }

        public override bool CanBeSet()
        {
            return character.IsDead;
        }

        public override IEnumerable GetEvaluationBlock()
        {
            character.Pawn.ClearDestination();
            
            if (stateController == character.StateController)
            {
                var deathSound = character.Status.Info.DeathSounds.RandomElement();
                if (deathSound != null)
                {
                    AudioSource.PlayClipAtPoint(deathSound, character.Pawn.position);                    
                }

                if (1f.Random() <= character.dropProbability && !character.ItemsToDrop.IsNullOrEmpty())
                {
                    character.ItemsToDrop.RandomElement().DropItem(character.Pawn.transform);
                }

                character.Pawn.MakeDead();
            }

            while (CanBeSet())
            {
                yield return null;
            }
        }
    }

    public override CharacterState GetState()
    {
        return new State(this);
    }
}