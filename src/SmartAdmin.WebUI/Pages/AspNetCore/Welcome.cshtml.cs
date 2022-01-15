// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace SmartAdmin.WebUI.Pages.AspNetCore;

[Authorize()]
public class WelcomeModel : PageModel
{
    static int _callCount;
    private readonly ILogger<WelcomeModel> _logger;
    private readonly IQiniuService _qiniuService;
    private readonly IHostEnvironment _environment;
    private readonly IDiagnosticContext _diagnosticContext;

    public WelcomeModel(ILogger<WelcomeModel> logger,
        IQiniuService qiniuService,
        IHostEnvironment environment,
        IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _qiniuService = qiniuService;
        _environment = environment;
        _diagnosticContext = diagnosticContext;

    }

    public void OnGet()
    {
        _logger.LogInformation("Welcome.");
        _diagnosticContext.Set("IndexCallCount", Interlocked.Increment(ref _callCount));
    }
    public Task<JsonResult> OnGetFilter(string input)
    {
        return Task.FromResult(new JsonResult(input));
    }
    public Task<JsonResult> OnPost(string input)
    {
        return Task.FromResult(new JsonResult(string.Empty));
    }
}
