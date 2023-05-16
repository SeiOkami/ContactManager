using Contacts.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Contacts.Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{

    private readonly UserManager<AppUser> _userManager;

    public UsersController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    //[Authorize(Roles = Configuration.AdminRoleName)]
    public async Task<ActionResult<IEnumerable<UserInfo>>> GetAll()
    {
        var result = new List<UserInfo>();

        var users = await _userManager.GetUsersInRoleAsync(Configuration.UserRoleName);

        foreach (var user in users)
        {
            result.Add(
                new()
                {
                    Id = user.Id,
                    Name = user.UserName
                });
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    //[Authorize(Roles = Configuration.AdminRoleName)]
    public async Task<ActionResult<UserInfo>> GetUser(string id)
    {
        var appUser  = await _userManager.FindByIdAsync(id.ToUpper());
        var userInfo = NewUserInfo(appUser);
        return Ok(userInfo);
    }

    private UserInfo NewUserInfo(AppUser appUser)
    {
        return new()
        {
            Id = appUser.Id,
            Name = appUser.UserName
        };
    }

}