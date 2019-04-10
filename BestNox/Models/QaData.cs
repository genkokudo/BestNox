using EfCore.Shaman;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BestNox.Models
{
    /// <summary>
    /// Q&A形式のデータ
    /// オンラインメモ
    /// </summary>
    public class QaData : IEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        [Required, Column(TypeName = "text"), StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 問題
        /// </summary>
        [Required, Column(TypeName = "text")]
        public string Question { get; set; }

        /// <summary>
        /// 回答
        /// </summary>
        [Column(TypeName = "text")]
        public string Answer { get; set; }

        /// <summary>
        /// 解決済み
        /// </summary>
        [Required]
        public bool IsSolved { get; set; }

        /// <summary>
        /// 回答優先度
        /// 大きい方が優先
        /// </summary>
        [Required, Display(Name = "優先度")]
        public int RelativeNo { get; set; }

        /// <summary>
        /// 分類
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// 公開設定
        /// </summary>
        [Required]
        public int IsPublic { get; set; }


        #region 共通項目
        /// <summary>
        /// 登録者
        /// </summary>
        [Index, DisplayName("登録者"), StringLength(255)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [DisplayName("登録日時")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [DisplayName("更新者"), StringLength(255)]
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [DisplayName("更新日時")]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// 論理削除フラグ
        /// </summary>
        [DisplayName("論理削除")]
        public bool IsDeleted { get; set; }
        #endregion
    }
}
