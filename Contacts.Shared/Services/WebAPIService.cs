using Contacts.Shared.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace Contacts.Shared.Services;

public class WebAPIService : IWebAPIService
{
    private Contacts.Shared.Settings.SettingsModelApi settings { get; set; }

    public WebAPIService()
    {
        settings = Contacts.Shared.Settings.SettingsManager.Settings.WebAPI;
    }

    private HttpClient NewHttpClient(string token)
    {
        var httpClient = new HttpClient();
        httpClient.SetBearerToken(token);

        return httpClient;
    }
    
    public async Task<ContactsModel?> ListContactsAsync(string token, Guid? userID)
    {
        using var httpClient = NewHttpClient(token);

        var fullPath = settings.ListContactsMethodURL;
        if (userID != null)
            fullPath = $"{fullPath}/{userID}";

        var result = await httpClient.GetAsync(fullPath);
        if (result.IsSuccessStatusCode)
            return (ContactsModel?)(await result.Content.ReadFromJsonAsync(typeof(ContactsModel)));
        else
            throw new Exception(result.ToString());
    }

    public async Task<Stream> ExportContactsAsync(string token)
    {
        using var httpClient = NewHttpClient(token);

        var result = await httpClient.GetAsync(settings.ListContactsMethodURL);
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadAsStreamAsync();
        }
        else
        {
            throw new Exception(result.ToString());
        }
    }

    public async Task CreateContactAsync(string token, ContactModel contact)
    {
        using var httpClient = NewHttpClient(token);
        
        var json = JsonConvert.SerializeObject(contact);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var result = await httpClient.PostAsync(settings.CreateContactMethodURL, data);

        if (!result.IsSuccessStatusCode)
            throw new Exception(result.ToString());
    }

    public async Task UpdateContactAsync(string token, ContactModel contact)
    {
        using var httpClient = NewHttpClient(token);

        var fullURL = settings.UpdateContactMethodURL;

        var json = JsonConvert.SerializeObject(contact);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var result = await httpClient.PutAsync(fullURL, data);

        if (!result.IsSuccessStatusCode)
            throw new Exception(result.ToString());
    }

    public async Task<ContactModel?> GetContactAsync(string token, Guid ID)
    {
        using var httpClient = NewHttpClient(token);

        var fullURL = $"{settings.DetailsContactMethodURL}/{ID}";

        var result = await httpClient.GetAsync(fullURL);
        if (result.IsSuccessStatusCode)
            return (ContactModel?)(await result.Content.ReadFromJsonAsync(typeof(ContactModel)));
        else
            throw new Exception(result.ToString());
    }

    public async Task DeleteContactAsync(string token, Guid ID)
    {
        using var httpClient = NewHttpClient(token);

        var fullURL = $"{settings.DeleteContactMethodURL}/{ID}";

        var result = await httpClient.DeleteAsync(fullURL);

        if (!result.IsSuccessStatusCode)
            throw new Exception(result.ToString());
    }

    public async Task ClearContactsAsync(string token)
    {
        using var httpClient = NewHttpClient(token);

        var result = await httpClient.DeleteAsync(settings.ClearContactsMethodURL);

        if (!result.IsSuccessStatusCode)
            throw new Exception(result.ToString());
    }

    public async Task GenerateContactsAsync(string token, bool clear = false)
    {
        if (clear)
            await ClearContactsAsync(token);
        
        using var httpClient = NewHttpClient(token);

        var result = await httpClient.PostAsync(settings.GenerateContactsMethodURL, null);

        if (!result.IsSuccessStatusCode)
            throw new Exception(result.ToString());
    }
    public async Task ImportContactsAsync(string token, string data, bool clear)
    {
        if (clear)
            await ClearContactsAsync(token);

        using var httpClient = NewHttpClient(token);

        var content = new StringContent(data, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(settings.ImportContactsMethodURL, content);

        if (!response.IsSuccessStatusCode)
            throw new Exception(response.ToString());

    }

    public async Task ImportContactsAsync(string token, IFormFile file, bool clear)
    {
        if (clear)
            await ClearContactsAsync(token);

        var contacts = new StringBuilder();
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
                contacts.AppendLine(await reader.ReadLineAsync());
        }

        using var httpClient = NewHttpClient(token);

        var content = new StringContent(contacts.ToString(), Encoding.UTF8, "application/json");

        var result = await httpClient.PostAsync(settings.ImportContactsMethodURL, content);

        if (!result.IsSuccessStatusCode)
            throw new Exception(result.ToString());

    }

    public async Task<List<UserInfoModel>?> ListUsersAsync(string token)
    {

        using var httpClient = NewHttpClient(token);

        var result = await httpClient.GetAsync(settings.ListUsersMethodURL);
        if (result.IsSuccessStatusCode)
            return (List<UserInfoModel>?)(await result.Content.ReadFromJsonAsync(typeof(List<UserInfoModel>)));
        else
            throw new Exception(result.ToString());

    }

    public async Task<UserInfoModel?> GetUserAsync(string token, Guid Id)
    {

        using var httpClient = NewHttpClient(token);
        var fullURL = $"{settings.GetUserMethodURL}/{Id}";
        var result = await httpClient.GetAsync(fullURL);
        if (result.IsSuccessStatusCode)
            return (UserInfoModel?)(await result.Content.ReadFromJsonAsync(typeof(UserInfoModel)));
        else
            throw new Exception(result.ToString());

    }
}
