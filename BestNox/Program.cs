using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BestNox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Webホスト作成
            var host = CreateWebHostBuilder(args).Build();

            // 権限データがなければ作成する
            CreateRole(host);

            host.Run();
        }

        #region 雑な権限作成
        /// <summary>
        /// 権限データが無ければ作成する
        /// </summary>
        /// <param name="host">WebのHost（必要なサービス取得に使用）</param>
        private static void CreateRole(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                // 権限管理を取得
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var role = SystemConstants.Administrator; // 管理者権限

                try
                {
                    Task<IdentityResult> roleResult;
                    // 管理者権限があるかチェック
                    Task<bool> hasRole = roleManager.RoleExistsAsync(role);
                    hasRole.Wait();

                    if (!hasRole.Result)
                    {
                        // 無ければ追加
                        roleResult = roleManager.CreateAsync(new IdentityRole(role));
                        roleResult.Wait();
                    }
                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "権限作成に失敗しました。");
                }
            }
        }
        #endregion

        // Core1系で毎回書いていたコードをラップしている
        // 内容はここ
        // https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/index?tabs=windows&view=aspnetcore-2.2#host
        // ・HTTP サーバーの実装
        // ・ミドルウェア コンポーネント
        // ・ログの記録
        // ・DI
        // ・構成
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
