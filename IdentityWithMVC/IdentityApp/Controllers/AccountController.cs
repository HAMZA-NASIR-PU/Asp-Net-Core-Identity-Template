using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Models;
using IdentityApp.ViewModels;
using IdentityApp.Services;
using System.Security.Claims;
using Microsoft.Extensions.Logging.Abstractions;

namespace IdentityApp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        private SignInManager<AppUser> _signInManager;
        private IEmailSender _emailSender;
        private IJwtToken _jwtToken;
        private IOtpService _otpService;

        public AccountController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager,
            IEmailSender emailSender,
            IJwtToken jwtToken,
            IOtpService otpService)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
            this._jwtToken = jwtToken;
            _otpService = otpService;
        }

        public IActionResult Login()
        {
            if(User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            } 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await this._userManager.FindByEmailAsync(model.Email);
                var isCorrectPassword = await _userManager.CheckPasswordAsync(user!, model.Password);

                if (user!=null && isCorrectPassword)
                {
                    if(!await this._userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Your email is not confirmed.");
                        return View(model);
                    }

                    //This lockout is for when the user has failed in providing correct email and password.
                    var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                    if(lockoutDate.HasValue && lockoutDate > DateTime.UtcNow)
                    {
                        var timeLeft = lockoutDate.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Your account has been locked, please try in {timeLeft.Minutes} minutes and {timeLeft.Seconds} seconds.");
                        return View(model);
                    }
                    await this._userManager.ResetAccessFailedCountAsync(user);
                    await this._userManager.SetLockoutEndDateAsync(user, null);

                    //This lockout is for when the user has failed in providing correct OTP.
                    if (user.OtpLockoutEnd.HasValue && user.OtpLockoutEnd > DateTime.UtcNow)
                    {
                        var timeLeft = user.OtpLockoutEnd.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Your account has been locked, please try in {timeLeft.Minutes} minutes and  {timeLeft.Seconds}  seconds.");
                        return View(model);
                    }

                    var otp = this._otpService.GenerateOtp();
                    user.Otp = otp;
                    user.OtpExpiration = DateTime.UtcNow.AddMinutes(5);
                    user.FailedOtpAttempts = 0;
                    user.OtpLockoutEnd = null;
                    await _userManager.UpdateAsync(user);

                    // Send OTP to user via email or SMS here

                    var token = this._jwtToken.GenerateToken(model.Email, otp, model.RememberMe);

                    return RedirectToAction("EnterOtp", "Account", new { token });
                }
                else if(user != null && !isCorrectPassword)
                {
                    // Password is incorrect, increment access failed count
                    await _userManager.AccessFailedAsync(user);
                    ModelState.AddModelError("", "Your password is incorrect");
                }
                else
                {
                    ModelState.AddModelError("", "No account found with this email address");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult EnterOtp(string token)
        {
            ClaimsPrincipal principal = this._jwtToken.ValidateToken(token);
            if (principal == null)
            {
                return RedirectToAction("Login", "Account");
            }

            OtpViewModel otpViewModel = new OtpViewModel();
            otpViewModel.Token = token;
            //IEnumerable<Claim> claims = principal.Claims;
            //otpViewModel.Email = claims.Where(c => c.Type == "email").FirstOrDefault()!.Value;
            return View(otpViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EnterOtp(OtpViewModel model, string token)
        {
            var principal = this._jwtToken.ValidateToken(token);
            if (principal == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var isPersistentClaim = principal.Claims.FirstOrDefault(c => c.Type == "isPersistent");

            if (emailClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var user = await _userManager.FindByEmailAsync(emailClaim.Value);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (user.OtpLockoutEnd.HasValue && user.OtpLockoutEnd > DateTime.UtcNow)
            {
                var timeLeft = user.OtpLockoutEnd.Value - DateTime.UtcNow;
                TempData["message"] = $"Your account has been locked, please try in {timeLeft.Minutes} minutes and {timeLeft.Seconds} seconds.";
                return RedirectToAction("Login", "Account");
            }

            if (user.OtpExpiration < DateTime.UtcNow)
            {
                //ModelState.AddModelError(string.Empty, "OTP has expired. Please request a new OTP.");
                TempData["message"] = "OTP has expired. Please request a new OTP.";
                return RedirectToAction("Login", "Account");
            }

            if (model.Otp == user.Otp)
            {
                // OTP is valid, complete the authentication process
                user.Otp = null;
                user.OtpExpiration = null;
                user.FailedOtpAttempts = 0;
                user.OtpLockoutEnd = null;
                await _userManager.UpdateAsync(user);

                // Fully sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                user.FailedOtpAttempts += 1;
                if (user.FailedOtpAttempts >= 3)
                {
                    user.OtpLockoutEnd = DateTime.UtcNow.AddMinutes(3);
                    user.Otp = null;
                    user.FailedOtpAttempts = 0;
                    user.OtpExpiration = null;
                }

                await _userManager.UpdateAsync(user);

                ModelState.AddModelError("Otp", "Invalid OTP.");
                return View(model);
            }
        }
    }
}
