using AdminService.Interfaces;
using AuthService.Models;
using AvastInfrastructureRepository.Repositories.Services;
using EntityRepository.Context;
using Entitys.BLL;
using Entitys.Enums;
using Entitys.Models.Main;
using Entitys.PostModels;
using MainsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MainsService.Services
{
    public class EntOrgService : EntityRepositoryCore<EntOrg>, IEntOrgService
    {
        private readonly IAuthUsersService _authUserService;
        public EntOrgService(IDbContext context, IAuthUsersService authUsersService) : base(context)
        {
            _authUserService = authUsersService;
        }


        public bool AddOrganization(RegisterModel model)
        {
            var item_company = this.Find(x => x.Inn == model.Tin).ToList().FirstOrDefault();

            var item_director = _authUserService.Find(x => x.UserName == model.UserName).ToList().FirstOrDefault();

            if (item_director != null || item_company != null) return false;

            var organization = new EntOrg
            {
                CreatedDate = DateTime.Now,
                Status = OrganizationStatus.ACTIVE,
                District = model.DistrictId,
                Region = model.RegionId,
                ShortName = model.CompanyName,
                Name = model.CompanyName,
                Phone = model.PhoneNumber,
                ContactName = model.FirstName + " " + model.LastName,
                StructureId = (int)EnumStructure.Shop
            };

            Add(organization);

            var user = new UserPostModel();
            user.Login = model.UserName;
            user.Password = model.Password;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IsAdmin = true;
            //user. = organization.Id;
            //user.StructureId = (int)EnumStructure.Shop;
            //user.Telefon = model.PhoneNumber;
            //user.Active = true;
            //_authUserService.UserInsert(user);

            return true;
        }

    }
}
