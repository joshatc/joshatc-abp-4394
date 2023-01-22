using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;
using BookStore.Data;
using DXWebApplication1.PredefinedReports;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.Settings;
using DevExpress.DataAccess.Json;
using DevExpress.XtraReports;
using System.Text.RegularExpressions;
using System;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class CustomReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        readonly string ReportDirectory;
        private readonly ISettingProvider _configuration;

        const string FileExtension = ".repx";
        public CustomReportStorageWebExtension(IWebHostEnvironment env, ISettingProvider configuration)
        {
            _configuration = configuration;
            ReportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            if (!Directory.Exists(ReportDirectory))
            {
                Directory.CreateDirectory(ReportDirectory);
            }
        }
        private bool IsWithinReportsFolder(string url, string folder)
        {
            var rootDirectory = new DirectoryInfo(folder);
            var fileInfo = new FileInfo(Path.Combine(folder, url));
            return fileInfo.Directory.FullName.ToLower().StartsWith(rootDirectory.FullName.ToLower());
        }

        public override bool CanSetData(string url) {
            // Determines whether a report with the specified URL can be saved.
            // Add custom logic that returns **false** for reports that should be read-only.
            // Return **true** if no valdation is required.
            // This method is called only for valid URLs (if the **IsValidUrl** method returns **true**).

            return true;
        }

        public override bool IsValidUrl(string url) {
            // Determines whether the URL passed to the current report storage is valid.
            // Implement your own logic to prohibit URLs that contain spaces or other specific characters.
            // Return **true** if no validation is required.

            return true;
        }

        public override async Task<byte[]> GetDataAsync(string url) {

            // Returns report layout data stored in a Report Storage using the specified URL. 
            // This method is called only for valid URLs after the IsValidUrl method is called.
            try
            {
                if (Directory.EnumerateFiles(ReportDirectory).Select(Path.GetFileNameWithoutExtension).Contains(url))
                {
                    var report = XtraReport.FromXmlFile(Path.Combine(ReportDirectory, url + FileExtension));
                    var ds = DataSourceManager.GetDataSources(report, true);
                    foreach (var item in ds)
                    {
                        if (item is JsonDataSource)
                        {
                            var Datasource = (item as JsonDataSource);
                            var OldUri = (Datasource.JsonSource as UriJsonSource).Uri.OriginalString;
                            string pattern = @"(https://)(.*?)(?=/api)";
                            string replacement = await _configuration.GetOrNullAsync("SelfUrl");
                            var NewUri = Regex.Replace(OldUri, pattern, replacement);


                            var NewSource = new UriJsonSource(new Uri(NewUri));
                            //      NewSource.HeaderParameters.Add(new HeaderParameter("Authorization", ""));
                            //      NewSource.QueryParameters.Add(new QueryParameter())
                            Datasource.JsonSource = NewSource;
                        }
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        report.SaveLayoutToXml(ms);
                        return ms.ToArray();
                    }
                }
                if (ReportsFactory.Reports.ContainsKey(url))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var report = ReportsFactory.Reports[url]();

                        var ds = DataSourceManager.GetDataSources(report, true);



                        report.SaveLayoutToXml(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException("Could not get report data.", ex);
            }
            throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not find report '{0}'.", url));
        }

        public override Task<Dictionary<string, string>> GetUrlsAsync() {
            return Task.FromResult(Directory.GetFiles(ReportDirectory, "*" + FileExtension)
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .Union(ReportsFactory.Reports.Select(x => x.Key))
                                     .ToDictionary<string, string>(x => x));
        }

        public override Task SetDataAsync(XtraReport report, string url)
        {
            if (!IsWithinReportsFolder(url, ReportDirectory))
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException("Invalid report name.");
            report.SaveLayoutToXml(Path.Combine(ReportDirectory, url + FileExtension));
            return Task.CompletedTask;
        }

        public override async Task<string> SetNewDataAsync(XtraReport report, string defaultUrl) {
            // Allows you to validate and correct the specified name (URL).
            // This method also allows you to return the resulting name (URL),
            // and to save your report to a storage. The method is called only for new reports.
            await SetDataAsync(report, defaultUrl);
            return defaultUrl;
        }
    }
}