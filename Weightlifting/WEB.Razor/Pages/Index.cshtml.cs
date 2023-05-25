using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Services;

namespace WEB.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHomeService _homeService;

        public string Result;

        public IndexModel(ILogger<IndexModel> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        public async Task OnGetAsync()
        {
            Result = await _homeService.CheckConnection();
        }
    }
}