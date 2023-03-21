using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.Services;
using WEB.ViewModels.Account;

namespace WEB.Pages.Account
{
    public class RegisterAthleteModel : PageModel
    {
        [BindProperty]
        public RegisterAthlete Register { get; set; } = default!;

        private readonly IAccountService _accountService;

        public string Result = default!;

        public RegisterAthleteModel(IAccountService accountService)
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
                var result = await _accountService.RegisterAthlete(Register);

                Result = result;

                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
