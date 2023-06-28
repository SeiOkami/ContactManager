using Microsoft.AspNetCore.Authentication;

namespace Contacts.WebClient.Extensions;

public static class HttpContextExtensions
{
    public static async Task<string> GetTokenAsync(this HttpContext context)
    {
        var authResult = await context.AuthenticateAsync();

        var access_token = authResult?.Ticket?.Properties.GetTokenValue("access_token");

        if (access_token == null)
            throw new Exception("Not access token");

        return access_token;
    }
}
