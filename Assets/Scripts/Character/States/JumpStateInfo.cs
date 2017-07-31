using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Create/States/Jump")]
public class JumpStateInfo : CharacterStateInfo
{
    private class State : CharacterState<JumpStateInfo>
    {
        public State(CharacterStateInfo info) : base(info)
        {
        }

        public override bool CanBeSet()
        {
            return character.Pawn.IsGrounded() && Input.GetButton("Jump");
        }

        public override IEnumerable GetEvaluationBlock()
        {
            var impulse = character.Status.Info.JumpSpeed;

            while (impulse > 0)
            {
                character.Pawn.SetSpeed(character.Status.MoveSpeed.Value * character.StatModifier);
                character.Pawn.MoveHorizontal(GetMoveDirection());
                character.Pawn.MoveVertical(ref impulse, deltaTime);

                yield return null;
            }
        }

        private Vector3 GetMoveDirection()
        {
            return new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0).ClampMagnitude(1f);
        }
    }

    public override CharacterState GetState()
    {
        return new State(this);
    }
}