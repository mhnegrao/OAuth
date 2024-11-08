using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace OAuth.Auth
{
    public class Authorization
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Authorization(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Callback()
        {
             var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            // if (accessToken != null)
            // {
            //     // Agora você pode usar o token para acessar a API de cobrança do Banco do Brasil
            //     return accessToken;
            // }
            return accessToken ?? "";
        }

    }
}