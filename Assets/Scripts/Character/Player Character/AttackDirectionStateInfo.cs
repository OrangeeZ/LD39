using UnityEngine;
using System.Collections;
using Packages.EventSystem;

[CreateAssetMenu(menuName = "Create/States/Attack direction")]
public class AttackDirectionStateInfo : CharacterStateInfo
{
    private class State : CharacterState<AttackDirectionStateInfo>
    {
        public State(CharacterStateInfo info) : base(info)
        {
        }

        public override bool CanBeSet()
        {
            var isButtonDown = Input.GetMouseButton(0);
            isButtonDown = isButtonDown || Input.GetMouseButton(1);

            return isButtonDown && (Input.mousePosition - Camera.main.WorldToScreenPoint(character.Pawn.position)).magnitude > 0.1f;
        }

        public override IEnumerable GetEvaluationBlock()
        {
            while (CanBeSet())
            {
                var slotType = Input.GetMouseButton(0) ? ArmSlotType.Primary : ArmSlotType.Secondary;

                var weapon = character.Inventory.GetArmSlotItem(slotType) as Weapon;

                var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(character.Pawn.position);
                direction = direction.Set(z: 0).normalized;

                weapon?.Attack(direction);

                yield return null;
            }
        }
    }

    public override CharacterState GetState()
    {
        return new State(this);
    }
}