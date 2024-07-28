namespace Acidmanic.Utilities.MintGum.Configuration;

public interface IMintGumConfigurationBuilder
{

    IMintGumConfigurationBuilder ServeAngularSpa(bool serve = true);

    IMintGumConfigurationBuilder DefaultPage(string fileName);

    IMintGumConfigurationBuilder ServingDirectory(string directoryName);
    
    

}