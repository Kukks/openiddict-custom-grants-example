using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Authentication;
using openiddict_test.Contracts;

namespace openiddict_test.Services
{
    public class AuthorizationService: IAuthorizationService
    {
        private readonly IEnumerable<IOpenIdGrantHandler> _grantHandlers;

        public AuthorizationService(IEnumerable<IOpenIdGrantHandler> grantHandlers)
        {
            _grantHandlers = grantHandlers;
        }

        public bool Authorize(OpenIdConnectRequest connectRequest, out OpenIdConnectResponse openIdConnectResponse,
            out AuthenticationTicket authenticationTicket)
        {
            authenticationTicket = null;

            foreach (var grantHandler in _grantHandlers.Where(grantHandler => grantHandler.CanHandle(connectRequest)))
            {
                return grantHandler.Handle(connectRequest, out openIdConnectResponse, out authenticationTicket);
            }

            openIdConnectResponse = new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            };
            return false;
        }
    }
}