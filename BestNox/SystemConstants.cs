namespace BestNox
{
    /// <summary>
    /// システム設定
    /// </summary>
    public static class SystemConstants
    {
        // 環境
        /// <summary>
        /// 環境：開発系
        /// </summary>
        public const string EnvDevelopment = "Development";
        /// <summary>
        /// 環境：本番系
        /// </summary>
        public const string EnvProduction = "Production";

        // 設定名
        /// <summary>
        /// 現在のDB接続設定
        /// 詳細はappsettings.json
        /// </summary>
        public const string Connection = "DefaultConnection";
        /// <summary>
        /// デモサイト用のDB接続設定
        /// </summary>
        public const string ConnectionDemo = "DemoConnection";
        /// <summary>
        /// サブディレクトリに配置するときのパス
        /// </summary>
        public const string PathBase = "PathBase";
        /// <summary>
        /// サブディレクトリに配置するときのパス
        /// </summary>
        public const string PathBaseDemo = "PathBaseDemo";
        /// <summary>
        /// 使用するポート番号
        /// </summary>
        public const string Port = "Port";

        // 環境変数名
        /// <summary>
        /// DBパスワード環境変数名
        /// </summary>
        public const string DbPasswordEnv = "DATABASE_PASSWORD";
        /// <summary>
        /// 展示用モードかの環境変数名
        /// </summary>
        public const string IsDemoEnv = "IS_DEMO";


        /// <summary>
        /// ページタイトル
        /// </summary>
        public const string Title = "BestNox";

        /// <summary>
        /// 管理者権限名
        /// </summary>
        public const string Administrator = "Administrator";
        /// <summary>
        /// ゲスト権限名
        /// </summary>
        public const string Guest = "Guest";

        /// <summary>
        /// ファイル未選択
        /// </summary>
        public const string NoFileError = "ファイルが選択されていません";

        /// <summary>
        /// パスワードの確認誤りメッセージ
        /// </summary>
        public const string ConfirmPasswordError = "パスワードが一致していません";

        /// <summary>
        /// コメント未入力
        /// </summary>
        public const string NoComment = "コメントなし";

        /// <summary>
        /// 非公開ファイルアップロード先
        /// </summary>
        public const string PrivateUploadsDirectry = "PrivateUploads";
        /// <summary>
        /// 公開ファイルアップロード先
        /// </summary>
        public const string PublicUploads = "PublicUploads";
        /// <summary>
        /// 公開ファイルアップロード先
        /// </summary>
        public const string PublicUploadsDirectry = "wwwroot/" + PublicUploads;

        /// <summary>
        /// アプリケーション設定：システムパラメータ初期値
        /// </summary>
        public const string DefaultParameters = "DefaultParameters";
        /// <summary>
        /// 初期値設定ユーザ名
        /// </summary>
        public const string DefaultParameterUserName = "default";
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
