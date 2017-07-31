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

            var weapon = GetWeapon();

            return isButtonDown && !weapon.IsReloading && (Input.mousePosition - Camera.main.WorldToScreenPoint(character.Pawn.position)).magnitude > 0.1f;
        }

        public override IEnumerable GetEvaluationBlock()
        {
            while (CanBeSet())
            {
                var weapon = GetWeapon();

                var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(character.Pawn.position);
                direction = direction.Set(z: 0).normalized;

                character.Pawn.UpdateSpriteAnimationDirection(direction);

                weapon?.Attack(direction);
                
                var directionTimer = new AutoTimer(0.1f, useUnscaledTime: true);
                while (directionTimer.HasNotExpired)
                {
                    character.Pawn.UpdateSpriteAnimationDirection(direction);

                    yield return null;
                }
                
                yield return null;
            }
        }

        private RangedWeaponInfo.RangedWeapon GetWeapon()
        {
            var slotType = Input.GetMouseButton(0) ? ArmSlotType.Primary : ArmSlotType.Secondary;

            return character.Inventory.GetArmSlotItem(slotType) as RangedWeaponInfo.RangedWeapon;
        }
    }

    public override CharacterState GetState()
    {
        return new State(this);
    }
}