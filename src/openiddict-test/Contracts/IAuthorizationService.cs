using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Authentication;

namespace openiddict_test.Contracts
{
    public interface IAuthorizationService
    {
        bool Authorize(OpenIdConnectRequest connectRequest, out OpenIdConnectResponse openIdConnectResponse,
            out AuthenticationTicket authenticationTicket);
    }
}