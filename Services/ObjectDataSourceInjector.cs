using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.Native.Data;
using DevExpress.XtraReports.Services;
using DevExpress.XtraReports.UI;
using System.ComponentModel.Design;
using System;
using Microsoft.Extensions.DependencyInjection;
using DevExpress.XtraPrinting.Native;

namespace BookStore.Services
{
    public class ObjectDataSourceInjector : IObjectDataSourceInjector
    {
        private readonly IServiceProvider _serviceProvider;

        public ObjectDataSourceInjector(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Process(XtraReport report)
        {
            var dse = new UniqueDataSourceEnumerator();
            ((IServiceContainer)report).ReplaceService(typeof(IReportProvider), _serviceProvider.GetRequiredService<IReportProvider>());
            foreach (var dataSource in dse.EnumerateDataSources(report, true))
            {
                if (dataSource is ObjectDataSource ods && ods.DataSource is Type dataSourceType)
                {
                    ods.DataSource = _serviceProvider.GetRequiredService(dataSourceType); // <== DISPOSE = FALSE
                }
            }
        }
    }
}
