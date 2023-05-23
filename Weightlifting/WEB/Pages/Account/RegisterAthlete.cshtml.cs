using DTO.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WEB.Services;

namespace WEB.Pages.Account
{
    public class RegisterAthleteModel : PageModel
    {
        [BindProperty]
        public UserRegistrationDTO Register { get; set; } = default!;

        private readonly IAccountService _accountService;

        public UserRegistrationResponseDTO Result = default!;

        public RegisterAthleteModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterAthlete(Register);

                Result = result;

                return Page();
            } else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                return Page();
            }
        }
    }
}
