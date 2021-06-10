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
    public class RefDistrictService : EntityRepositoryCore<RefDistrict>, IRefDistrictService
    {
        public RefDistrictService(IDbContext context) : base(context)
        {
        }
    }
}
