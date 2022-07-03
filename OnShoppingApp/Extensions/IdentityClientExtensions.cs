using IdentityModel.OidcClient.Results;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnShoppingApp.Models;

namespace OnShoppingApp.Extensions
{
    public static class IdentityClientExtensions
    {
        public static ClientCredentials ToCredentials(this LoginResult loginResult)
            => new ClientCredentials
            {
                AccessToken = loginResult.AccessToken,
                IdentityToken = loginResult.IdentityToken,
                RefreshToken = loginResult.RefreshToken,
                AccessTokenExpiration = loginResult.AccessTokenExpiration
            };

        public static ClientCredentials ToCredentials(this RefreshTokenResult refreshTokenResult)
            => new ClientCredentials
            {
                AccessToken = refreshTokenResult.AccessToken,
                IdentityToken = refreshTokenResult.IdentityToken,
                RefreshToken = refreshTokenResult.RefreshToken,
                AccessTokenExpiration = refreshTokenResult.AccessTokenExpiration
            };
    }
}
