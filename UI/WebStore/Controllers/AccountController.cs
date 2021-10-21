using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(
            UserManager<User> UserManager, 
            SignInManager<User> SignInManager,
            ILogger<AccountController> Logger)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _Logger = Logger; // WebStore.Controllers.AccountController
        }

        #region Register

        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            using (_Logger.BeginScope("Регистрация пользователя {UserName}", Model.UserName))
            {
                var user = new User
                {
                    UserName = Model.UserName,
                };

                //_Logger.LogInformation("Регистрация пользователя {0}", user.UserName);
                _Logger.LogInformation("Регистрация пользователя {UserName}", user.UserName);
                //_Logger.LogInformation($"Регистрация пользователя {user.UserName}"); // не надо так!

                var register_result = await _UserManager.CreateAsync(user, Model.Password);
                if (register_result.Succeeded)
                {
                    _Logger.LogInformation("Пользователь {0} успешно зарегистрирован", user.UserName);

                    await _UserManager.AddToRoleAsync(user, Role.Users);

                    _Logger.LogInformation("Пользователю {0} назначена роль {1}",
                        user.UserName, Role.Users);

                    await _SignInManager.SignInAsync(user, false);
                    _Logger.LogInformation("Пользователь {0} вошёл в систему после регистрации", user.UserName);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in register_result.Errors)
                    ModelState.AddModelError("", error.Description);

                _Logger.LogWarning("Ошибка при регистрации пользователя {0}: {1}",
                    user.UserName, string.Join(", ", register_result.Errors.Select(err => err.Description)));
            }

            return View(Model);
        }

        #endregion

        #region Login

        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl });

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var login_result = await _SignInManager.PasswordSignInAsync(
                Model.UserName,
                Model.Password,
                Model.RememberMe,
                false);

            if (login_result.Succeeded)
            {
                _Logger.LogInformation("Пользователь {0} успешно вошёл в систему", Model.UserName);

                //return Redirect(Model.ReturnUrl); // Не безопасно!!!
                //if (Url.IsLocalUrl(Model.ReturnUrl))
                //    return Redirect(Model.ReturnUrl);
                //return RedirectToAction("Index", "Home");
                return LocalRedirect(Model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Ошибка ввода имени пользователя, или пароля");

            _Logger.LogWarning("Ошибка ввода пользователя, или пароля при входе {0}", Model.UserName);

            return View(Model);
        } 

        #endregion

        public async Task<IActionResult> Logout()
        {
            var user_name = User.Identity!.Name;
            await _SignInManager.SignOutAsync();

            _Logger.LogInformation("Пользователь {0} вышел из системы", user_name);

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            _Logger.LogWarning("Отказано в доступе {0} к uri:{1}",
                User.Identity!.Name, HttpContext.Request.Path);
            return View();
        }
    }
}
