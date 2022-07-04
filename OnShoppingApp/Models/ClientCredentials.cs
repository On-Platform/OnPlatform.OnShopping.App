using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnPlatform.OnShopping.App.Models
{
    public class ClientCredentials
    {
        public string AccessToken { get; set; } = "";
        public string IdentityToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTimeOffset AccessTokenExpiration { get; set; }
        public string Error { get; set; } = "";
        public bool IsError => !string.IsNullOrEmpty(Error);
    }
}
