using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.Auth;
using Entitys.PostModels;
using System;

namespace AdminService.Interfaces
{
    public interface IAuthUsersService : IEntityRepositoryCore<AuthUsers>
    {
        ResponseCoreData GetAllUsers(string permissions, int bankCode);
        ResponseCoreData GetUsersOnly(string permissions, int bankCode);
        ResponseCoreData GetUserById(int userId, string permissions, int bankCode);
        ResponseCoreData ChangePassword(int id, string password, string newpassword, int bankCode);
        ResponseCoreData UserInsert(UserPostModel user, int orgId, string permissions);
        ResponseCoreData UserUpdate(UserPostModel model, string permissions, int bankCode);
        ResponseCoreData GetBanks();
        ResponseCoreData GetBanksForSuperAdmin(string permissions);
        ResponseCoreData GetCashierTypes();
        ResponseCoreData GetPermissions();
        ResponseCoreData GetBranches(DateTime data);
    }
}
