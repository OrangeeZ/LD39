using UnityEngine;
using System.Collections;

namespace AI.Gambits
{
    [CreateAssetMenu(menuName = "Create/Gambits/Wander")]
    public class Wander : GambitInfo
    {
        public float radius = 5f;

        public float interval = 1f;

        private class WanderGambit : Gambit<Wander>
        {
            private float _lastExecutionTime = 0f;
            private float _lastSign;

            public WanderGambit(Character character, Wander info) : base(character, info)
            {
                _lastSign = Mathf.Sign(Random.Range(-1f, 1f));
            }

            public override bool Execute()
            {
                if (Time.time - _lastExecutionTime < Info.interval)
                {
                    return false;
                }

                _lastSign *= -1;

                var point = Character.Pawn.position + Vector3.right * Random.Range(Info.radius * 0.5f, Info.radius) * _lastSign;
                Character.StateController.GetState<ApproachTargetStateInfo.State>().SetDestination(point);

                _lastExecutionTime = Time.time + Random.Range(0, Info.interval) * 0.5f;

                return true;
            }
        }

        public override Gambit GetGambit(Character target)
        {
            return new WanderGambit(target, this);
        }
    }
}