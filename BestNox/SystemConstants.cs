namespace BestNox
{
    /// <summary>
    /// システム設定
    /// </summary>
    public static class SystemConstants
    {
        /// <summary>
        /// 現在のDB接続設定
        /// 詳細はappsettings.json
        /// </summary>
        public const string Connection = "DebugConnection";

        /// <summary>
        /// ページタイトル
        /// </summary>
        public const string Title = "BestNox";

        /// <summary>
        /// 管理者権限名
        /// </summary>
        public const string Administrator = "Administrator";

        /// <summary>
        /// ファイル未選択
        /// </summary>
        public const string NoFileError = "ファイルが選択されていません";

        /// <summary>
        /// コメント未入力
        /// </summary>
        public const string NoComment = "コメントなし";

        /// <summary>
        /// 公開ファイルアップロード先
        /// </summary>
        public const string PublicUploadsDirectry = "wwwroot/" + PublicUploads;

        /// <summary>
        /// 公開ファイルアップロード先
        /// </summary>
        public const string PublicUploads = "PublicUploads";

        /// <summary>
        /// 非公開ファイルアップロード先
        /// </summary>
        public const string PrivateUploadsDirectry = "PrivateUploads";

        /// <summary>
        /// サイトアドレス
        /// </summary>
        public const string SiteUrl = "https://localhost:44307/";

        /// <summary>
        /// パスワードの確認誤りメッセージ
        /// </summary>
        public const string ConfirmPasswordError = "パスワードが一致していません";

        /// <summary>
        /// システムパラメータ：分類コード：分類
        /// </summary>
        public const int SystemPatameterCategory = 1;
        /// <summary>
        /// システムパラメータ：分類コード：オンラインメモ
        /// </summary>
        public const int SystemPatameterMemo = 2;
        /// <summary>
        /// システムパラメータ：分類コード：メモ一覧の背景色
        /// </summary>
        public const int SystemPatameterMemoBack = 3;
    }
}
