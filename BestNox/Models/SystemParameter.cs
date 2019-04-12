using Microsoft.AspNetCore.Identity;
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
    /// システムパラメータ
    /// </summary>
    public class SystemParameter : IEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分類ID
        /// </summary>
        [Required, DisplayName("分類ID")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 選択肢の表示順序
        /// </summary>
        [Required, DisplayName("選択肢の表示順序（若い順）")]
        public int OrderNo { get; set; }

        /// <summary>
        /// 画面表示
        /// </summary>
        [Required, DisplayName("画面表示内容"), Column(TypeName = "text")]
        public string Display { get; set; }

        /// <summary>
        /// 設定値
        /// </summary>
        [Required, DisplayName("設定値（コード）"), Column(TypeName = "text")]
        public string CurrentValue { get; set; }

        #region 共通項目
        /// <summary>
        /// 登録者
        /// </summary>
        [DisplayName("登録者"), StringLength(255)]
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
