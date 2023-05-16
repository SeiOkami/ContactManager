using Contacts.WebClient.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.WebClient.Controllers
{
    public class UsersController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly IWebAPIService _webAPI;

        public UsersController(ITokenService tokenService, IWebAPIService webAPI)
        {
            _tokenService = tokenService;
            _webAPI = webAPI;
        }

        [HttpGet()]
        [Authorize(Roles = Configuration.RoleAdmin)]
        public async Task<ActionResult> Index()
        {
            var users = await _webAPI.ListUsers(HttpContext);
            return View(users);
        }
    }
}
