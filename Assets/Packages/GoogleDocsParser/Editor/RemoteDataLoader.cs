using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

#endif

public enum CsvParseRegime
{
	ObjectPerRow,
	ObjectPerTable,
}

[CreateAssetMenu( fileName = "Remote Data Loader", menuName = "Data/Remote Data Loader" )]
public partial class RemoteDataLoader : ScriptableObject {

	[SerializeField]
	private string _url = "https://docs.google.com/spreadsheets/d/1vewdQjSxYpgDuyxaV5uC2182Y4kxRRVK3r6u-c_Whp8/export?format=csv";

	[SerializeField]
	private string _pageId = string.Empty;

	[SerializeField]
	private string type = string.Empty;

	[SerializeField]
	private string postfix = string.Empty;

	[SerializeField]
	private CsvParseRegime _regime = CsvParseRegime.ObjectPerRow;
}

#if UNITY_EDITOR
public partial class RemoteDataLoader {

	[MenuItem("Tools/Load all remote data")]
	public static void LoadAllRemoteData() {
		
		var loaders = AssetHelper.GetAllAssetsOfType<RemoteDataLoader>();
		foreach ( var each in loaders ) {
			
			each.LoadRemoteData();
		}

		Debug.Log( "Finished loading remote data" );
	}
	
	[ContextMenu( "Load remote data" )]
	public void LoadRemoteData() {

		string url = _url;
		if (!string.IsNullOrEmpty(_pageId)) {
			url = url + "&gid=" + _pageId;
		}

		GoogleDocsCsvParser.Get( url, type, _regime, postfix );
	}
}

#endif