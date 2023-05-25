using DTO.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using WEB.Services;

namespace WEB.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public UserAuthenticationDTO Login { get; set; } = default!;
        public UserAuthenticationResponseDTO Result = default!;

        private readonly IAccountService _accountService;

        public LoginModel(IAccountService accountService)
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

            Result = await _accountService.Login(Login);

            // Probably go to landing page once that exists.
            return RedirectToPage("/Index");
        }
    }
}
