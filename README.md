#openiddict-custom-grants-example
A proof of concept based on capesean/openiddict-test (http://capesean.co.za/blog/asp-net-5-jwt-tokens/ )

##Objectives:
* Enable Authentication to an asp.net core backend
* Support JWT Tokens 
* Support Social Logins without loading a page from the auth server

##How will this be achieved?
* ASP.NET Core
* ASP.NET Identity V3
* Entity Framework V7
* OpenIddict
* JWT Token support was already implemented from the main repo I forked from
* Custom Grant types supported by this PR: https://github.com/openiddict/openiddict-core/pull/205

##Status
>Implementation is complete!
* Facebook Custom grant
* Standard Password Grant
* Revealing Pattern for grant handling


##Todo
* Add Google Custom Grant
* Add an Aurelia Demo
* Clean up

