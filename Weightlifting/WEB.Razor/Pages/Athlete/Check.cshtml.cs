using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Services;
using WEB.Services.Interfaces;

namespace WEB.Pages.Athlete
{
    public class CheckModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IAthleteService _athleteService;

        public string Result = default!;

        public CheckModel(ILogger<IndexModel> logger, IAthleteService athleteService)
        {
            _logger = logger;
            _athleteService = athleteService;
        }

        public async Task OnGetAsync()
        {
            Result = await _athleteService.Check();
        }
    }
}
