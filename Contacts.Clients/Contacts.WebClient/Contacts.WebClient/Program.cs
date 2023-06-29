using Contacts.WebClient;
using Contacts.Shared.LaunchManager;
using Contacts.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

var sharedSettings = Contacts.Shared.Settings.SettingsManager.Settings;
var launch = new LaunchManager(new()
{
    ExpectedAddress = sharedSettings.WebAPI.MainURL,
});
launch.OnStart();

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

services.AddSingleton<IWebAPIService, WebAPIService>();


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
