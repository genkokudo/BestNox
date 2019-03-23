using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BestNox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Core1系で毎回書いていたコードをラップしている
            // 内容はここ
            // https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/index?tabs=windows&view=aspnetcore-2.2#host
            // ・HTTP サーバーの実装
            // ・ミドルウェア コンポーネント
            // ・ログの記録
            // ・DI
            // ・構成
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
