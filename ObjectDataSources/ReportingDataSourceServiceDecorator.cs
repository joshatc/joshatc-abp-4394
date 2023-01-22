using System.Collections.Generic;
using BookStore.ObjectDataSources.Interfaces;
using Volo.Abp.DependencyInjection;

namespace BookStore.ObjectDataSources
{
    [ExposeServices(typeof(ReportingDataSourceServiceDecorator))]
    public class ReportingDataSourceServiceDecorator : IReportingDataSourceService, ITransientDependency
    {
        private readonly IScopedServiceProvider<ReportingDataSourceService> _scopedServiceProvider;

        public ReportingDataSourceServiceDecorator(IScopedServiceProvider<ReportingDataSourceService> scopedServiceProvider)
        {
            _scopedServiceProvider = scopedServiceProvider;
        }

        public BookStore.Test.Test Get()
        {
            using (var scopeServer = _scopedServiceProvider.GetService())
            {
                var res = scopeServer.Service.Get();
                scopeServer.Dispose();
                return res;
            }
        }
    }
}