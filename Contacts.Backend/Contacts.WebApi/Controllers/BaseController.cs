﻿using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.WebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {

        private IMediator _mediator = null!;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>() ?? null!;

        internal bool IsAdmin => User?.IsInRole(Shared.Identity.Roles.Admin) ?? false;

        internal Guid UserId
        {
            get
            {
                var result = Guid.Empty;

                if (User != null && User.Identity != null 
                    && User.Identity.IsAuthenticated)
                {
                    var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null)
                        Guid.TryParse(claim.Value, out result);
                }
                
                return result;
            }
        }
    }
}
