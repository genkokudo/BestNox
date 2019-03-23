using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BestNox.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace BestNox
{
    public class Startup
    {
        // Core1系で毎回書いていたコードをラップしている
        // 内容はここ
        // https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/index?tabs=windows&view=aspnetcore-2.2#host
        // ・HTTP サーバーの実装
        // ・ミドルウェア コンポーネント
        // ・ログの記録
        // ・DI
        // ・構成

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        /// <summary>
        /// ランタイムから呼ばれるメソッド
        /// コンテナにサービスを追加する
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // appsettings.jsonから、使用するデータベースの接続文字列設定を取得
            // 初期状態ではSQL Serverを使用する設定だが、標準機能でSQLiteにすることもできる。
            // MySQLを使用するように変更
            // MySQL.Data.EntityFrameworkCoreはバグっているので
            // Pomelo.EntityFrameworkCore.MySqlを使用する。
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DebugConnection"),
                mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(10, 3, 13), ServerType.MariaDb);
                }
            ));

            //// ユーザ認証に使用するデータを指定？が2.2になって無くなっている
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            // RazorPagesを使用する設定
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// ランタイムから呼ばれるメソッド
        /// HTTPリクエストパイプラインの設定に使用する
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // 開発環境
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // 本番環境
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // HTTPをHTTPSにリダイレクトする
            app.UseHttpsRedirection();

            // 静的ファイルのルーティング設定
            // /wwwroot 配下のファイルに対して直接 URL アクセスが可能となる
            // /wwwroot/css/site.css というファイルに対しては http://..../css/site.css という URL でアクセスを行うことができる。
            app.UseStaticFiles();

            // cookieポリシーを使用する
            app.UseCookiePolicy();

            // ユーザ認証を行う
            app.UseAuthentication();

            // RazorPagesを使用する設定
            // ルーティング設定
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",    // ルート名
                    template: "{controller=Home}/{action=Index}/{id?}");    // URIパターン(デフォルト値付きで設定、defalts:パラメータは使用しない)
                // id?は任意に設定できるパラメータとなる
            });
        }
    }
}
