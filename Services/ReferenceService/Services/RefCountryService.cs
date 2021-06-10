using AvastInfrastructureRepository.Repositories.Services;
using EntityRepository.Context;
using Entitys.BLL;
using Entitys.Models.Reference;
using ReferenceService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReferenceService.Services
{
    public class RefCountryService : EntityRepositoryCore<RefCountry>, IRefCountryService
    {
        public RefCountryService(IDbContext context) : base(context)
        {
        }
    }
}
