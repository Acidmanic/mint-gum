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
    public static object Translate(this IRequestDescriptor descriptor, IServiceProvider sp)
    {
        var scheme = descriptor.Scheme;

        if (scheme.MimeType == RequestBodyMimeType.None) return new object();

        if (scheme.MimeType == RequestBodyMimeType.Json)
        {
            var instance = Instantiate(scheme.BodyModelType!, descriptor, sp);

            var json = JsonConvert.SerializeObject(instance, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
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
            var instance = Instantiate(scheme.BodyModelType!, descriptor, sp);
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


    private static object Instantiate(Type type, IRequestDescriptor descriptor, IServiceProvider sp)
    {
        var evaluator = new ObjectEvaluator(type);

        foreach (var key in evaluator.Map.Keys)
        {
            var node = evaluator.Map.NodeByKey(key);

            var value = DefaultValue(node, descriptor, sp);

            evaluator.Write(key, value);
        }

        return evaluator.RootObject;
    }


    private static object? DefaultValue(AccessNode node, IRequestDescriptor descriptor, IServiceProvider sp)
    {
        var att = node.PropertyAttributes.FirstOrDefault(attribute => attribute is SuggestedValueAttribute)
            as SuggestedValueAttribute;

        if (att is { } a)
        {
            if (a.Strategy == SuggestedValueStrategy.FixedValue)
            {
                return a.SuggestValue(descriptor);
            }

            if (a.Strategy == SuggestedValueStrategy.DiRegisteredFactory)
            {
                return a.SuggestValue(descriptor, sp);
            }
        }

        return DefaultValue(node.Type);
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