using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BestNox.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.Extensions.Logging;
using BestNox.Settings;

namespace BestNox
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        private ILogger Logger { get; }

        // Core1系で毎回書いていたコードをラップしている
        // 内容はここ
        // https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/index?tabs=windows&view=aspnetcore-2.2#host
        // ・HTTP サーバーの実装
        // ・ミドルウェア コンポーネント
        // ・ログの記録
        // ・DI
        // ・構成
        public Startup(IHostingEnvironment env, IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;

            //構成ファイル、環境変数等から、構成情報をロード
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            //構成情報をプロパティに設定
            Configuration = builder.Build();
            Environment = env;
        }


        /// <summary>
        /// ランタイムから呼ばれるメソッド
        /// コンテナにサービスを追加する
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // このラムダは、必須ではないCookieに対するユーザーの同意が特定のリクエストに必要かどうかを決定します。
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // appsettings.jsonから、使用するデータベースの接続文字列設定を取得
            // 初期状態ではSQL Serverを使用する設定だが、標準機能でSQLiteにすることもできる。
            // MySQLを使用するように変更
            // MySQL.Data.EntityFrameworkCoreはバグっているので
            // Pomelo.EntityFrameworkCore.MySqlを使用する。
            if (Environment.EnvironmentName == SystemConstants.EnvDevelopment)
            {
                // 開発系はappsettingsから接続文字列とパスワードを取得する。
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString(SystemConstants.Connection),
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(10, 3, 13), ServerType.MariaDb);
                    }
                ));
            }
            else
            {
                // 本番系は接続先をappsettingsから、パスワードを環境変数から取得する
                // マイグレーションを行う場合、環境名は"Development"になり、環境変数から値が取れないのでここは使えない。
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString(SystemConstants.Connection) + "Password=" + Configuration.GetValue<string>(SystemConstants.DbPasswordEnv) + ";",
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(10, 3, 13), ServerType.MariaDb);
                    }
                ));
            }

            // デフォルトUI
            // UI画面を自作しない場合、この設定でデフォルトのRegisterページUIが設定される
            // ユーザ認証に使用するデータを指定
            // 権限を使用するように設定
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI();

            // RazorPagesを使用する設定
            // RazorPagesの設定なので、Pagesフォルダじゃないと適用されない。
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // アプリケーション設定の読み込み
            var defaultParameter = services.Configure<DefaultParameters>(this.Configuration.GetSection(SystemConstants.DefaultParameters));
        }

        /// <summary>
        /// ランタイムから呼ばれるメソッド
        /// HTTPリクエストパイプラインの設定に使用する
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="services">設定ファイルの読み込み</param>
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

            // cookieポリシーを使用する
            // これをUseMvc()より前に書くと、クライアントに提供するCookieが渡されないのでセッションが維持できない。
            app.UseCookiePolicy();
        }
    }

}