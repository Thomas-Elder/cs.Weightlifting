using DTO.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WEB.Services;

namespace WEB.Pages.Account.Register
{
    public class AthleteModel : PageModel
    {
        [BindProperty]
        public UserRegistrationDTO Register { get; set; } = default!;
        public UserRegistrationResponseDTO Result = default!;

        private readonly IAccountService _accountService;

        public AthleteModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Result = await _accountService.RegisterAthlete(Register);

            return RedirectToPage("/Account/Login");
        }
    }
}
