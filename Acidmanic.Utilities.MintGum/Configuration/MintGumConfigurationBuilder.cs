namespace Acidmanic.Utilities.MintGum.Configuration;

internal class MintGumConfigurationBuilder:IMintGumConfigurationBuilder,IMintGumConfiguration
{
    
    
    public static readonly string DefaultServingDirectoryName = "front-end";
    
    public static readonly string DefaultDefaultPageFileName = "Index.html";
    
    public static readonly string DefaultMaintenanceApisBaseUri = "api/mint-gum";

    public string ServingDirectoryName { get; private set; } = DefaultServingDirectoryName;

    public string DefaultPageFileName { get; private set; } = DefaultDefaultPageFileName;

    public bool ServesAngularSpa { get; private set; } = false;
    
    public bool AddMaintenanceApis { get; private set; } = false;
    
    public bool MaintenanceApisRequireAuthorization { get; private set; } = true;
    
    public string MaintenanceApisBaseUri { get; private set; } = DefaultMaintenanceApisBaseUri;


    public IMintGumConfigurationBuilder ServeAngularSpa(bool serve = true)
    {
        ServesAngularSpa = serve;

        return this;
    }

    public IMintGumConfigurationBuilder DefaultPage(string fileName)
    {
        DefaultPageFileName = fileName;

        return this;
    }

    public IMintGumConfigurationBuilder ServingDirectory(string directoryName)
    {
        ServingDirectoryName = directoryName;

        return this;
    }

    public IMintGumConfigurationBuilder WithMaintenanceApis()
    {
        AddMaintenanceApis = true;

        return this;
    }

    public IMintGumConfigurationBuilder AuthorizeMaintenanceApis(bool authorize = true)
    {
        MaintenanceApisRequireAuthorization = authorize;

        return this;
    }

    public IMintGumConfigurationBuilder MaintenanceApiBasedOn(string maintenanceApiBaseUri)
    {
        MaintenanceApisBaseUri = maintenanceApiBaseUri;

        return this;
    }
}