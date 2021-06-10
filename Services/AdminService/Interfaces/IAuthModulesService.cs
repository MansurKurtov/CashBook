using Entitys.Models.Auth;
using System;
using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;

namespace AdminService.Interfaces
{
    public interface IAuthModulesService : IEntityRepositoryCore<AuthModules>
    {
        ResponseCoreData IsDayClosed(DateTime date, int bankCode);
        ResponseCoreData CloseDay(int bankCode, int userId, DateTime date, DateTime nextDate);
        ResponseCoreData OpenDay(int bankCode, int userId, DateTime date);
        ResponseCoreData SetClearCloseDayAll(int userId, DateTime date, int bankCode);
    }
}
