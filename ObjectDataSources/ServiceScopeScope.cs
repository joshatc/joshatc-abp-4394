using Microsoft.Extensions.DependencyInjection;
using System;

namespace BookStore.ObjectDataSources
{
    public class ServiceScopeScope<TService> : IDisposable
    {
        private readonly IServiceScope _scope;

        public ServiceScopeScope(IServiceScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));

            Service = scope.ServiceProvider.GetRequiredService<TService>();
        }

        public TService Service { get; }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
