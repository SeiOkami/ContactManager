using IdentityModel.OidcClient;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using Contacts.Shared.Settings;
using Contacts.Shared.Services;
using Contacts.Shared.Models;
using Contacts.DesctopClient.ViewModels;
using System.Net.Http;

namespace Contacts.DesctopClient.Identity;

public class WebAPI
{

    private OidcClient oidcClient;

    public IdentityUserModel User;
    private string token => User.Token ?? "";

    private readonly string userCancelKeyError = "UserCancel";
    private readonly string accessDeniedKeyError = "access_denied";
    private readonly IWebAPIService webApi;

    public WebAPI()
    {
        User = new();
        User.Name = Properties.Settings.Default.UserName;
        User.Token = Properties.Settings.Default.UserToken;

        User.IsAuthenticated = !String.IsNullOrEmpty(User.Token);

        webApi = new WebAPIService();

        var options = new OidcClientOptions()
        {
            Authority = SettingsManager.Settings.Identity.MainURL,
            ClientId = "WPF",
            ClientSecret = "AFD9AF9D-03E1-4F54-9E57-44B334A11B78",
            Scope = "openid profile ContactsWebAPI",
            RedirectUri = "https://localhost/sigin-wpf-app-oidc",
            Browser = new IdentityBrowser(),
            PostLogoutRedirectUri = "https://localhost/signout-wpf-app-oidc"
        };

        oidcClient = new OidcClient(options);
    }

    public async Task LoginAsync()
    {
        await LoginOnStart();
        if (User.IsAuthenticated)
            return;

        User.InProcessAuthenticated = true;

        string? error;
        try
        {
            var result = await oidcClient.LoginAsync();

            User.Token = result.AccessToken;
            User.Name = result.User?.Identity?.Name;
            User.IsAuthenticated = result.User?.Identity?.IsAuthenticated ?? false;
            User.IsAdmin = result.User?.IsInRole("Admin") ?? false;

            if (result.User != null && result.User.Identity != null
                && result.User.Identity.IsAuthenticated)
            {
                Guid id = Guid.NewGuid();
                var claim = result.User.FindFirst("sub");
                if (claim != null)
                    if (Guid.TryParse(claim.Value, out id))
                        User.Id = id;
            }

            error = result.Error;
        }
        catch (Exception)
        {
            throw;
        }

        if (error != null
            && error != userCancelKeyError
            && error != accessDeniedKeyError)
            MessageBox.Show(error);

        User.InProcessAuthenticated = false;
    }

    private async Task LoginOnStart()
    {
        if (!User.IsAuthenticated)
            return;
        
        var userInfo = await oidcClient.GetUserInfoAsync(User.Token);
        if (userInfo.IsError)
        {
            User.IsAuthenticated = false;
            return;
        }
        
        foreach (var claim in userInfo.Claims)
        {
            if (claim.Type == "sub")
            {
                Guid id1 = Guid.NewGuid();
                if (claim != null)
                    if (Guid.TryParse(claim.Value, out id1))
                        User.Id = id1;
            }
            else if (claim.Type == "role" && claim.Value == "Admin")
            {
                User.IsAdmin = true;
            }
        }
    }

    public void Logout()
    {
        User.IsAuthenticated = false;
        User.InProcessAuthenticated = false;

        User.Name = String.Empty;
    }

    public async Task<List<UserInfoModel>?> ListUsersAsync()
    {
        try
        {
            return await webApi.ListUsersAsync(token);
        }
        catch (Exception ex)
        {
            ShowError(ex);
            return null;
        }
    }

    public async Task<ContactsViewModel?> ListContactsAsync(Guid UserId)
    {
        try
        {
            return await webApi.ListContactsAsync<ContactsViewModel>(token, UserId);
        }
        catch (Exception ex)
        {
            ShowError(ex);
            return null;
        }
    }

    public async Task<ContactViewModel?> GetContactAsync(Guid ID)
    {
        try
        {
            return await webApi.GetContactAsync<ContactViewModel>(token, ID);
        }
        catch (Exception ex)
        {
            ShowError(ex);
            return null;
        }
    }

    public async Task UpdateContactAsync(ContactViewModel contact)
    {
        try
        {
            await webApi.UpdateContactAsync<ContactViewModel>(token, contact);
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
    }

    public async Task<Guid?> CreateContactAsync(ContactModel contact)
    {
        try
        {
            return await webApi.CreateContactAsync<ContactModel>(token, contact);
        }
        catch (Exception ex)
        {
            ShowError(ex);
            return null;
        }
    }

    public async Task<bool> DeleteContactAsync(Guid ID)
    {
        try
        {
            await webApi.DeleteContactAsync(token, ID);
            return true;
        }
        catch (Exception ex)
        {
            ShowError(ex);
            return false;
        }
    }

    public async Task<Stream?> ExportContactsAsync()
        => await webApi.ExportContactsAsync(token);

    public async Task ClearContactsAsync()
        => await webApi.ClearContactsAsync(token);

    public async Task GenerateContactsAsync()
        => await webApi.GenerateContactsAsync(token);

    public async Task ImportContactsAsync(string data)
        => await webApi.ImportContactsAsync(token, data, false);
    
    private void ShowError(Exception ex)
    {
        MessageBox.Show(ex.Message);
    }

}
