using System;

namespace BestNox.Models
{
    /// <summary>
    /// エラー画面のViewModel
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// リクエストID
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 画面にリクエストIDを表示するか
        /// 空の場合は表示しない
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}