namespace ERF.Private;

internal enum CallConv
{
    AsCallCdecl = 0,
    AsCallStdcall = 1,
    AsCallThiscallAsglobal = 2,
    AsCallThiscall = 3,
    AsCallCdeclObjlast = 4,
    AsCallCdeclObjfirst = 5,
    AsCallGeneric = 6,
    AsCallThiscallObjlast = 7,
    AsCallThiscallObjfirst = 8,
}