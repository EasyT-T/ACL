
# ACL - Anomalous Csharp Loader

This is a C# plugin loader / wrapper for SCP Containment Breach Multiplayer Reborn


## How to use it

1. Add configs to your `server.cfg` file.
`plugin {ProjectFolder}/AngelScript/preinit.as`
`plugin {ProjectFolder}/AngelScript/csloader.as`

2. Compile the `CppBridge` and get `ACL.dll`. Put this library to your server folder.

3. Compile the `CsharpLoader/ACL` and rename the library to `ACL.NET.dll`. Put this library to your server folder.

4. Write your own C# plugin or compile the `CsharpLoader/ACL.Sample` as example plugin. Put the plugin to `{ServerFolder}/Plugins`

5. Open the server executable. If everything runs well, the plugin should be loaded.
