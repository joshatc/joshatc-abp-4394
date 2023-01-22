using AutoMapper.Internal.Mappers;
using System.Collections.Generic;
using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Uow;
using DevExpress.DataAccess.ObjectBinding;
using BookStore.Test;
using Volo.Abp.Domain.Repositories;
using BookStore.ObjectDataSources.Interfaces;

namespace BookStore.ObjectDataSources
{
    public class ReportingDataSourceService : ApplicationService, IReportingDataSourceService
    {
        private readonly ITestRepository _repo;

        public ReportingDataSourceService()
        {
            // We use this parameterless constructor in the Data Source Wizard only, and not for the actual instantiation of the repository object.
            throw new NotSupportedException();
        }
        public ReportingDataSourceService(ITestRepository repo)
        {
            _repo = repo;
        }

        [HighlightedMember]
        [UnitOfWork(isTransactional: false)]
        public virtual BookStore.Test.Test Get()
        {
            return _repo.FirstAsync().Result;
        }
    }
}
