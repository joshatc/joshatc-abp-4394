using DevExpress.XtraReports.Services;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class CustomReportProviderAsync : IReportProviderAsync
    {
        readonly ReportStorageWebExtension reportStorageWebExtension;
        readonly IObjectDataSourceInjector dataSourceInjector;

        public CustomReportProviderAsync(ReportStorageWebExtension reportStorageWebExtension, IObjectDataSourceInjector dataSourceInjector)
        {
            this.reportStorageWebExtension = reportStorageWebExtension;
            this.dataSourceInjector = dataSourceInjector;
        }
        public async Task<XtraReport> GetReportAsync(string id, ReportProviderContext context)
        {
            var reportLayoutBytes = await reportStorageWebExtension.GetDataAsync(id);
            using (var ms = new MemoryStream(reportLayoutBytes))
            {
                var report = XtraReport.FromXmlStream(ms);
                dataSourceInjector.Process(report);
                return report;
            }
        }
    }
}
