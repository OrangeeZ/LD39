using UnityEngine;
using System.Collections;
using System.Linq;

[CreateAssetMenu( menuName = "Create/Lists/NPC List" )]
public class NPCListInfo : ScriptableObject {

	public NPCInfo[] Infos;

#if UNITY_EDITOR
	[ContextMenu( "Find all NPC infos" )]
	private void FindAllInfos() {

		Infos = AssetHelper.GetAllAssetsOfType<NPCInfo>().ToArray();
	}
#endif
}