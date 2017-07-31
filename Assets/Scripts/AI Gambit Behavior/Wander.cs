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

            public WanderGambit(Character character, Wander info) : base(character, info)
            {
            }

            public override bool Execute()
            {
                if (Time.time - _lastExecutionTime < Info.interval)
                {
                    return false;
                }
                
                Debug.Log("Execute");

                var point = Character.Pawn.position + Vector3.right * Random.Range(-Info.radius, Info.radius);
Debug.Log(Random.Range(-Info.radius, Info.radius));
                Character.StateController.GetState<ApproachTargetStateInfo.State>().SetDestination(point);

                _lastExecutionTime = Time.time;

                return true;
            }
        }

        public override Gambit GetGambit(Character target)
        {
            return new WanderGambit(target, this);
        }
    }
}