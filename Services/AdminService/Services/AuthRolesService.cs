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
    public class AuthRolesService : EntityRepositoryCore<AuthRoles>, IAuthRolesService
    {
        public AuthRolesService(IDbContext context) : base(context)
        {
        }
        public AuthRoles toEntity(RoleViewModel model)
        {
            var entity = new AuthRoles();
            if (model.Id > 0) entity = this.Get(model.Id);
            entity.Name = model.Name;
            entity.Comment = model.Comment;
            entity.StructureId = model.StructureId;
            return entity;
        }

        public RoleViewModel toModel(AuthRoles entity)
        {
            var model = new RoleViewModel();
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Comment = entity.Comment;
            model.StructureId = entity.StructureId;
            return model;
        }
    }
}
