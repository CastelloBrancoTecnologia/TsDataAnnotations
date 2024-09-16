using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using TsModelGeneratorLib.Attributes;

using CommunityToolkit.Mvvm.ComponentModel;

namespace TsModelGenerator;

public class TsModelGenerator
{
    private static string GetTsType(Type t)
    {
        Console.WriteLine(t.Name);

        return t.Name switch
        {
            "String[]" => "string [] = []",
            "long" or "System.Int64" or "Int64" or "int64" or "System.UInt64" or "UInt64" or "uint64" => "BigInt = BigInt(0)",
            "System.DateTime" or "DateTime" or "System.DateTimeOffset" or "DateTimeOffset" => "Date = new Date()",
            "System.TimeSpan" or "TimeSpan" => "TimeSpan = new TimeSpan()",
            "int" or "System.Byte" or "Byte" or "byte" 
                  or "System.Int16" or "Int16" or "int16" 
                  or "System.Int32" or "Int32" or "int32" 
                  or "System.UInt16" or "UInt16" or "uint16" 
                  or "System.UInt32" or "UInt32" or "uint32" 
                  or "System.Single" or "Single" or "float" 
                  or "System.Double" or "Double" or "double" => "number = 0",
            "System.Char" or "char" or "Char" or "System.String" or "string" or "String" => "string = \"\"",
            "System.Boolean" or "bool" or "Boolean" => "boolean = false",
            _ => "any?",
        };
    }

    private static string FormatArgument(object? value)
    {
        return value switch
        {
            null => "null",
            string s => $"\"{s}\"",
            bool b => b.ToString().ToLower(),
            _ => value.ToString()
        } ?? "null";
    }

    private static bool IsDefaultValue(PropertyInfo property, object attributeInstance)
    {
        var defaultValue = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
        var currentValue = property.GetValue(attributeInstance);

        return Equals(defaultValue, currentValue);
    }
    private static bool IsIgnoredProperty(string propertyName)
    {
        return ((string[])["TypeId", "DataType"]).Contains(propertyName);
    }

    private static string GenerateSource(Type t)
    {
        var sb = new StringBuilder();

        PropertyInfo[]? properties = t.GetProperties().Where(x => x.Name != "HasErrors" && x.GetCustomAttribute<TypeScriptIgnoreAttribute>(true) == null).ToArray();

        sb.Append("import { ");

        List<string> validators_names = new();

        foreach (PropertyInfo field in properties)
        {
            object[] validators = field.GetCustomAttributes(true).Where(x => x is ValidationAttribute).ToArray();
            
            foreach (object validator in validators)
            {
                Type tv = validator.GetType();

                string validatorName = tv.Name.EndsWith("Attribute") ? tv.Name[..^9] : tv.Name;

                if (! validators_names.Contains (validatorName))
                {
                    validators_names.Add (validatorName);
                }
            }
        }

        sb.Append(string.Join(", ", validators_names));

        sb.AppendLine(" } from \"../Validators/validators\"");
        sb.AppendLine("import { BaseViewModel } from  \"../Validators/validators\"");

        sb.AppendLine($"export class {t.Name} extends BaseViewModel {{");

        foreach (PropertyInfo field in properties)
        {
            Console.WriteLine ($" Encontrada propriedade {field.Name}");

            object[] validators = field.GetCustomAttributes(true).Where(x => x is ValidationAttribute).ToArray();

            string tsType = GetTsType(field.PropertyType) ?? "any";

            string name = field.Name; //.TrimStart('_');

            name = char.ToUpper(name[0]) + name[1..];

            foreach (object validator in validators)
            {
                Type tv = validator.GetType();
                string validatorName = tv.Name.EndsWith("Attribute") ? tv.Name[..^9] : tv.Name;

                // Extract non-default arguments passed to the ValidationAttribute
                var arguments = tv.GetProperties()
                    .Where(p => p.CanRead && ! IsDefaultValue(p, validator) && !IsIgnoredProperty(p.Name) )
                    .Select(p => $"{FormatArgument(p.GetValue(validator))}")
                    .ToArray(); // {p.Name}: 

                string argumentsString = arguments.Length > 0 ? $"({string.Join(", ", arguments)})" : "()";

                sb.AppendLine($"    @{validatorName}{argumentsString}");
            }

            sb.AppendLine($"    public {name}: {tsType};");
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    public static bool GenerateModels(string? AssemblyPathName, string? TypeScriptOutputpath)
    {
        try
        {
            if (!File.Exists(AssemblyPathName))
            {
                Console.WriteLine($"Assembly {AssemblyPathName} nao encontrado ");

                return false;
            }

            if (!Directory.Exists(TypeScriptOutputpath))
            {
                Console.WriteLine($"Diretorio de saida {TypeScriptOutputpath} nao encontrado ");

                return false;
            }

            AssemblyLoadContext loadContext = new(null, true);

            Assembly? assembly = loadContext.LoadFromAssemblyPath(AssemblyPathName);

            foreach (Type type in assembly.GetTypes().Where(x => x.GetCustomAttribute<GenerateTypeScriptAttribute>() != null))
            {
                Console.WriteLine($"Gerando Modelo {type.Name} em {TypeScriptOutputpath}");

                File.WriteAllText(Path.Combine(TypeScriptOutputpath, $"{type.Name}.ts"), GenerateSource(type));
            }

            assembly = null;

            loadContext.Unload();

            // Trigger garbage collection to release file handles
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
        }
        catch
        {
        }

        return false;
    }

}
