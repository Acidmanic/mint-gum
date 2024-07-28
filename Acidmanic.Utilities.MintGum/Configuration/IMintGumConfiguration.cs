namespace Acidmanic.Utilities.MintGum.Configuration;

internal interface IMintGumConfiguration
{
    public string ServingDirectoryName { get; }

    public string DefaultPageFileName { get; }

    public bool ServesAngularSpa { get; }
    
    public bool AddMaintenanceApis { get; }
    
    public bool MaintenanceApisRequireAuthorization { get; }
    
    public string MaintenanceApisBaseUri { get; }
}