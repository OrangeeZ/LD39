using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using csv;
using Microsoft.CSharp;
using UnityEngine;

public class ObjectInfoCodeGenerator
{
    public static void Generate(string path, string className, string[] fieldNames, IList<Type> fieldTypes)
    {
        if (File.Exists(path))
        {
            Debug.Log($"Skipping file generation for {className}; the file already exists");
            
            return;
        }
        
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            using (var writer = new StreamWriter(fileStream))
            {
                WriteHeader(className, writer);

                var compiler = new CSharpCodeProvider();
                
                var index = 0;
                
                foreach (var each in fieldNames)
                {
                    var fieldType = fieldTypes[index++];
                    var typeReference = new CodeTypeReference(fieldType);
                    var fieldTypeName = compiler.GetTypeOutput(typeReference);
                    
                    WriteField(fieldTypeName, each, writer);
                }
                
                WriteFooter(writer);
            }
        }
    }

    public static Type DeduceType(string value)
    {
        var possibleTypes = new[] {typeof(float), typeof(int)};

        foreach (var each in possibleTypes)
        {
            var didSucceed = true;

            try
            {
                Convert.ChangeType(value, each);
            }
            catch
            {
                didSucceed = false;
            }

            if (didSucceed)
            {
                return each;
            }
        }

        return typeof(string);
    }

    private static void WriteHeader(string className, TextWriter outputFile)
    {
        outputFile.WriteLine("using UnityEngine;");
        outputFile.WriteLine("using csv;");
        outputFile.WriteLine(string.Empty);
        
        outputFile.WriteLine($"[CreateAssetMenu(menuName = \"Info/{className}\")]");
        outputFile.WriteLine("public class " + className + " : ScriptableObject, ICsvConfigurable");
        outputFile.WriteLine("{");
    }

    private static void WriteField(string fieldTypeName, string fieldName, TextWriter outputFile)
    {
        outputFile.WriteLine("\t[RemoteProperty]");
        var result = $"\tpublic {fieldTypeName} {fieldName};";

        outputFile.WriteLine(result);
        outputFile.WriteLine(string.Empty);
    }

    private static void WriteFooter(TextWriter outputFile)
    {
        outputFile.Write("\tpublic void Configure(Values values)"
        + "\n\t{"
        + "\n\t}");
        
        outputFile.WriteLine("\n}");
    }
}