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
    public class AuthRolePermissionsService : EntityRepositoryCore<AuthRolePermissions>, IAuthRolePermissionsService
    {
        public AuthRolePermissionsService(IDbContext context) : base(context)
        {
        }
        public AuthRolePermissions toEntity(RolePermissionViewModel model)
        {
            var entity = new AuthRolePermissions();
            if (model.Id > 0) entity = this.Get(model.Id);
            entity.RoleId = model.RoleId;
            entity.PermissionId = model.PermissionId;
            //entity.ActionCodes = model.ActionCodes;
            return entity;
        }

        public RolePermissionViewModel toModel(AuthRolePermissions entity)
        {
            var model = new RolePermissionViewModel();
            model.Id = entity.Id;
            model.RoleId = entity.RoleId;
            model.PermissionId = entity.PermissionId;
            model.RoleName = entity.AuthRolesModel?.Name;
            model.PermissionName = entity.PermissionModel?.Name;
            //model.ActionCodes = entity.ActionCodes;
            return model;

        }
    }
}
