using BookStore.ObjectDataSources;
using DevExpress.DataAccess.Web;
using System;
using System.Collections.Generic;

namespace BookStore.Services {
    public class ObjectDataSourceWizardCustomTypeProvider : IObjectDataSourceWizardTypeProvider {
        public IEnumerable<Type> GetAvailableTypes(string context) {
            return new[] { typeof(ReportingDataSourceServiceDecorator) };
        }
    }
}