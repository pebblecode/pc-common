namespace PebbleCode.Entities
{
    public interface IAuthorizationController
    {
        bool CanUpdate(string contextName, string propertyName);
        void IndicateUpdated(string propertyName, string contextName);
    }
}
