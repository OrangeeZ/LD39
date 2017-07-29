using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(menuName = "Character States/Roll")]
public class RollStateInfo : CharacterStateInfo
{
    private class State : CharacterState<RollStateInfo>
    {
        public State(CharacterStateInfo info) : base(info)
        {
        }

        public override bool CheckInterrupPending()
        {
            return CanBeSet();
        }

        public override bool CanBeSet()
        {
            return Input.GetButtonDown("Roll");
        }

        public override IEnumerable GetEvaluationBlock()
        {
            var speed = character.Status.MoveSpeed.Value * character.StatModifier;
            var impulse = 0f;
            var moveDirection = Input.GetAxis("Horizontal") * Vector3.right;

            character.Pawn.SetDamageSphereActive(true);
            
            while (speed > 0)
            {
                character.Pawn.SetSpeed(speed);
                character.Pawn.MoveHorizontal(moveDirection);
                character.Pawn.MoveVertical(ref impulse, deltaTime);

                speed -= Time.deltaTime * character.Status.Info.RollDeceleration;
                
                yield return null;
            }
            
            character.Pawn.SetDamageSphereActive(false);
        }
    }

    public override CharacterState GetState()
    {
        return new State(this);
    }
}