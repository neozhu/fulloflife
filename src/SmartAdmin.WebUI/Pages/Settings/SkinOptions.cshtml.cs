using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartAdmin.WebUI.Pages.Settings
{
    public class SkinOptionsModel : PageModel
    {
        private readonly ILogger<SkinOptionsModel> _logger;

        public SkinOptionsModel(ILogger<SkinOptionsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
