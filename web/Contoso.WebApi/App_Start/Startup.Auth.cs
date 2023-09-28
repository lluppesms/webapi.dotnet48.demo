using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Configuration;
using System.Security.Claims;

namespace Contoso.WebApi
{
	public partial class Startup
	{
		public void ConfigureAuth(IAppBuilder app)
		{
			var clientId = ConfigurationManager.AppSettings["AzureAD__ClientId"];
			var aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["AzureAD__AADInstance"]);
			var tenantId = ConfigurationManager.AppSettings["AzureAD__TenantId"];
			var postLogoutRedirectUri = ConfigurationManager.AppSettings["AzureAD__PostLogoutRedirectUri"];
			var authority = aadInstance + tenantId + "/v2.0";

			if (!string.IsNullOrEmpty(clientId))
			{
				app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

				app.UseCookieAuthentication(new CookieAuthenticationOptions());

				app.UseOpenIdConnectAuthentication(
					new OpenIdConnectAuthenticationOptions
					{
						ClientId = clientId,
						Authority = authority,
						PostLogoutRedirectUri = postLogoutRedirectUri,

						Notifications = new OpenIdConnectAuthenticationNotifications()
						{
							SecurityTokenValidated = (context) =>
							{
								string name = context.AuthenticationTicket.Identity.FindFirst("preferred_username").Value;
								context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, name, string.Empty));

								if (IsAdmin(context.AuthenticationTicket.Identity))
								{
									context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, "Admin", string.Empty));
								}
								return System.Threading.Tasks.Task.FromResult(0);
							}
						}
					});
			}
		}

		private bool IsAdmin(ClaimsIdentity identity)
		{
			if (identity != null)
			{
				var isAdmin = identity.HasClaim("Role", "Admin");

				// yes, this should rely just on the roles, but I've added a check for some user names for easy testing...!
				if (!isAdmin)
				{
					isAdmin =
						(identity.Name.ToLower().Contains("lyle") && identity.Name.ToLower().Contains("luppes")) ||
						(identity.Name.ToLower().Contains("ryan") && identity.Name.ToLower().Contains("pfalz")) ||
						(identity.Name.ToLower().Contains("brian") && identity.Name.ToLower().Contains("cheng"));
				}
				return isAdmin;
			}
			return false;
		}

		private static string EnsureTrailingSlash(string value)
		{
			if (value == null)
			{
				value = string.Empty;
			}

			if (!value.EndsWith("/", StringComparison.Ordinal))
			{
				return value + "/";
			}

			return value;
		}
	}
}
