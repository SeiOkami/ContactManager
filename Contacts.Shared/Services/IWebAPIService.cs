using Contacts.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace Contacts.Shared.Services;

public interface IWebAPIService
{
    public Task<ContactsModel?> ListContactsAsync(string token, Guid? userID);
    public Task<T?> ListContactsAsync<T>(string token, Guid? userID);
    public Task<Stream> ExportContactsAsync(string token);
    public Task ImportContactsAsync(string token, string data, bool clear = false);
    public Task ImportContactsAsync(string token, IFormFile file, bool clear = false);
    public Task<Guid?> CreateContactAsync(string token, ContactModel contact);
    public Task<Guid?> CreateContactAsync<T>(string token, T contact);
    public Task UpdateContactAsync(string token, ContactModel contact);
    public Task UpdateContactAsync<T>(string token, T contact);
    public Task<ContactModel?> GetContactAsync(string token, Guid Id);
    public Task<T?> GetContactAsync<T>(string token, Guid Id);
    public Task DeleteContactAsync(string token, Guid Id);
    public Task ClearContactsAsync(string token);
    public Task GenerateContactsAsync(string token, bool clear = false);
    public Task<List<UserInfoModel>?> ListUsersAsync(string token);
    public Task<UserInfoModel?> GetUserAsync(string token, Guid Id);

}
