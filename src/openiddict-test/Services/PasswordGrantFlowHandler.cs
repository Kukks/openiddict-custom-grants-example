using System.Security.Claims;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using openiddicttest.Models;
using openiddict_test.Contracts;
using OpenIddict;

namespace openiddict_test.Services
{
    public class PasswordGrantFlowHandler : IOpenIdGrantHandler
    {
        private readonly OpenIddictUserManager<ApplicationUser> _userManager;
        public string GrantType => "password_grant_type";

        public PasswordGrantFlowHandler(OpenIddictUserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public bool CanHandle(OpenIdConnectRequest connectRequest)
        {
            return connectRequest.IsPasswordGrantType();
        }

        public bool Handle(OpenIdConnectRequest connectRequest, out OpenIdConnectResponse openIdConnectResponse, out AuthenticationTicket authenticationTicket)
        {
            authenticationTicket = null;
            openIdConnectResponse = null;
            var user = _userManager.FindByNameAsync(connectRequest.Username).Result;
            if (user == null)
            {
                openIdConnectResponse = new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The username/password couple is invalid."
                };
                return false;
            }

            // Ensure the password is valid.
            if (!_userManager.CheckPasswordAsync(user, connectRequest.Password).Result)
            {
                if (_userManager.SupportsUserLockout)
                {
                    _userManager.AccessFailedAsync(user).RunSynchronously();
                }

                openIdConnectResponse = new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The username/password couple is invalid."
                };

                return false;
            }

            if (_userManager.SupportsUserLockout)
            {
                _userManager.ResetAccessFailedCountAsync(user);
            }
            var identity = _userManager.CreateIdentityAsync(user, connectRequest.GetScopes()).Result;

            // Create a new authentication ticket holding the user identity.
            authenticationTicket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                OpenIdConnectServerDefaults.AuthenticationScheme);

            authenticationTicket.SetResources(connectRequest.GetResources());
            authenticationTicket.SetScopes(connectRequest.GetScopes());

            return true;

        }
    }
}