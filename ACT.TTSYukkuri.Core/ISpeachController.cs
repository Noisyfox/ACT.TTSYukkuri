using System;
using System.IO;
using System.Linq;
using System.Threading;
using FFXIV.Framework.Common;
using NLog;

namespace ACT.TTSYukkuri
{
    /// <summary>
    /// スピーチコントローラの汎用インターフェース
    /// </summary>
    public interface ISpeechController
    {
        /// <summary>
        /// TTSを初期化する
        /// </summary>
        void Initialize();

        /// <summary>
        /// TTSを開放する
        /// </summary>
        void Free();

        /// <summary>
        /// TTSに話してもらう
        /// </summary>
        /// <param name="text">読上げるテキスト</param>
        void Speak(string text, PlayDevices playDevice = PlayDevices.Both);
    }

    public static class SpeechControllerExtentions
    {
        #region Logger

        private static Logger Logger => AppLog.DefaultLogger;

        #endregion Logger

        public static string CacheDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            @"anoyetta\ACT\tts cache");

        public static Logger GetLogger(
            this ISpeechController ctrl)
            => Logger;

        public static void SpeakWithDelay(
            this ISpeechController speechController,
            string text,
            int delay,
            PlayDevices playDevice = PlayDevices.Both)
        {
            if (delay == 0)
            {
                speechController.Speak(text, playDevice);
                return;
            }

            var timer = new Timer(new TimerCallback((state) =>
            {
                speechController.Speak(text, playDevice);
                (state as Timer).Dispose();
            }));

            timer.Change(
                delay * 1000,
                0);
        }

        private static readonly char[] InvalidCars = Path.GetInvalidFileNameChars();

        /// <summary>
        /// キャッシュファイル用の衝突しない名前を生成する
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="ttsType"></param>
        /// <param name="tts"></param>
        /// <param name="parameter"></param>
        /// <returns>
        /// キャッシュファイル名</returns>
        public static string GetCacheFileName(
            this ISpeechController ctrl,
            string ttsType,
            string tts,
            string parameter)
        {
            var hash = parameter.GetHashCode().ToString("X4");
            var cacheName = $"{ttsType}.{tts}.{hash}.wav";

            // ファイル名に使用できない文字を除去する
            cacheName = string.Concat(cacheName.Where(c => !InvalidCars.Contains(c)));

            var fileName = Path.Combine(
                CacheDirectory,
                cacheName);

            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }

            return fileName;
        }
    }
}
