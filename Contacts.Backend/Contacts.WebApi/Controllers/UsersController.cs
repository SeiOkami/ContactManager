﻿using Contacts.WebApi.Models;
using Contacts.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.WebApi.Controllers
{
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IdentityServerService _identityServer;

        public UsersController(IdentityServerService identityServer)
        {
            _identityServer = identityServer;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = Shared.Identity.Roles.Admin)]
        public async Task<ActionResult<IEnumerable<IdentityUserInfo>>> GetAll()
        {
            var users = await _identityServer.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("Get/{id}")]
        [Authorize]
        public async Task<ActionResult<IdentityUserInfo>> Get(Guid id)
        {
            var user = await _identityServer.GetUserAsync(id);
            return Ok(user);
        }

    }
}
