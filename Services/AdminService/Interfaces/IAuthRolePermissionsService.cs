using AvastInfrastructureRepository.Repositories.Interfaces;
using Entitys.Models.Auth;
using Entitys.ViewModels.Auth;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminService.Interfaces
{
    public interface IAuthRolePermissionsService : IEntityRepositoryCore<AuthRolePermissions>
    {
        AuthRolePermissions toEntity(RolePermissionViewModel model);
        RolePermissionViewModel toModel(AuthRolePermissions entity);
    }
}
