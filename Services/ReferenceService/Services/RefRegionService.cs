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
    public class RefRegionService : EntityRepositoryCore<RefRegion>, IRefRegionService
    {
        public RefRegionService(IDbContext context) : base(context)
        {
        }
    }
}
