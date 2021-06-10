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
    public class AuthPermissionsService : EntityRepositoryCore<AuthPermissions>, IAuthPermissionsService
    {
        public AuthPermissionsService(IDbContext context) : base(context)
        {
        }

    }
}
