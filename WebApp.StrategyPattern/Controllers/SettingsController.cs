using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.StrategyPattern.Models;

namespace WebApp.StrategyPattern.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> signInManager;

        public SettingsController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            this.signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            Settings settings= new();
            if (User.Claims.Where(x=> x.Type== Settings.ClaimdatabaseType).FirstOrDefault() != null)
            {
                settings.DatabaseType=(EDatabaseType)int.Parse(User.Claims.First(x=> x.Type==Settings.ClaimdatabaseType).Value);
            }
            else
            {
                settings.DatabaseType = settings.GetDefaultDatabaseType;
            }
            return View(settings);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeDatabse(int databaseType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var newClaim = new Claim(Settings.ClaimdatabaseType, databaseType.ToString());
            var claims=await _userManager.GetClaimsAsync(user);
            var hasDatabaseTypeClaim = claims.FirstOrDefault(x => x.Type == Settings.ClaimdatabaseType);
            if(hasDatabaseTypeClaim != null) { 
            await _userManager.ReplaceClaimAsync(user, hasDatabaseTypeClaim,newClaim);
            }
            else
            {
                await _userManager.AddClaimAsync(user, newClaim);
            }

            await signInManager.SignOutAsync();
            var result=await HttpContext.AuthenticateAsync();
            await signInManager.SignInAsync(user, result.Properties);
            return RedirectToAction("Index");

        }
    }
}
