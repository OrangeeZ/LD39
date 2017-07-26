using System;
using System.Collections.Generic;
using System.IO;
using csv;

public class ObjectInfoCodeGenerator
{
    public static void Generate(string path, string className, string[] fieldNames, IList<Type> fieldTypes)
    {
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            using (var writer = new StreamWriter(fileStream))
            {
                WriteHeader(className, writer);

                var index = 0;
                
                foreach (var each in fieldNames)
                {
                    WriteField(fieldTypes[index++], each, writer);
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
        
        outputFile.WriteLine("public class " + className + " : ScriptableObject, ICsvConfigurable");
        outputFile.WriteLine("{");
    }

    private static void WriteField(Type fieldType, string fieldName, TextWriter outputFile)
    {
        var result = string.Format("\tpublic {0} {1};", fieldType, fieldName);

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