using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AssetStore.Models;
using System.Web.Security;


namespace AssetStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private ItemDbContext _itemContext;
        private AssetsDbContext _context;
        private PublisherModelContext _publisherContext;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
            _context = new AssetsDbContext();
            _itemContext = new ItemDbContext();
            _publisherContext = new PublisherModelContext();
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult AddUserToRole() {
  
            var model = new AddToRoleModel();
            model.roles = new System.Collections.Generic.List<string>();
            model.roles.Add("Administrator");
            model.roles.Add("Publisher");
            model.roles.Add("User");
            return View(model);
        }

        public ActionResult RemoveFromCart(int Id) {
            Item tmp = _itemContext.Assets.FirstOrDefault(z => z.Id == Id);
            _itemContext.Assets.Remove(tmp);
            _itemContext.SaveChanges();
            return RedirectToAction("Cart", _itemContext.Assets.ToList());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddUserToRole(AddToRoleModel model)
        {
            try
            {
                var user = UserManager.FindByEmail(model.Email);

                UserManager.AddToRole(user.Id, model.Role);
                return View("AddUserToRole", model);
            }
            catch (Exception ex)
            {

                return HttpNotFound();
            }
            }

        [Authorize(Roles = "Administrator")]
        public ActionResult GetAllUsers() {
           
            List<AssetStore.Models.ApplicationUser> list = UserManager.Users.ToList();
            
            return View(list);
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteUser(string Id) {
            var user = UserManager.FindById(Id);
            UserManager.Delete(user);
            return View("GetAllUsers", UserManager.Users.ToList());
        }

        public ActionResult DetailsUser(string Id) {
            var user = UserManager.FindById(Id);
            return View(user);
        }

        [Authorize]
        public ActionResult Cart() {
            List<Item> items= new List<Item>(); /*= _itemContext.Assets.Where(z => z.buyerId == User.Identity.GetUserId()).ToList();*/

            foreach (Item item in _itemContext.Assets) {
                if (item.buyerId == User.Identity.GetUserId() && !item.isBought) {
                    items.Add(item);
                }
            }

            return View(items);   
        }
        [Authorize]
        public ActionResult MyAssets() {

            List<Item> items = new List<Item>();
            var user_id = User.Identity.GetUserId();
            foreach (Item item in _itemContext.Assets)
            {
                if (item.buyerId == user_id && item.isBought)
                {
                    items.Add(item);
                }
            }

            return View(items);
        }
        [Authorize]
        public ActionResult ProceedToCheckout() {
            string user_id = User.Identity.GetUserId();
            foreach (Item item in _itemContext.Assets) {
                if (item.buyerId.Equals(user_id)) {
                    item.isBought = true;
                    item.isInCart = false;
                }
            }
            _itemContext.SaveChanges();
            return RedirectToAction("Cart", _itemContext.Assets.ToList());
        }
        public ActionResult DownloadAsset3D(int Id)
        {
            var model = _context.Assets3D.FirstOrDefault(z => z.Id == Id);
            return new FileContentResult(model.assetZip, System.Net.Mime.MediaTypeNames.Application.Zip)
            {
                FileDownloadName = model.Name + ".zip"
            };
        }

        public ActionResult DownloadAsset2D(int Id)
        {
            var model = _context.Assets2D.FirstOrDefault(z => z.Id == Id);
            return new FileContentResult(model.assetZip, System.Net.Mime.MediaTypeNames.Application.Zip) {
                FileDownloadName = model.Name+".zip"
            };
        }

        public ActionResult DownloadAssetAudio(int Id)
        {
            var model = _context.AssetsAudio.FirstOrDefault(z => z.Id == Id);
            return new FileContentResult(model.assetZip, System.Net.Mime.MediaTypeNames.Application.Zip) {
                FileDownloadName = model.Name + ".zip"
            };
        }

        public ActionResult DownloadAssetTools(int Id)
        {
            var model = _context.AssetsTools.FirstOrDefault(z => z.Id == Id);
            return new FileContentResult(model.assetZip, System.Net.Mime.MediaTypeNames.Application.Zip) {
                FileDownloadName = model.Name + ".zip"
        };
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ChangeUserPass(string Id) {
            PersonModel pm = new PersonModel();
            pm.Id = Id;
            return View(pm);

        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult ChangeUserPass(PersonModel model) {
            model.Id = model.Id.Substring(0, model.Id.Length - 1);
            var user = UserManager.FindById(model.Id);
            UserManager.RemovePassword(user.Id);
            UserManager.AddPassword(user.Id, model.Password);
            return View("GetAllUsers", UserManager.Users.ToList());
        }

        [Authorize(Roles = "User")]
        public ActionResult Publisher() {
            PublisherModel model = new PublisherModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Publisher(PublisherModel model) {
            if (ModelState.IsValid)
            {
                //model.isActive = false;
                model.Id = User.Identity.GetUserId();
                _publisherContext.Publishers.Add(model);
                _publisherContext.SaveChanges();
            }
            else {
                return View(model);
            }
            return RedirectToAction("Index", "Assets");
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult PublisherApplications() {
            return View(_publisherContext.Publishers.ToList());
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult AcceptPublisher(string Id) {
            var model = _publisherContext.Publishers.FirstOrDefault(z => z.Id == Id);
            //model.isActive = true;
            var user = UserManager.FindById(model.Id);
            UserManager.AddToRole(user.Id, "Publisher");
            //UserManager.RemoveFromRole(user.Id, "User");
            _publisherContext.Publishers.Remove(model);
            _publisherContext.SaveChanges();
            return RedirectToAction("PublisherApplications");
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult DeclinePublisher(string Id)
        {
            var model = _publisherContext.Publishers.FirstOrDefault(z => z.Id == Id);
            _publisherContext.Publishers.Remove(model);
            _publisherContext.SaveChanges();
            return RedirectToAction("PublisherApplications");
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
           if (!ModelState.IsValid)
            {
                
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
            
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                    //throw new Exception();
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName=model.Username ,Email = model.Email};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    UserManager.AddToRole(user.Id, "User");
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Assets");
                }
                AddErrors(result);
            }
            
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                 string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                 var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                 await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                 return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Assets");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Assets");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}