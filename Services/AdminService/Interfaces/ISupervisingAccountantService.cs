using AvastInfrastructureRepository.Repositories.Interfaces;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.CashOperation;
using Entitys.PostModels.CashOperations;

namespace AdminService.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISupervisingAccountantService : IEntityRepositoryCore<SupervisingAccountant>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        ResponseCoreData GetSupAccountant(int bankCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResponseCoreData Add(SupervisingAccountantPostModel model, int bankCode, int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResponseCoreData Update(SupervisingAccountantPostModel model, int bankCode, int userId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ResponseCoreData DeleteById(int Id);
    }
}
