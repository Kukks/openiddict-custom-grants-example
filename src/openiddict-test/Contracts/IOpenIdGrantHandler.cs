using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Authentication;

namespace openiddict_test.Contracts
{
    public interface IOpenIdGrantHandler
    {
        string GrantType { get; }

        bool CanHandle(OpenIdConnectRequest connectRequest);

        bool Handle(OpenIdConnectRequest connectRequest, out OpenIdConnectResponse openIdConnectResponse, out AuthenticationTicket authenticationTicket);
    }
}