

![Icon](Graphics/icon.png) __Mint Gum!__ : Static Web Server Extension
==========================


About
-------


How To Install
---------


How To Use
----------


__Simple Usage__


__Maintenance Apis__


__Configuration Details__


__Configure CsProj For publish and build__

Example:

```xml

    <PropertyGroup>
        <TargetFramework>net8.0</TargetFramework>
        <Nullable>enable</Nullable>
        <ImplicitUsings>enable</ImplicitUsings>
        <SpaRoot>path/to/front-end/source-code</SpaRoot>
        <SpaDeployRoot>front-end</SpaDeployRoot>
        <SpaDistributionDir>path/to/front-end/built-artifacts</SpaDistributionDir>
    </PropertyGroup>


    <Target Name="BuildFront" BeforeTargets="DeployFrontToBuildDir;DeployFrontToPublishDir">
        <Exec WorkingDirectory="$(SpaRoot)" Command="npm install"/>
        <Exec WorkingDirectory="$(SpaRoot)" Command="npm run build -- --configuration production"/>
    </Target>


    <Target Name="DeployFrontToBuildDir" AfterTargets="Build">

        <Exec Command="mkdir -p $(OutDir)$(SpaDeployRoot)/"/>
        <Exec Command="cp -r $(SpaRoot)/$(SpaDistributionDir)/**  $(OutDir)$(SpaDeployRoot)/"/>
    </Target>


    <Target Name="DeployFrontToPublishDir" AfterTargets="ComputeFilesToPublish">
        <Exec Command="mkdir -p $(PublishDir)$(SpaDeployRoot)/"/>
        <Exec Command="echo SpaRoot: $(SpaRoot) OutDir: $(PublishDir)$(SpaDeployRoot)"/>
        <Exec Command="echo $(SpaDeployRoot)"/>
        <Exec Command="cp -r $(SpaRoot)/$(SpaDistributionDir)/**  $(PublishDir)$(SpaDeployRoot)/"/>
    </Target>
```

Contact
-------
