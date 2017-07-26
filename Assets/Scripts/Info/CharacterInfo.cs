using UnityEngine;

[CreateAssetMenu( menuName = "Create/Character Info" )]
public class CharacterInfo : ScriptableObject {

    public CharacterStatusInfo statusInfo;

    public CharacterStateControllerInfo stateControllerInfo;
    public CharacterStateControllerInfo weaponStateControllerInfo;
    public CharacterPawn pawnPrefab;

    public int teamId = 0;

    public bool applyColor = true;

    public virtual Character GetCharacter( Vector3 startingPosition, CharacterStatusInfo replacementStatusInfo = null ) {

        var inputSource = new ClickInputSource();
        var pawn = Instantiate( pawnPrefab, startingPosition, Quaternion.identity ) as CharacterPawn;

        var result = new Character(
            pawn,
            inputSource,
			replacementStatusInfo == null ? statusInfo.GetInstance() : replacementStatusInfo.GetInstance(),
            stateControllerInfo.GetStateController(),
            weaponStateControllerInfo.GetStateController(),
            teamId,
            this );

        return result;
    }
}