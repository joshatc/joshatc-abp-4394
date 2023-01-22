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
    public interface IObjectDataSourceInjector
    {
        public void Process(XtraReport report);
    }
}
