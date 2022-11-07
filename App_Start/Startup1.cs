using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(JobsManagementAPI.App_Start.Startup1))]

namespace JobsManagementAPI.App_Start
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseOpenIdConnectAuthentication(new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationOptions {
                 AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                 Authority = "https://accounts.google.com/o/oauth2/auth",
                 ClientId = "298597524720-lduls4lfar899s63boik187o5fbabk2t.apps.googleusercontent.com",
                 ClientSecret = "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",
                 UsePkce = true,
                 Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration() {
                    TokenEndpoint = "/Token",
                    AuthorizationEndpoint = "/Authorize",
                    RegistrationEndpoint = "/Registration"                    
                 }
            });
        }
    }
}
