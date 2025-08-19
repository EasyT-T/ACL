#include "uerm.as"

void OnInitialize()
{
    print("Loading ACL C++ Libraries...");

    RegisterAllCallbacks();

    TL_Load_Plugins();
}