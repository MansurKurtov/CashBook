using AvastInfrastructureRepository.Repositories.Interfaces;
using Entitys.Models.Auth;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminService.Interfaces
{
    public interface IAuthUserPermissionsService : IEntityRepositoryCore<AuthUserPermissions>
    {
    }
}
