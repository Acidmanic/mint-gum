namespace Acidmanic.Utilities.MintGum.Configuration;

public interface IMintGumConfiguration
{
    public string ServingDirectoryName { get; }

    public string DefaultPageFileName { get; }

    public bool ServesAngularSpa { get; }
}