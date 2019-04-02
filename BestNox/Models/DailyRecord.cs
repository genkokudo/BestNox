using EfCore.Shaman;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BestNox.Models
{
    /// <summary>
    /// 業務日報
    /// </summary>
    public class DailyRecord : IEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 記事の日付
        /// </summary>
        [Required]
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        [Required, Column(TypeName = "text"), StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 本文
        /// </summary>
        [Required, Column(TypeName = "text")]
        public string Detail { get; set; }

        #region 共通項目
        /// <summary>
        /// 登録者
        /// </summary>
        [Index]
        public int? CreatedBy { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        public int? UpdatedBy { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// 論理削除フラグ
        /// </summary>
        public bool IsDeleted { get; set; }
        #endregion
    }
}
