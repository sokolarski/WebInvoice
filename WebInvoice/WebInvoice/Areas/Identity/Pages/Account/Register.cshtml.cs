using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using WebInvoice.Data.AppData.Models;
using WebInvoice.Dto.Company;
using WebInvoice.Services;

namespace WebInvoice.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ICompanyService companyService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ICompanyService companyService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.companyService = companyService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} трябва да бъде между {2} и {1} символа.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърди паролата")]
            [Compare("Password", ErrorMessage = "Паролата не е еднаква!")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Полето е задължително!")]
            [MaxLength(50, ErrorMessage = "Максимална дължина 50 символа")]
            [Display(Name = "Име на фирма")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Полето е задължително!")]
            [MaxLength(150, ErrorMessage = "Максимална дължина 150 символа")]
            [Display(Name = "Адрес")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Полето е задължително!")]
            [MaxLength(50, ErrorMessage = "Максимална дължина 50 символа")]
            [Display(Name = "Град")]
            public string City { get; set; }

            [Required(ErrorMessage = "Полето е задължително!")]
            [MaxLength(50, ErrorMessage = "Максимална дължина 50 символа")]
            [Display(Name = "Държава")]
            public string Country { get; set; }

            [MaxLength(20, ErrorMessage = "ЕИК трябва да бъде точно 9 цифри")]
            [RegularExpression(@"^[0-9]{2,13}$", ErrorMessage = "ЕИК номер трябва да започва с две латински букви последвани от 2 до 13 символа")]
            [Display(Name = "ЕИК")]
            public string EIK { get; set; }

            [Display(Name = "ДДС номер")]
            [RegularExpression(@"^[A-Z]{2}[0-9A-Z]{2,13}$", ErrorMessage = "ДДС номер трябва да започва с две главни латински букви последвани от 2 до 13 символа")]
            public string VatId { get; set; }

            [EmailAddress]
            [Display(Name = "Електронна поща на фирмата")]
            public string CompanyEmail { get; set; }

            [Required(ErrorMessage = "Полето е задължително!")]
            [MaxLength(50, ErrorMessage = "Максимална дължина 50 символа")]
            [Display(Name = "Метериално отгорвоно лице (МОЛ)")]
            public string MOL { get; set; }

            [Display(Name = "Регистрация по ДДС")]
            public bool IsVatRegistered { get; set; }

            public string LogoPath { get; set; }

            [MaxLength(300, ErrorMessage = "Максимална дължина 300 символа")]
            [Display(Name = "Пояснения")]
            public string Description { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    await CreateCompanyAsync(this.Input, user.Id);
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task CreateCompanyAsync(InputModel input, string id)
        {
            var company = new CompanyInputDto()
            {
                Address = input.Address,
                City = input.City,
                Country = input.Country,
                Description = input.Description,
                EIK = input.EIK,
                Email = input.CompanyEmail,
                IsVatRegistered = input.IsVatRegistered,
                LogoPath = input.LogoPath,
                MOL = input.MOL,
                Name = input.Name,
                VatId = input.VatId,
            };
            await companyService.CreateCompanyAsync(company,id);
        }
    }
}
