int library = LoadLibrary("erf.dll");

void OnInitialize()
{
    print("Registering ERF C++ Libraries...");

    int TL_Load_Plugins = GetProcAddress(library, "_TL_Load_Plugins@0");
    RegisterLibraryFunction("void TL_Load_Plugins()", TL_Load_Plugins, 0);
}