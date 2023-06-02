using Contacts.WebClient.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using Contacts.WebClient;
using System.Security.Claims;
using Polly;

var builder = WebApplication.CreateBuilder(args);

Policy.Handle<Exception>()
    .WaitAndRetryAsync(
        retryCount: 20,
        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(1),
        onRetry: (exception, retryCount) =>
        {
            Console.WriteLine($"Retry {retryCount} due to {exception.Message}");
        })
    .ExecuteAsync(async () =>
    {
        using var client = new HttpClient();
        var response = await client.GetAsync("https://localhost:7058/");
        response.EnsureSuccessStatusCode();
    }).Wait();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllersWithViews();

services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {

        options.Authority = configuration["InteractiveServiceSettings:AuthorityUrl"];
        options.ClientId = Configuration.ClientName;
        options.ClientSecret = Configuration.SecretPassword;

        options.ResponseType = "code";
        options.UsePkce = true;
        options.ResponseMode = "query";
        options.SaveTokens = true;

        options.Scope.Add(Configuration.ApiScopeName);

    });

services.AddSingleton<ITokenService, TokenService>();
services.AddSingleton<IWebAPIService, WebAPIService>();
services.Configure<WebAPIServiceSettings>(configuration.GetSection("SettingsWebAPI"));


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contacts}/{action=Index}"
    );

app.Run();
