using Contacts.WebClient.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contacts.Shared.Services;
using Contacts.WebClient.Extensions;

namespace Contacts.WebClient.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IWebAPIService _webAPI;

        public UsersController(IWebAPIService webAPI)
        {
            _webAPI = webAPI;
        }

        [HttpGet()]
        [Authorize(Roles = Shared.Identity.Roles.Admin)]
        public async Task<ActionResult> Index()
        {
            var token = await HttpContext.GetTokenAsync();
            var users = await _webAPI.ListUsersAsync(token);
            return View(users);
        }
    }
}
