using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BestNox.Models;
using EfCore.Shaman;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        // 保存時の処理
        public override int SaveChanges()
        {
            // 保存時に日時を設定する
            SetCreatedDateTime();

            return base.SaveChanges();
        }

        private void SetCreatedDateTime()
        {
            DateTime now = DateTime.Now;

            // 追加エンティティのうち、IEntity を実装したものを抽出
            var entities = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .OfType<IEntity>();

            foreach (var entity in entities)
            {
                entity.UpdatedDate = now;
                if (entity.CreatedDate is null)
                {
                    entity.CreatedDate = now;
                }
                // TODO:ユーザIDも保存すること
            }
        }

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
