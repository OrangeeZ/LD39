using System;
using System.IO;
using System.Threading;
using CsvHelper;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using csv;
using NUnit.Framework;
using Debug = UnityEngine.Debug;

public class GoogleDocsCsvParser
{
    private string[] _fieldNames;
    private Dictionary<string, Values> _loadedObjects = new Dictionary<string, Values>();
    private string _type;

    public void Load(string url, string type, CsvParseMode mode, string postfix)
    {
        EditorUtility.DisplayProgressBar("Loading", "Requesting csv file. Please wait...", 0f);
        Debug.Log("Loading csv from: " + url);

        _type = type;

        var www = new WWW(url);
        while (!www.isDone)
        {
            EditorUtility.DisplayProgressBar("Loading", "Requesting csv file. Please wait...", www.progress);

            Thread.Sleep(100);
        }
//
//        ContinuationManager.Add(() =>
//            {
//                EditorUtility.DisplayProgressBar("Loading", "Requesting csv file. Please wait...", www.progress);
//                return www.isDone;
//            },
//            () =>
//            {
        EditorUtility.ClearProgressBar();

        // Let's parse this CSV!
        TextReader sr = new StringReader(www.text);
        try
        {
            if (mode == CsvParseMode.ObjectPerRow)
            {
                ParseObjectPerRow(sr, type, postfix);
            }
            else
            {
                ParseObjectPerTable(sr, type, postfix);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
//            });
        
        www.Dispose();
    }

    [Conditional("UNITY_EDITOR")]
    public void GenerateInfoFiles()
    {
        AssetDatabase.StartAssetEditing();
        
        foreach (var each in _loadedObjects)
        {
            var instance = GetOrCreate(_type, each.Key);
            var csvConfigurable = instance as ICsvConfigurable;

            ParseObjectFieldsAndProperties(csvConfigurable, each.Value);
            ProcessObject(csvConfigurable, each.Value);

            Debug.Log($"Data object {instance.name} saved to {AssetDatabase.GetAssetPath(instance)}", instance);
        }
        
        AssetDatabase.StopAssetEditing();
    }

    public void GenerateInfoClassFiles()
    {
        var fieldTypes = new List<Type>();
        foreach (var each in _fieldNames)
        {
            var firstNonNullValue = _loadedObjects.Values
                .Select(_ => _.Get<string>(each))
                .FirstOrDefault(_ => !string.IsNullOrEmpty(_));

            var fieldType = ObjectInfoCodeGenerator.DeduceType(firstNonNullValue);
            fieldTypes.Add(fieldType);
        }

        var path = Application.dataPath + "/Scripts/Info/" + _type + ".cs";
        ObjectInfoCodeGenerator.Generate(path, _type, _fieldNames, fieldTypes);
    }

    private void ParseObjectPerRow(TextReader csvReader, string type, string postfix)
    {
        var parser = new CsvParser(csvReader);
        var row = parser.Read(); // get first row and

        //Here we need to read spreadsheet name and use it for type
//        if (string.IsNullOrEmpty(type))
//        {
//            // Read Type info
//            if (row[0] == "type")
//            {
//                row = parser.Read();
//            }
//            else
//            {
//                Debug.LogError("Worksheet must declare 'Type' in first wor");
//                return;
//            }
//        }

        _fieldNames = row;

        row = parser.Read();

        while (row != null)
        {
            if (row.Length < 2 || string.IsNullOrEmpty(row[0]))
            {
                row = parser.Read();
                continue;
            }

            var instanceName = csv.Utility.FixName(row[0], postfix);
            _loadedObjects.Add(instanceName, CreateValues(_fieldNames, row, startingIndex: 1));

            row = parser.Read();
        }
    }

    private void ParseObjectPerTable(TextReader csvReader, string type = null, string postfix = null)
    {
        var parser = new CsvParser(csvReader);

        var fieldNames = new List<string>();
        var fieldValues = new List<string>();

        var row = parser.Read(); // get first row and
        // Read fields
        while (row != null) // && row[0] != "ID")
        {
            fieldNames.Add(row[0]);
            fieldValues.Add(row[1]);

            row = parser.Read();
        }

        var instanceName = csv.Utility.FixName(type, postfix);
        _loadedObjects.Add(instanceName, CreateValues(fieldNames, fieldValues, startingIndex: 0));

        _fieldNames = fieldNames.ToArray();
    }

    private static Values CreateValues(IList<string> fieldNames, IList<string> fieldValues, int startingIndex)
    {
        var valuesDictionary = new Dictionary<string, string>();

        for (var i = startingIndex; i < fieldValues.Count; i++)
        {
            if (valuesDictionary.ContainsKey(fieldNames[i]))
            {
                Debug.LogFormat("They key is duplicate: {0}:{1}", fieldNames[i], fieldValues[i]);
                continue;
            }

            var lowerRow = fieldValues[i].ToLower();
            if (lowerRow == "yes") fieldValues[i] = "true";
            if (lowerRow == "no") fieldValues[i] = "false";

            valuesDictionary[fieldNames[i].TrimEnd(' ')] = fieldValues[i];
        }

        return new Values(valuesDictionary);
    }

    private static ScriptableObject GetOrCreate(string typeName, string instanceName)
    {
        var assembly = Assembly.GetAssembly(typeof(ICsvConfigurable));
        var type = assembly
            .GetExportedTypes()
            .First(_ => _.FullName.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));

        if (type == null)
        {
            Debug.LogWarningFormat("Type {0} not found", typeName);
            return null;
        }

        var assetPath = Path.Combine("Assets/Data/Remote Data/", type.Name);
        var assetPathWithName = assetPath + "/" + instanceName + ".asset";

        var instance = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPathWithName);

        if (instance == null)
        {
            instance = ScriptableObject.CreateInstance(type);
            Directory.CreateDirectory(assetPath).Attributes = FileAttributes.Normal;
            AssetDatabase.CreateAsset(instance, assetPathWithName);
        }

        EditorUtility.SetDirty(instance);

        return instance;
    }

    private static void ProcessObject(ICsvConfigurable target, Values values)
    {
        ParseObjectFieldsAndProperties(target, values);
        target.Configure(values);
    }

    private static void ParseObjectFieldsAndProperties(ICsvConfigurable target, Values values)
    {
        var type = target.GetType();

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

        foreach (var each in fields)
        {
            ParseObjectField(target, each, values);
        }
    }

    private static void ParseObjectField(ICsvConfigurable target, FieldInfo fieldInfo, Values values)
    {
        var attribute = fieldInfo
            .GetCustomAttributes(typeof(RemotePropertyAttribute), inherit: false)
            .OfType<RemotePropertyAttribute>()
            .FirstOrDefault();

        if (attribute != null)
        {
            var propertyName = string.IsNullOrEmpty(attribute.PropertyName) ? fieldInfo.Name : attribute.PropertyName;
            var value = values.Get(propertyName, string.Empty);

            var fieldValue = default(object);

            if (value == string.Empty && fieldInfo.FieldType.IsValueType)
            {
                fieldValue = Activator.CreateInstance(fieldInfo.FieldType);
            }
            else
            {
                fieldValue = Convert.ChangeType(value, fieldInfo.FieldType);
            }

            fieldInfo.SetValue(target, fieldValue);
        }
    }
}