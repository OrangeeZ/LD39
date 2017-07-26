using UnityEngine;
using System.Collections;

namespace AI.Gambits {

    [CreateAssetMenu( menuName = "Create/Gambits/Wander")]
    public class Wander : GambitInfo {

        public float radius = 5f;

        public float interval = 1f;

        private class WanderGambit : Gambit<Wander> {

            private float lastExecutionTime = 0f;

            public WanderGambit( Character character, Wander info ) : base( character, info ) {
            }

            public override bool Execute() {

                if ( Time.time - lastExecutionTime < info.interval ) {

                    return false;
                }

	            var currentRoom = Room.FindRoomForPosition( character.Pawn.position );

	            var point = currentRoom.GetRandomPoint();//character.Pawn.position + character.Pawn.rotation * Random.insideUnitCircle.normalized.ToXZ() * info.radius;
                
                character.StateController.GetState<ApproachTargetStateInfo.State>().SetDestination( point );

                lastExecutionTime = Time.time;

                return true;
            }

        }

        public override Gambit GetGambit( Character target ) {

            return new WanderGambit( target, this );
        }

    }

}