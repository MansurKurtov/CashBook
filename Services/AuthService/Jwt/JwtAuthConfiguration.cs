using AuthService.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Jwt
{
    public static class JwtAuthConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureJwtAuthService(IServiceCollection services)
        {
            var keyByteArray = Encoding.ASCII.GetBytes(AuthParams.PARAM_SECRET_KEY);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT issuer (Iss) claim
                ValidateIssuer = true,
                ValidIssuer = AuthParams.PARAM_ISS,

                // Validate the JWT audience (Aud) claim
                ValidateAudience = true,
                ValidAudience = AuthParams.PARAM_AUD,

                // Validate token expiration
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}
