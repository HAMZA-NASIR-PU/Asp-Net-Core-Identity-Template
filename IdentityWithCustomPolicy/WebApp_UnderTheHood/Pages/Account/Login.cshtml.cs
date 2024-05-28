using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace WebApp_UnderTheHood.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; } = new Credential();

        public void OnGet()
        {
        }

		public async Task<IActionResult> OnPost()
		{
            if (!ModelState.IsValid) return Page();

            //Verify the credentials
            if(Credential.UserName == "admin" && Credential.Password == "password")
            {
				//Creating the security context.
				List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@email.com"),
                    new Claim("Department", "HR"),
                    new Claim("Manager", "true"),
                    new Claim("Admin", "true"),
                    new Claim("EmploymentDate", "2024-02-01")
                };

				ClaimsIdentity identity = new ClaimsIdentity(claims, "MyCookieAuth");   //See its function signature.
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };

                await HttpContext.SignInAsync("MyCookieAuth", principal, authProperties); //SignInAsync is an extension method

                return RedirectToPage("/Index");
            }else
            {
				// Add a model error for invalid credentials
				ModelState.AddModelError(string.Empty, "Invalid username or password");
			}

            return Page();
		}
	}

    public class Credential
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;
 
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}
