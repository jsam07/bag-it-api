using Microsoft.Owin.Security.OAuth;

namespace bagit_api.Controllers;

public class QueryStringOAuthBearerProvider : OAuthBearerAuthenticationProvider
{
    public override Task RequestToken(OAuthRequestTokenContext context)
    {
        var value = context.Request.Query.Get("access_token");

        if (!string.IsNullOrEmpty(value))
        {
            context.Token = value;
        }

        return Task.FromResult<object>(null);
    }
}