using BookStore.ObjectDataSources.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BookStore.ObjectDataSources
{
    public class ScopedServiceProvider<TService> : IScopedServiceProvider<TService>
    {
        private readonly IServiceProvider _provider;

        public ScopedServiceProvider(IServiceProvider provider)
        {
            this._provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public ServiceScopeScope<TService> GetService()
        {
            var scope = _provider.CreateScope();
            return new ServiceScopeScope<TService>(scope);
        }
    }
}
