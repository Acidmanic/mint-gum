namespace Acidmanic.Utilities.MintGum.Configuration;

internal interface IMintGumConfiguration
{
    public string ServingDirectoryName { get; }

    public string DefaultPageFileName { get; }

    public bool ServesAngularSpa { get; }
}