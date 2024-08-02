using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Acidmanic.Utilities.MintGum.Insomnia.Models;

public class InsomniaRequest:InsomniaResource
{
   
public string Url { get; set; } = string.Empty;

public string Name { get; set; } = string.Empty;

public string Method { get; set; } = "GET";
public object Body { get; set; } = new ();
public List<string> Parameters { get; set; } = new();
public List<InsomniaPair> Headers { get; set; } = new ();
public object Authentication { get; set; } = new();
public bool IsPrivate => false;
public bool SettingStoreCookies => true;
public bool SettingSendCookies => true;
public bool SettingDisableRenderRequestBody => false;
public bool SettingEncodeUrl => true;
public bool SettingRebuildPath => true;
public string SettingFollowRedirects => "global";

protected override string GetResourceType => "request";
protected override string GetResourceTypeShort => "req";
}