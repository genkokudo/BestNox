using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BestNox.Models;

namespace BestNox.Controllers
{
    /// <summary>
    /// HOME画面：最初に表示する
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 最初のページ
        /// </summary>
        public IActionResult Index()
        {
            // 引数に何も指定しない場合は、メソッド名と同じ名前のビューを返す。
            return View();
        }

        /// <summary>
        /// プライバシーポリシー
        /// 詳細キボンヌボタンからリンクされている
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// エラーページ
        /// 応答キャッシュなし（特定ページのキャッシュを無効にする）
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
