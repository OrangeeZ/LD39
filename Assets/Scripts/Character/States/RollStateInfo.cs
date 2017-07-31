using System;
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
            return Input.GetButton("Roll");
        }

        public override IEnumerable GetEvaluationBlock()
        {
            var speed = character.Status.MoveSpeed.Value * character.StatModifier;
            var maxSpeed = speed;
            
            var impulse = 0f;

            character.Pawn.SetDamageSphereActive(true);

            var moveDirection = Vector3.right;

            character.EnableWeaponStateController = false;
            character.Pawn.SetHeightNormalized(0.5f);
            
            while (CanBeSet())
            {
                moveDirection = Input.GetAxis("Horizontal") * Vector3.right;

                speed += deltaTime * moveDirection.x.Abs() * maxSpeed;
                
                if (moveDirection.x.Abs() < float.Epsilon)
                {
                    speed -= deltaTime * character.Status.Info.RollDeceleration;
                }
                
                speed = speed.Clamped(0, maxSpeed);

                if (Input.GetButtonDown("Jump") && impulse == 0)
                {
                    impulse = character.Status.Info.JumpSpeed;
                }
                
                character.Pawn.SetSpeed(speed);
                character.Pawn.MoveHorizontal(moveDirection);
                character.Pawn.MoveVertical(ref impulse, deltaTime);
                
                yield return null;
            }
            
            character.Pawn.SetDamageSphereActive(false);
            character.EnableWeaponStateController = true;
            character.Pawn.SetHeightNormalized(1f);
        }
    }

    public override CharacterState GetState()
    {
        return new State(this);
    }
}