using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Newtonsoft.Json.Linq;
using openiddicttest;
using openiddicttest.Models;
using openiddict_test.Contracts;
using OpenIddict;

namespace openiddict_test.Services
{
    public class FacebookGrantFlowHandler : IOpenIdGrantHandler
    {
        private readonly OpenIddictUserManager<ApplicationUser> _userManager;
        private readonly FacebookService _facebookService;
        public static string Test => new FacebookGrantFlowHandler().GrantType;

        public string GrantType => "facebook_access_token";

        public FacebookGrantFlowHandler()
        {
            
        }

        public FacebookGrantFlowHandler(OpenIddictUserManager<ApplicationUser> userManager, FacebookService facebookService)
        {
            _userManager = userManager;
            _facebookService = facebookService;
        }

        public bool CanHandle(OpenIdConnectRequest connectRequest)
        {
            if (connectRequest.GrantType == GrantType &&
                !string.IsNullOrEmpty(connectRequest.Assertion))
            {
                return true;
            }

            return false;
        }

        public bool Handle(OpenIdConnectRequest connectRequest, out OpenIdConnectResponse openIdConnectResponse,
            out AuthenticationTicket authenticationTicket)
        {
            authenticationTicket = null;
            openIdConnectResponse = null;

            object fbResponse;
            if (!_facebookService.VerifyAccessToken(connectRequest.Assertion, out fbResponse))
            {
                openIdConnectResponse = new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "Access Token could not be verified with Facebook"
                };
                return false;
                }
            var email = (fbResponse as JObject).GetValue("email").Value<string>();
            
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user == null)
            {
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