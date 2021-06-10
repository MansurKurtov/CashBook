using AdminService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using EntityRepository.Context;
using EntityRepository.Repository;
using Entitys.Models.Auth;
using Entitys.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminService.Services
{
    public class AuthUserRoleService : EntityRepositoryCore<AuthUserRole>, IAuthUserRoleService
    {
        public AuthUserRoleService(IDbContext context) : base(context)
        {
        }
        public AuthUserRole toEntity(UserRoleViewModel model)
        {
            var entity = new AuthUserRole();
            if (model.Id > 0) entity = this.Get(model.Id);
            entity.RoleId = model.RoleId;
            entity.UserId = model.UserId;
            return entity;
        }

        public UserRoleViewModel toModel(AuthUserRole entity)
        {
            var model = new UserRoleViewModel();
            model.Id = entity.Id;
            model.RoleId = entity.RoleId;
            model.UserId = entity.UserId;
            //  model.RoleName = entity.AuthRolesModel?.Name;
            //  model.UserName = entity.UsersModel?.UserName;
            return model;
        }
    }
}
