using AuthService.Models;
using AvastInfrastructureRepository.Repositories.Interfaces;
using Entitys.BLL;
using Entitys.Models.Main;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainsService.Interfaces
{
    public interface IEntOrgService : IEntityRepositoryCore<EntOrg>
    {
        bool AddOrganization(RegisterModel model);
    }
}
