using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;

namespace AI.Gambits {

	[CreateAssetMenu( menuName = "Create/Gambits/Attack on visible" )]
	public class AttackOnVisibleGambit : GambitInfo {

		private class GambitInternal : Gambit {
            
			private Character attackTarget;

			public GambitInternal( GambitInfo info, Character character )
				: base( character ) {
                
				character.Pawn.GetSphereSensor()
					.Select( _ => _.character )
					.Where( _=> _ != null )
					.Subscribe( OnTargetEnter );
			}

			public override bool Execute() {

				if ( attackTarget != null ) {

					character.StateController.GetState<ApproachTargetStateInfo.State>().SetDestination( attackTarget );
					character.WeaponStateController.GetState<AttackStateInfo.State>().SetTarget( attackTarget );
                    
					return true;
				}

				return false;
			}

			private void OnTargetEnter( Character target ) {

			    if ( target.TeamId != character.TeamId ) {

                    this.attackTarget = target;
			    }
			}
		}

		public override Gambit GetGambit( Character target ) {

			return new GambitInternal( this, target );
		}
	}
}