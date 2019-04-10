using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BestNox.Models;
using EfCore.Shaman;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BestNox.Data
{
    // データモデルを追加したとき、このクラスも更新すること
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<QaData> QaDatas { get; set; }
        public DbSet<DailyRecord> DailyRecords { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<SystemParameter> SystemParameters { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {            
        }

        #region 標準項目設定
        public Task<int> SaveChangesAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            // 保存時に日時を設定する
            SetCreatedDateTime(name);

            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetCreatedDateTime(string name)
        {
            DateTime now = DateTime.Now;

            // 追加エンティティのうち、IEntity を実装したものを抽出
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity)
                .OfType<IEntity>();

            foreach (var entity in entities)
            {
                entity.UpdatedDate = now;
                entity.UpdatedBy = name;
                if (entity.CreatedDate is null)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = name;
                }
            }
        }
        #endregion

        /// <summary>
        /// EfCore shamanを使用する設定
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            this.FixOnModelCreating(builder);
        }
    }
}
