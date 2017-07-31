using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;

namespace AI.Gambits
{
    [CreateAssetMenu(menuName = "Create/Gambits/Attack on visible")]
    public class AttackOnVisibleGambit : GambitInfo
    {
        private class GambitInternal : Gambit
        {
            private Character _attackTarget;

            public GambitInternal(GambitInfo info, Character character)
                : base(character)
            {
                character.Pawn.GetSphereSensor()
                    .Select(_ => _.Character)
                    .Where(_ => _ != null)
                    .Subscribe(OnTargetEnter);
            }

            public override bool Execute()
            {
                if (_attackTarget != null)
                {
                    Character.StateController.GetState<ApproachTargetStateInfo.State>().SetDestination(_attackTarget);
                    Character.WeaponStateController.GetState<AttackStateInfo.State>().SetTarget(_attackTarget);

                    return true;
                }

                return false;
            }

            private void OnTargetEnter(Character target)
            {
                if (target.TeamId != Character.TeamId)
                {
                    Debug.Log(target.Pawn);
                    _attackTarget = target;
                }
            }
        }

        public override Gambit GetGambit(Character target)
        {
            return new GambitInternal(this, target);
        }
    }
}