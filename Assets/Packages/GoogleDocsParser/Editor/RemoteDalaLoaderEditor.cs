using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using MoreLinq;
using System.Linq;

[CustomEditor(typeof(RemoteDataLoader))]
public class RemoteDalaLoaderEditor : Editor
{
    private SerializedProperty _url;
    private SerializedProperty _pageId;
    private SerializedProperty _type;
    private SerializedProperty _postfix;
    private SerializedProperty _mode;

    private int _selectedType;
    private string[] _types;

    private void OnEnable()
    {
        _url = serializedObject.FindProperty("_url");
        _pageId = serializedObject.FindProperty("_pageId");
        _type = serializedObject.FindProperty("_type");
        _postfix = serializedObject.FindProperty("_postfix");
        _mode = serializedObject.FindProperty("_mode");

        var types = typeof(ICsvConfigurable).Assembly.GetExportedTypes();
        _types = types.Where(t => t.IsSubclassOf(typeof(ScriptableObject))).Select(t => t.FullName).ToArray();
        _selectedType = ArrayUtility.IndexOf(_types, _type.stringValue);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_url);
        EditorGUILayout.PropertyField(_pageId);
        EditorGUILayout.PropertyField(_postfix);
        EditorGUILayout.PropertyField(_mode);

        EditorGUILayout.PropertyField(_type);

        var selected = EditorGUILayout.Popup(_selectedType, _types);
        if (selected != _selectedType)
        {
            _selectedType = selected;
            if (_selectedType != -1)
            {
                _type.stringValue = _types[_selectedType];
            }
        }

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Load"))
        {
            var remoteDataLoader = target as RemoteDataLoader;
            
            remoteDataLoader.ParseRemoteObjectData();

            if (!EditorApplication.isCompiling)
            {
                remoteDataLoader.LoadRemoteData();
            }
        }
    }
}