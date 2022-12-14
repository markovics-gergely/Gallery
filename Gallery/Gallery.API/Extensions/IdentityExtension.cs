using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Gallery.DAL.Domain;
using Gallery.DAL;
using Gallery.DAL.Types;

namespace Gallery.API.Extensions
{
    /// <summary>
    /// Manage identityserver configurations
    /// </summary>
    public static class IdentityExtension
    {
        /// <summary>
        /// Add identityserver to the application
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddIdentityExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<GalleryDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager()
                .AddClaimsPrincipalFactory<ClaimsFactory>();

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
                options.IssuerUri = configuration.GetValue<string>("IdentityServer:Authority");
            })
                .AddDeveloperSigningCredential()        //This is for dev only scenarios when you don’t have a certificate to use.
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(GetIdentityResources(configuration))
                .AddInMemoryApiScopes(GetApiScopes(configuration))
                .AddInMemoryClients(GetClients(configuration))
                .AddInMemoryApiResources(new List<ApiResource>
                {
                    new ApiResource(
                        configuration.GetValue<string>("Api:ApiResource:Name"),
                        configuration.GetValue<string>("Api:ApiResource:Description"),
                        new[] { JwtClaimTypes.Role, ClaimTypes.Role }
                    )
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidatorWithEmailAndUsername<ApplicationUser>>();
        }

        private static IEnumerable<IdentityResource> GetIdentityResources(IConfiguration configuration)
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = RoleTypes.RoleScope,
                    DisplayName = "Roles",
                    UserClaims = { JwtClaimTypes.Role, ClaimTypes.Role },
                    ShowInDiscoveryDocument = true,
                    Required = true,
                    Emphasize = true
                }
            };
        }

        private static IEnumerable<ApiScope> GetApiScopes(IConfiguration configuration)
        {
            return new List<ApiScope>
            {
                new ApiScope(
                    configuration.GetValue<string>("Api:ApiResource:Name"),
                    configuration.GetValue<string>("Api:ApiResource:Description"),
                    new string[] {
                        ClaimTypes.NameIdentifier,
                        JwtClaimTypes.Name,
                        ClaimTypes.Role,
                        JwtClaimTypes.Role
                    }
                )
            };
        }
        private static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = configuration.GetValue<string>("IdentityServer:ClientId"),
                    ClientSecrets = {
                        new Secret(configuration.GetValue<string>("IdentityServer:ClientSecret").Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new List<string> {
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        RoleTypes.RoleScope
                    }.Concat(configuration.GetSection("Api:ApiScopes").Get<List<string>>()).ToList(),
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AlwaysSendClientClaims = true,
                    UpdateAccessTokenClaimsOnRefresh = true
                }
            };
        }
    }
}
