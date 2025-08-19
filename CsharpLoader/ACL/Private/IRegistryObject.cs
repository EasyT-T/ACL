namespace ACL.Private;

using ACL.Feature;

internal interface IRegistryObject
{
    void Register(EventManager eventManager);
}