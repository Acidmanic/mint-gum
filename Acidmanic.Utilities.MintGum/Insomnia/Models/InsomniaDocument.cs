using System.Text.Json.Serialization;
using Acidmanic.Utilities.DataTypes;
using Newtonsoft.Json;

namespace Acidmanic.Utilities.MintGum.Insomnia.Models;

public class InsomniaDocument
{
    [JsonProperty("_type")]
    [JsonPropertyName("_type")]
    public string Type => "export";

    [JsonProperty("__export_format")]
    [JsonPropertyName("__export_format")]
    public int Format => 4;

    [JsonProperty("__export_date")]
    [JsonPropertyName("__export_date")]
    public DateTime ExportDate { get; set; }


    [JsonProperty("__export_source")]
    [JsonPropertyName("__export_source")]
    public string ExportSource => "MintGum";


    public List<object> Resources { get; } = new();

    private InsomniaWorkspace _workspace;
    private Dictionary<string, InsomniaFolder> _foldersById;
    private Dictionary<string, List<InsomniaFolder>> _foldersByName;
    private Dictionary<string, InsomniaRequest> _requestsById;


    public InsomniaDocument(string workspaceName)
    {
        _workspace = new InsomniaWorkspace()
        {
            Description = "Mint gum Apis",
            Name = workspaceName,
            ParentId = null
        };

        _workspace.Id = _workspace.CreateId();

        Resources.Add(_workspace);
        
        _foldersById = new Dictionary<string, InsomniaFolder>();
        _foldersByName = new Dictionary<string, List<InsomniaFolder>>();
        _requestsById = new Dictionary<string, InsomniaRequest>();
    }

    public InsomniaFolder AddFolder(string folderName, string? parentFolderId = null)
    {
        var parentId = parentFolderId ?? _workspace.Id;

        var folder = new InsomniaFolder()
        {
            Name = folderName,
            ParentId = parentId
        };

        folder.Id = folder.CreateId();

        _foldersById.Add(folder.Id, folder);

        if (!_foldersByName.ContainsKey(folderName))
        {
            _foldersByName.Add(folderName, new List<InsomniaFolder>());
        }
        
        _foldersByName[folderName].Add(folder);
        
        Resources.Add(folder);
        
        return folder;
    }


    public InsomniaRequest AddRequest(string name, string description, string method, string url,
        string? parentFolderId = null)
    {
        var parentId = parentFolderId ?? _workspace.Id;

        var request = new InsomniaRequest()
        {
            Name = name,
            Description = description,
            Method = method,
            Url = url,
            ParentId = parentId
        };
        request.Id = request.CreateId();

        _requestsById.Add(request.Id, request);

        Resources.Add(request);

        return request;
    }
}