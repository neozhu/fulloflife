using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace SmartAdmin.WebUI.Pages.AspNetCore
{
    [Authorize()]
    public class WelcomeModel : PageModel
    {
        static int _callCount;
        private readonly ILogger<WelcomeModel> _logger;
        private readonly IQiniuService _qiniuService;
        private readonly Microsoft.Extensions.Hosting.IHostingEnvironment _environment;
        private readonly IDiagnosticContext _diagnosticContext;

        public WelcomeModel(ILogger<WelcomeModel> logger,
            IQiniuService qiniuService,
            Microsoft.Extensions.Hosting.IHostingEnvironment environment,
            IDiagnosticContext diagnosticContext)
        {
            _logger = logger;
            _qiniuService = qiniuService;
            _environment = environment;
            _diagnosticContext = diagnosticContext;

        }

        public void OnGet()
        {
            var path = Path.Combine(_environment.ContentRootPath, "wwwroot/img/logo.png");
            var buffer= System.IO.File.ReadAllBytes(path);
             _qiniuService.Upload(buffer, "logo.png");
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
}
