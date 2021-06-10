using Entitys.Models.Auth;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Models;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using AvastInfrastructureRepository.Repositories.Interfaces;

namespace AuthService.Interfaces
{
    public interface ITokenService : IEntityRepositoryCore<AuthUserRTokens>
    {
        AuthUserRTokens FindToken(string refreshtoken);
        AuthUserRTokens InsertToken(AuthUserRTokens model);
        void UpdateToken(AuthUserRTokens model);
        ResponseCoreData DeleteToken(string refresh);
        ResponseCoreData DoPassword(LoginModel model);

        ResponseCoreData DoRefreshToken(string refreshtoken);

    }
}
