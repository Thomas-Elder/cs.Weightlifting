using DTO.Athletes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Services;
using WEB.Services.Interfaces;

namespace WEB.Pages.Athlete
{
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IAthleteService _athleteService;

        public AthleteDetailsDTO? AthleteDetailsDTO { get; set; }

        public DetailsModel(IAthleteService athleteService,
            IAccountService accountService)
        {
            _athleteService = athleteService;
            _accountService = accountService;
        }   

        public async void OnGetAsync()
        {
            // Call the AthleteService with get Details
            // Set that info as the AthleteDetailsDTO
            AthleteDetailsDTO = await _athleteService.Details();
        }
    }
}
