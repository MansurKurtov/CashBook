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
    public class AuthUIElementsService : EntityRepositoryCore<AuthUIElements>, IAuthUIElementsService
    {
        public AuthUIElementsService(IDbContext context) : base(context)
        {
        }

    }
}
