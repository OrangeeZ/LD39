using System;
using UniRx;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Channels;

[CreateAssetMenu(menuName = "Create/States/Attack target")]
public class AttackStateInfo : CharacterStateInfo
{
    [SerializeField]
    private bool _attackOnce = false;

    [Serializable]
    public class State : CharacterState<AttackStateInfo>
    {
        private Character target;

        public State(CharacterStateInfo info) : base(info)
        {
        }

        public void SetTarget(Character targetCharacter)
        {
            if (targetCharacter != character)
            {
                target = targetCharacter;
            }
        }

        public override bool CanBeSet()
        {
            var weapon = GetCurrentWeapon();

            return target != null && weapon != null && weapon.CanAttack(target) && !weapon.IsReloading;
        }

        public override IEnumerable GetEvaluationBlock()
        {
            var weapon = GetCurrentWeapon();

            var animationEventController = character.Pawn._weaponAnimationController.GetComponent<AnimationEventController>();

            while (CanBeSet())
            {
               
                if (animationEventController != null)
                {
                    var animationTrigger = false;
                    animationEventController.AnimationEvents.Take(1).Subscribe(_ => animationTrigger = true);
                    
                    while (!animationTrigger)
                    {
                        yield return null;
                    }
                }
                
                var direction = target.Pawn.position - character.Pawn.position;

                weapon.Attack(direction.normalized);

                character.Pawn.UpdateSpriteAnimationDirection(direction.normalized);

                yield return null;

                if (typedInfo._attackOnce)
                {
                    break;
                }
            }
        }

        private RangedWeaponInfo.RangedWeapon GetCurrentWeapon()
        {
            return character.Inventory.GetArmSlotItem(ArmSlotType.Primary) as RangedWeaponInfo.RangedWeapon
                ?? character.Inventory.GetArmSlotItem(ArmSlotType.Secondary) as RangedWeaponInfo.RangedWeapon;
        }
    }

    public override CharacterState GetState()
    {
        return new State(this);
    }
}