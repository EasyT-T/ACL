#include "uerm.as"

void OnInitialize()
{
    print("Loading ERF C++ Libraries...");

    RegisterAllCallbacks();

    TL_Load_Plugins();
}