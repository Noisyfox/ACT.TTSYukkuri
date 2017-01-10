namespace ACT.TTSYukkuri.TTSServer.Core
{
    using System.ServiceModel;

    using ACT.TTSYukkuri.TTSServer.Core.Models;

    [ServiceContract]
    public interface ITTSServerContract
    {
        /// <summary>
        /// 準備完了か？
        /// </summary>
        /// <returns>boolean</returns>
        [OperationContract]
        bool IsReady();

        /// <summary>
        /// 引数で指定された音声化データに従ってwaveファイルを生成する
        /// </summary>
        /// <param name="speakModel">
        /// 音声化データ</param>
        [OperationContract]
        void Speak(Speak speakModel);

        /// <summary>
        /// ささら(Cevio Creative Studio)の設定を取得する
        /// </summary>
        /// <returns>
        /// ささらの設定</returns>
        [OperationContract]
        SasaraSettings GetSasaraSettings();

        /// <summary>
        /// ささら(Cevio Creative Studio)を設定する
        /// </summary>
        /// <param name="settings">
        /// ささらの設定</param>
        [OperationContract]
        void SetSasaraSettings(SasaraSettings settings);

        /// <summary>
        /// ささら(Cevio Creative Studio)を開始する
        /// </summary>
        [OperationContract]
        void StartSasara();

        /// <summary>
        /// ささら(Cevio Creative Studio)を終了する
        /// </summary>
        [OperationContract]
        void EndSasara();

        /// <summary>
        /// サーバを終了する
        /// </summary>
        [OperationContract]
        void End();
    }
}
