using AvastInfrastructureRepository.Repositories.Interfaces;
using Entitys.Models.Auth;
using Entitys.ViewModels.Auth;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminService.Interfaces
{
    public interface IAuthRolesService : IEntityRepositoryCore<AuthRoles>
    {
        AuthRoles toEntity(RoleViewModel model);
        RoleViewModel toModel(AuthRoles entity);
    }
}
