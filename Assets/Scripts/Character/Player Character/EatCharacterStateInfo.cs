using System.Collections;
using System.Linq;
using MoreLinq;
using UnityEngine;

[CreateAssetMenu(menuName = "Character States/Eat Character")]
public class EatCharacterStateInfo : CharacterStateInfo
{
    private class State : CharacterState<EatCharacterStateInfo>
    {
        public State(CharacterStateInfo info) : base(info)
        {
        }

        public override bool CanBeSet()
        {
            //Has biteable character around

            var characterSensor = character.Pawn.GetSphereSensor();
            var nearbyCharacters = characterSensor.NearbyCharacters.Select(_ => _.Character);
            
            return CheckInterrupPending() && nearbyCharacters.Any(IsBiteable);
        }

        public override bool CheckInterrupPending()
        {
            return Input.GetButtonDown("Eat");
        }

        public override IEnumerable GetEvaluationBlock()
        {
            //Get biteable character
            //Play bite animation and wait
            
            Debug.Log("Eat character!");
            
            var characterSensor = character.Pawn.GetSphereSensor();
            var nearbyCharacters = characterSensor.NearbyCharacters.Select(_ => _.Character);
            var biteableCharacters = nearbyCharacters.Where(IsBiteable);
            var closestBiteableCharacter = biteableCharacters.MinBy(_ => Vector3.SqrMagnitude(_.Pawn.position - character.Pawn.position) );
            
            closestBiteableCharacter.Damage(int.MaxValue);
            
            character.Pawn.UpdateSpriteAnimationDirection(closestBiteableCharacter.Pawn.position - character.Pawn.position);
            
            var timer = new AutoTimer(character.Status.Info.EatStateDuration, useUnscaledTime: true);
            var powerPerSecond = character.Status.Info.PowerPerEat / character.Status.Info.EatStateDuration;
            while (timer.ValueNormalized < 1)
            {
                GameplayController.Instance.AddPower(powerPerSecond * deltaTime);

                yield return null;
            }
        }

        private static bool IsBiteable(Character targetCharacter)
        {
            return targetCharacter.Health.Value > 0 && targetCharacter.Health.Value <= targetCharacter.Status.Info.BiteStateHealthThreshold;
        }
    }

    public override CharacterState GetState()
    {
        return new State(this);
    }
}