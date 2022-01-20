// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Infrastructure.Configurations;
using CleanArchitecture.Razor.Infrastructure.Identity;
using CleanArchitecture.Razor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAdmin.WebUI.Extensions;
using SmartAdmin.WebUI.Models;

namespace SmartAdmin.WebUI.EndPoints;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersEndpoint : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _manager;
    private readonly SmartSettings _settings;

    public UsersEndpoint(ApplicationDbContext context, UserManager<ApplicationUser> manager, SmartSettings settings)
    {
        _context = context;
        _manager = manager;
        _settings = settings;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ApplicationUser>>> Get()
    {
        var users = await _manager.Users.AsNoTracking().ToListAsync();
        return Ok(new { data = users, recordsTotal = users.Count, recordsFiltered = users.Count });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApplicationUser>> Get([FromRoute] string id) => Ok(await _manager.FindByIdAsync(id));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromForm] ApplicationUser model)
    {
        model.Id = Guid.NewGuid().ToString();
        model.UserName = model.UserName ?? model.Email.Split('@')[0];

        var result = await _manager.CreateAsync(model);

        if (result.Succeeded)
        {
            // HACK: This password is just for demonstration purposes!
            // Please do NOT keep it as-is for your own project!
            result = await _manager.AddPasswordAsync(model, "Password123!");

            if (result.Succeeded)
            {
                return CreatedAtAction("Get", new { id = model.Id }, model);
            }
        }

        return BadRequest(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromForm] ApplicationUser model)
    {
        var user = await this._manager.FindByIdAsync(model.Id);
        if (user != null)
        {
            user.Site = model.Site;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.DisplayName = model.DisplayName;
            user.EmailConfirmed = model.EmailConfirmed;
            user.PhoneNumber = model.PhoneNumber;
            user.LockoutEnabled = model.LockoutEnabled;
            var result = await this._manager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result);
            }

        }


        return BadRequest(IdentityResult.Failed());
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromForm] ApplicationUser model)
    {
        // HACK: The code below is just for demonstration purposes!
        // Please use a different method of preventing the currently logged in user from being removed
        if (model.UserName == _settings.Theme.Email)
        {
            return BadRequest(SmartError.Failed("Please do not delete the main user! =)"));
        }

        var result = await _context.DeleteAsync<ApplicationUser>(model.Id);

        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result);
    }
}
