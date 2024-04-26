﻿using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;

namespace ExamenApiAzureCubosMauricio.Helpers
{
    public class HelperActionServicesOAuth
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }

        public HelperActionServicesOAuth(SecretClient secretClient)
        {
            KeyVaultSecret secretIssuer = secretClient.GetSecret("Issuer");
            KeyVaultSecret secretAudience = secretClient.GetSecret("Audience");
            KeyVaultSecret secretSecretKey = secretClient.GetSecret("SecretKey");
            this.Issuer = secretIssuer.Value;
            this.Audience = secretAudience.Value;
            this.SecretKey = secretSecretKey.Value;
        }

        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data = Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }

        public Action<JwtBearerOptions> GetJwtBearerOptions()
        {
            Action<JwtBearerOptions> options = new Action<JwtBearerOptions>(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = this.Issuer,
                    ValidAudience = this.Audience,
                    IssuerSigningKey = this.GetKeyToken()
                };
            });
            return options;
        }
        public Action<AuthenticationOptions> GetAuthenticateSchema()
        {
            Action<AuthenticationOptions> options = new Action<AuthenticationOptions>(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            return options;
        }
    }
}
