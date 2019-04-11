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
    /// アップロードファイルのデータ
    /// </summary>
    public class UploadFile : IEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// コメント
        /// </summary>
        [DisplayName("コメント"), StringLength(200)]
        public string Comment { get; set; }

        /// <summary>
        /// 元ファイル名
        /// </summary>
        [DisplayName("ファイル名"), StringLength(60)]
        public string Filename { get; set; }

        /// <summary>
        /// ファイルサイズ(MB)
        /// </summary>
        [Required, DisplayName("サイズ")]
        public float Size { get; set; }

        /// <summary>
        /// 格納ファイル名
        /// </summary>
        [DisplayName("直接参照用アドレス"), StringLength(50)]
        public string TmpFilename { get; set; }

        /// <summary>
        /// 公開設定
        /// </summary>
        [Required, DisplayName("公開設定")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// 変更前の公開設定
        /// </summary>
        [NotMapped]
        public bool IsPublicOld { get; set; }

        /// <summary>
        /// コンテントタイプ
        /// </summary>
        [StringLength(50)]
        public string ContentType { get; set; }

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
