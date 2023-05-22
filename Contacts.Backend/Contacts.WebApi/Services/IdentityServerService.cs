using Contacts.WebApi.Models;

namespace Contacts.WebApi.Services;

public class IdentityServerService
{
    private readonly HttpClient _httpClient;

    public IdentityServerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<IdentityUserInfo>> GetUsersAsync()
    {
        var response = await _httpClient.GetAsync("api/users/");
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException(response.ReasonPhrase);

        var users = await response.Content.ReadAsAsync<IEnumerable<IdentityUserInfo>>();
        return users;
    }

    public async Task<IdentityUserInfo> GetUserAsync(Guid Id)
    {
        var response = await _httpClient.GetAsync($"api/users/{Id}");
        if (!response.IsSuccessStatusCode)
            throw new ApplicationException(response.ReasonPhrase);

        var result = await response.Content.ReadAsAsync<IdentityUserInfo>();
        return result;
    }
}
