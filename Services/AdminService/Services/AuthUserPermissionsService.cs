using AdminService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using EntityRepository.Context;
using EntityRepository.Repository;
using Entitys.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminService.Services
{
    public class AuthUserPermissionsService : EntityRepositoryCore<AuthUserPermissions>, IAuthUserPermissionsService
    {
        public AuthUserPermissionsService(IDbContext context) : base(context)
        {
        }

    }
}
