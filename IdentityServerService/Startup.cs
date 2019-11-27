using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using IdentityServerService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<IUserService, UsersService>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(new IdentityResource[]
                    {
                        new IdentityResources.OpenId()
                    })
                .AddInMemoryApiResources(new List<ApiResource>
                    {
                        // defined in the startup.xs of WebApi
                        new ApiResource("WebApi")
                        {
                            ApiSecrets = {new Secret("web_api_key".Sha256())}
                        }
                    })
                .AddInMemoryClients(new List <Client>
                    {
                        // defined when clients call identityServer to request the token
                        new Client
                        {
                            ClientId = "ConsoleApp_ClientId",
                            ClientSecrets = { new Secret("client_key".Sha256()) },
                            AccessTokenType = AccessTokenType.Reference,
                            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                            AllowedScopes = { "WebApi" },
                        }
                    })
                .AddProfileService<ProfileService>();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseIdentityServer();
        }
    }
}
