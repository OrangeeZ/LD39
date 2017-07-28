using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

public enum CsvParseMode
{
    ObjectPerRow,
    ObjectPerTable,
}

[CreateAssetMenu(fileName = "Remote Data Loader", menuName = "Data/Remote Data Loader")]
public class RemoteDataLoader : ScriptableObject
{
    [SerializeField]
    private string _url =
        "https://docs.google.com/spreadsheets/d/1vewdQjSxYpgDuyxaV5uC2182Y4kxRRVK3r6u-c_Whp8/export?format=csv";

    [SerializeField]
    private string _pageId = string.Empty;

    [SerializeField]
    private string _type = string.Empty;

    [SerializeField]
    private string _postfix = string.Empty;

    [SerializeField]
    private CsvParseMode _mode = CsvParseMode.ObjectPerRow;

#if UNITY_EDITOR

    [MenuItem("Tools/Load all remote data")]
    public static void LoadAllRemoteData()
    {
        var loaders = AssetHelper.GetAllAssetsOfType<RemoteDataLoader>();
        foreach (var each in loaders)
        {
            try
            {
                each.ParseRemoteObjectData();
                each.LoadRemoteData();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.Log($"Failed to load data from {each.name}");
            }
            
        }

        Debug.Log("Finished loading remote data");
    }

    [ContextMenu("Load remote data")]
    public void LoadRemoteData()
    {
        var url = _url;
        if (!string.IsNullOrEmpty(_pageId))
        {
            url = url + "&gid=" + _pageId;
        }

        var parser = new GoogleDocsCsvParser();
        parser.Load(url, _type, _mode, _postfix);
        parser.GenerateInfoFiles();
    }

    public void ParseRemoteObjectData()
    {
        var url = _url;
        if (!string.IsNullOrEmpty(_pageId))
        {
            url = url + "&gid=" + _pageId;
        }

        var parser = new GoogleDocsCsvParser();
        parser.Load(url, _type, _mode, _postfix);
        parser.GenerateInfoClassFiles();
    }

#endif
}