using Contacts.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Contacts.Shared.Services;

public interface IWebAPIService
{
    public Task<ContactsModel?> ListContactsAsync(string token, Guid? userID);
    public Task<Stream> ExportContactsAsync(string token);
    public Task ImportContactsAsync(string token, string data, bool clear = false);
    public Task ImportContactsAsync(string token, IFormFile file, bool clear = false);
    public Task CreateContactAsync(string token, ContactModel contact);
    public Task UpdateContactAsync(string token, ContactModel contact);
    public Task<ContactModel?> GetContactAsync(string token, Guid Id);
    public Task DeleteContactAsync(string token, Guid Id);
    public Task ClearContactsAsync(string token);
    public Task GenerateContactsAsync(string token, bool clear = false);
    public Task<List<UserInfoModel>?> ListUsersAsync(string token);
    public Task<UserInfoModel?> GetUserAsync(string token, Guid Id);

}
