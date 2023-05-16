using Contacts.WebClient.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Contacts.WebClient.Services
{
    public class WebAPIService : IWebAPIService
    {
        public WebAPIServiceSettings Settings { get; set; }
        
        private ITokenService tokenService;
        
        public WebAPIService(ITokenService tokenService, IOptions<WebAPIServiceSettings> options)
        {
            this.tokenService = tokenService;
            this.Settings = options.Value;
        }        

        public async Task<ContactsModel?> ListContacts(HttpContext сontext, Guid? userID)
        {
            
            using var httpClient = await NewHttpClient(сontext);

            var fullPath = Settings.ListContactsMethodURL;
            if (userID != null)
                fullPath = $"{fullPath}/{userID}";

            var result = await httpClient.GetAsync(fullPath);
            if (result.IsSuccessStatusCode)
                return (ContactsModel?)(await result.Content.ReadFromJsonAsync(typeof(ContactsModel)));
            else
                throw new Exception(result.ToString());            

        }

        public async Task<Stream> ExportContacts(HttpContext сontext)
        {
            using var httpClient = await NewHttpClient(сontext);

            var result = await httpClient.GetAsync(Settings.ListContactsMethodURL);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsStreamAsync();
            }
            else
            {
                throw new Exception(result.ToString());
            }
        }

        public async Task CreateContact(HttpContext сontext, ContactModel contact)
        {
            using var httpClient = await NewHttpClient(сontext);
            
            var json = JsonConvert.SerializeObject(contact);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(Settings.CreateContactMethodURL, data);

            if (!result.IsSuccessStatusCode)
                throw new Exception(result.ToString());
        }

        private async Task<HttpClient> NewHttpClient(HttpContext сontext)
        {
            var token = await tokenService.GetToken(сontext);

            var httpClient = new HttpClient();
            httpClient.SetBearerToken(token);
            
            return httpClient;
        }

        public async Task UpdateContact(HttpContext сontext, ContactModel contact)
        {
            using var httpClient = await NewHttpClient(сontext);

            var fullURL = Settings.UpdateContactMethodURL;

            var json = JsonConvert.SerializeObject(contact);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PutAsync(fullURL, data);

            if (!result.IsSuccessStatusCode)
                throw new Exception(result.ToString());
        }

        public async Task<ContactModel?> GetContactAsync(HttpContext сontext, Guid ID)
        {
            using var httpClient = await NewHttpClient(сontext);

            var fullURL = $"{Settings.DetailsContactMethodURL}/{ID}";

            var result = await httpClient.GetAsync(fullURL);
            if (result.IsSuccessStatusCode)
                return (ContactModel?)(await result.Content.ReadFromJsonAsync(typeof(ContactModel)));
            else
                throw new Exception(result.ToString());
        }

        public async Task DeleteContact(HttpContext сontext, Guid ID)
        {
            using var httpClient = await NewHttpClient(сontext);

            var fullURL = $"{Settings.DeleteContactMethodURL}/{ID}";

            var result = await httpClient.DeleteAsync(fullURL);

            if (!result.IsSuccessStatusCode)
                throw new Exception(result.ToString());
        }

        public async Task ClearContacts(HttpContext сontext)
        {
            using var httpClient = await NewHttpClient(сontext);

            var result = await httpClient.DeleteAsync(Settings.ClearContactsMethodURL);

            if (!result.IsSuccessStatusCode)
                throw new Exception(result.ToString());
        }

        public async Task GenerateContacts(HttpContext сontext)
        {
            using var httpClient = await NewHttpClient(сontext);

            var result = await httpClient.PostAsync(Settings.GenerateContactsMethodURL, null);

            if (!result.IsSuccessStatusCode)
                throw new Exception(result.ToString());
        }

        public async Task ImportContacts(HttpContext сontext, ImportContactsModel model)
        {

            var contacts = new StringBuilder();
            using (var reader = new StreamReader(model.FileContacts.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    contacts.AppendLine(await reader.ReadLineAsync());
            }

            using var httpClient = await NewHttpClient(сontext);

            var content = new StringContent(contacts.ToString(), Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(Settings.ImportContactsMethodURL, content);

            if (!result.IsSuccessStatusCode)
                throw new Exception(result.ToString());

        }

        public async Task<List<UserModel>?> ListUsers(HttpContext сontext)
        {

            using var httpClient = await NewHttpClient(сontext);

            var result = await httpClient.GetAsync(Settings.ListUsersMethodURL);
            if (result.IsSuccessStatusCode)
                return (List<UserModel>?)(await result.Content.ReadFromJsonAsync(typeof(List<UserModel>)));
            else
                throw new Exception(result.ToString());

        }

        public async Task<UserModel?> GetUser(HttpContext сontext, Guid Id)
        {

            using var httpClient = await NewHttpClient(сontext);
            var fullURL = $"{Settings.GetUserMethodURL}/{Id}";
            var result = await httpClient.GetAsync(fullURL);
            if (result.IsSuccessStatusCode)
                return (UserModel?)(await result.Content.ReadFromJsonAsync(typeof(UserModel)));
            else
                throw new Exception(result.ToString());

        }
    }
}
