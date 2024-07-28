namespace Acidmanic.Utilities.MintGum.Configuration;

internal class MintGumConfigurationBuilder:IMintGumConfigurationBuilder,IMintGumConfiguration
{
    
    
    public static readonly string DefaultServingDirectoryName = "front-end";
    
    public static readonly string DefaultDefaultPageFileName = "Index.html";

    public string ServingDirectoryName { get; private set; } = DefaultServingDirectoryName;

    public string DefaultPageFileName { get; private set; } = DefaultDefaultPageFileName;

    public bool ServesAngularSpa { get; private set; } = false;
    
    
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
}