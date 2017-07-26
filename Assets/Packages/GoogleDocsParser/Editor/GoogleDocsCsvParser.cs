using System;
using System.IO;
using System.Threading;
using CsvHelper;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using csv;

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
                        ParseCsv2(sr, type, postfix);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
//            });
    }

    public void GenerateInfoFiles()
    {
        foreach (var each in _loadedObjects)
        {
            var instance = GetOrCreate(_type, each.Key);
            var csvConfigurable = instance as ICsvConfigurable;
            
            ParseObjectFieldsAndProperties(csvConfigurable, each.Value);
            ProcessObject(csvConfigurable, each.Value);
            
            Debug.LogFormat("Data object '{0}' saved to \"{1}\"", instance.name, AssetDatabase.GetAssetPath(instance));
        }
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
            
            Debug.Log(each + ":" + fieldType);
        }

        var path = Application.dataPath + "/Scripts/Info/" + _type + ".cs";
        ObjectInfoCodeGenerator.Generate(path, _type, _fieldNames, fieldTypes);
    }

    private void ParseCsv2(TextReader csvReader, string type, string postfix)
    {
        var parser = new CsvParser(csvReader);
        var row = parser.Read(); // get first row and

        if (string.IsNullOrEmpty(type))
        {
            // Read Type info
            if (row[0] == "type")
            {
                type = row[1];

                row = parser.Read();
            }
            else
            {
                Debug.LogError("Worksheet must declare 'Type' in first wor");
                return;
            }
        }

        // Read fields
        while (row != null && row[0] != "ID")
        {
            row = parser.Read();
        }
        if (row == null)
        {
            Debug.LogError("Can't find header!");
            return;
        }

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
            _loadedObjects.Add(instanceName, CreateValues(_fieldNames, row));
            
            row = parser.Read();
        }
    }

    private static Values CreateValues(IList<string> fieldNames, string[] row)
    {
        var dict = new Dictionary<string, string>();

        for (var i = 1; i < row.Length; i++)
        {
            if (dict.ContainsKey(fieldNames[i]))
            {
                Debug.LogFormat("They key is duplicate: {0}:{1}", fieldNames[i], row[i]);
                continue;
            }

            var lowerRow = row[i].ToLower();
            if (lowerRow == "yes") row[i] = "true";
            if (lowerRow == "no") row[i] = "false";

            dict[fieldNames[i].TrimEnd(' ')] = row[i];
        }

        return new Values(dict);
    }

    private static ScriptableObject GetOrCreate(string typeName, string instanceName)
    {
        var assembly = Assembly.GetAssembly(typeof(ICsvConfigurable));
        var type = assembly.GetExportedTypes()
            .First((x) => x.FullName.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));
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
        var attribute = fieldInfo.GetCustomAttributes(typeof(RemotePropertyAttribute), inherit: false)
            .OfType<RemotePropertyAttribute>().FirstOrDefault();

        if (attribute != null)
        {
            var value = values.Get<string>(attribute.PropertyName, string.Empty);

            var fieldValue = Convert.ChangeType(value, fieldInfo.FieldType);

            fieldInfo.SetValue(target, fieldValue);
        }
    }
}