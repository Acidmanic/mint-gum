using System.Diagnostics;
using Acidmanic.Utilities.MintGum.Insomnia.Models;
using Acidmanic.Utilities.MintGum.RequestHandling.Contracts;
using Acidmanic.Utilities.Reflection;
using Acidmanic.Utilities.Reflection.ObjectTree;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

public static class InsomniaBodyTranslator
{
    public static object Translate(this RequestBodyScheme scheme)
    {
        if (scheme.MimeType == RequestBodyMimeType.None) return new object();

        if (scheme.MimeType == RequestBodyMimeType.Json)
        {
            var instance = Instantiate(scheme.BodyModelType!);

            var json = JsonConvert.SerializeObject(instance, new JsonSerializerSettings()
            {
                Formatting =Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return new
            {
                MimeType = "application/json",
                Text = json
            };
        }

        if (scheme.MimeType == RequestBodyMimeType.Xml)
        {
            var instance = Instantiate(scheme.BodyModelType!);
            //TODO Serialize to xml
            var xml = JsonConvert.SerializeObject(instance);

            return new
            {
                MimeType = "application/xml",
                Text = xml
            };
        }

        if (scheme.MimeType == RequestBodyMimeType.MultipartForm)
        {
            var parameters = scheme.MultiPartData.Select(Translate)
                .Where(p => p != null)
                .Select(p => p!)
                .ToList();
            return new
            {
                MimeType = "multipart/form-data",
                Params = parameters
            };
        }


        return new object();
    }


    private static object? Translate(MultiPartKeyValuePair pair)
    {
        if (pair.Type == MultipartValueType.File)
        {
            return new
            {
                Id = $"pair_{Guid.NewGuid():N}",
                Name = pair.Name,
                Type = "file",
                Description = string.Empty,
                Value = string.Empty,
                FileName = pair.Value
            };
        }

        if (pair.Type == MultipartValueType.Text)
        {
            return new
            {
                Id = $"pair_{Guid.NewGuid():N}",
                Name = pair.Name,
                Type = "text",
                Description = string.Empty,
                Value = string.Empty,
                Text = pair.Value
            };
        }

        if (pair.Type == MultipartValueType.MultiLineText)
        {
            return new
            {
                Id = $"pair_{Guid.NewGuid():N}",
                Name = pair.Name,
                Type = pair.Type,
                Description = string.Empty,
                Value = string.Empty,
                Text = "multilineText"
            };
        }

        return null;
    }


    private static object Instantiate(Type type)
    {
        var evaluator = new ObjectEvaluator(type);

        foreach (var key in evaluator.Map.Keys)
        {
            var node = evaluator.Map.NodeByKey(key);

            var value = DefaultValue(node.Type);

            evaluator.Write(key,value);
        }

        return evaluator.RootObject;
    }

    private static object? DefaultValue(Type type)
    {
        if (type == typeof(string)) return "string";

        if (type == typeof(int) ||
            type == typeof(long) ||
            type == typeof(short) ||
            type == typeof(byte) ||
            type == typeof(sbyte) ||
            type == typeof(double) ||
            type == typeof(float) ||
            type == typeof(decimal)) return 0;

        if (type == typeof(bool)) return false;

        return default;
    }
}