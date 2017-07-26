using UnityEngine;
using System.Collections;
using AI.Gambits;

[CreateAssetMenu( menuName = "Create/Enemy Character Info" )]
public class EnemyCharacterInfo : CharacterInfo {

	public EnemyCharacterStatusInfo EnemyStatusInfo;

	public GambitListInfo GambitListInfo;

	public override Character GetCharacter( Vector3 startingPosition, CharacterStatusInfo replacementStatusInfo = null ) {

		var inputSource = GambitListInfo.GetGambitList();
		var pawn = Instantiate( replacementStatusInfo == null ? pawnPrefab : ( replacementStatusInfo as EnemyCharacterStatusInfo ).PawnPrefab, startingPosition, Quaternion.identity ) as CharacterPawn;

		var status = replacementStatusInfo == null ? EnemyStatusInfo.GetInstance() : replacementStatusInfo.GetInstance();

		var result = new Character(
			pawn,
			inputSource,
			status,
			stateControllerInfo.GetStateController(),
			weaponStateControllerInfo.GetStateController(),
			teamId,
			this );

		pawn.GetSphereSensor().SetRadius( ( status.Info as EnemyCharacterStatusInfo ).AggroRadius );

		inputSource.Initialize( result );

		return result;
	}

}