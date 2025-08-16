
# ERF - EasyRebornFramework

This is a C# plugin loader / wrapper for SCP Containment Breach Multiplayer Reborn


## How to use it

1. Add a config to your `server.cfg` file.
`script {ProjectFolder}/AngelScript/csloader.as`

2. Compile the `CppBridge` and get `crf.dll`. Put this library to your server folder.

3. Compile the `CsharpLoader/ERF` and rename the library to `crf.net.dll`. Put this library to your server folder.

4. Write your own C# plugin or compile the `CsharpLoader/ERF.Example` as example plugin. Put the plugin to `{ServerFolder}/Plugins`

5. Open the server executable. If everything runs well, the plugin should be loaded.
