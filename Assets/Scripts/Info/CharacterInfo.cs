using UnityEngine;

[CreateAssetMenu(menuName = "Create/Character Info")]
public class CharacterInfo : ScriptableObject
{
    public CharacterStatusInfo statusInfo;

    public CharacterStateControllerInfo stateControllerInfo;
    public CharacterStateControllerInfo weaponStateControllerInfo;

    public int teamId = 0;

    public bool applyColor = true;

    public virtual Character GetCharacter(Vector3 startingPosition, CharacterStatusInfo replacementStatusInfo = null)
    {
        var currentStatusInfo = replacementStatusInfo == null ? statusInfo : replacementStatusInfo;
        var inputSource = new ClickInputSource();
        var pawn = Instantiate(currentStatusInfo.PawnPrefab, startingPosition, Quaternion.identity);

        var result = new Character(
            pawn,
            inputSource,
            currentStatusInfo.GetInstance(),
            stateControllerInfo.GetStateController(),
            weaponStateControllerInfo.GetStateController(),
            teamId,
            this);

        return result;
    }
}