using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using openiddict_test.Contracts;

namespace openiddict_test.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("~/connect/token")]
        [Produces("application/json")]
        public IActionResult Exchange()
        {
            var request = HttpContext.GetOpenIdConnectRequest();
            OpenIdConnectResponse openIdConnectResponse;
            AuthenticationTicket authenticationTicket;

            if (_authorizationService.Authorize(request, out openIdConnectResponse, out authenticationTicket))
            {
                return SignIn(authenticationTicket.Principal, authenticationTicket.Properties, authenticationTicket.AuthenticationScheme);
            }
            else
            {
                return BadRequest(openIdConnectResponse);
            }
        }
    }
}