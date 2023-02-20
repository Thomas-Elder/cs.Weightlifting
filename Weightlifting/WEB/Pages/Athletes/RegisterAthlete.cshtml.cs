using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB.ViewModels.Athletes;
using WEB.Services;

namespace WEB.Pages.Athletes
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterAthlete Register { get; set; } = default!;

        private readonly IAccountService _accountService;

        public string Result = default!;

        public RegisterModel(IAccountService accountService)
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