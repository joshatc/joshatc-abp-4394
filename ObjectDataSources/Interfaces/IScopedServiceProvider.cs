namespace BookStore.ObjectDataSources.Interfaces
{
    public interface IScopedServiceProvider<TService>
    {
        ServiceScopeScope<TService> GetService();
    }
}
