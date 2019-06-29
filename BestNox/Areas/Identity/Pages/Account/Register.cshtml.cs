using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BestNox.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BestNox.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "UserName")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{2} - {1} charactors", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = SystemConstants.ConfirmPasswordError)]
            public string ConfirmPassword { get; set; }

            [Display(Name = "IsAdministrator")]
            public bool IsAdministrator { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            var list = await _context.SystemParameters.Where(d => !d.IsDeleted && d.CategoryId == SystemConstants.SystemPatameterMode && d.OrderNo == SystemConstants.SystemPatameterModeRegisterAdmin).ToListAsync();
            if (list.Count == 0
                || list.Count > 0 && list[0].CurrentValue == "1")
            {
                // 管理者権限登録可能
                // ・初期状態の場合
                // ・対象レコードが無いとき
                // ・対象レコードの値が1のとき
                ViewData["IsAdminEnable"] = "1";
            }
            else
            {
                ViewData["IsAdminEnable"] = "0";
            }
            ReturnUrl = returnUrl;
        }

        /// <summary>
        /// ボタン押したときの処理
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.UserName };
                // この1行でユーザテーブルにデータ追加
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    // 管理者ならば、権限を付与する
                    if (Input.IsAdministrator)
                    {
                        // この1行で、ユーザ権限にデータ追加する
                        result = await _userManager.AddToRoleAsync(user, SystemConstants.Administrator);
                    }

                    _logger.LogInformation("パスワード付きのアカウントを作成しました。");

                    // サインイン
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            // ここに来たら何か失敗してるのでフォーム再表示すること。
            return Page();
        }
    }
}
