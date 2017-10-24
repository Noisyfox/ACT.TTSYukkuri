using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ACT.TTSYukkuri.Config.ViewModels;
using ACT.TTSYukkuri.resources;
using ACT.TTSYukkuri.Voiceroid;
using Advanced_Combat_Tracker;
using FFXIV.Framework.Common;

namespace ACT.TTSYukkuri.Config.Views
{
    /// <summary>
    /// GeneralView.xaml の相互作用ロジック
    /// </summary>
    public partial class GeneralView : UserControl, ILocalizable
    {
        public GeneralView()
        {
            this.InitializeComponent();
            this.DataContext = new GeneralViewModel();

            this.SetLocale(TTSYukkuriConfig.Default.UILocale);

            this.TTSTypesComboBox.SelectionChanged += this.TTSTypeOnSelectionChanged;
            this.Loaded += this.OnLoaded;
        }

        public GeneralViewModel ViewModel => this.DataContext as GeneralViewModel;

        public void SetLocale(Locales locale) => this.ReloadLocaleDictionary(locale);

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var ttsType = string.Empty;
#if DEBUG
            if (WPFHelper.IsDesignMode)
            {
                ttsType = TTSType.Yukkuri;
            }
            else
            {
                ttsType = (string)(this.TTSTypesComboBox.SelectedValue);
            }
#else
            ttsType = (string)(this.TTSTypesComboBox.SelectedValue);
#endif
            this.LoadTTSConfigPage(ttsType);
        }

        private void TTSTypeOnSelectionChanged(
            object sender,
            RoutedEventArgs e)
        {
            var ttsType = (string)((sender as ComboBox).SelectedValue);
            this.LoadTTSConfigPage(ttsType);
        }

        private async void LoadTTSConfigPage(
            string ttsType)
        {
            var content = default(object);
            switch (ttsType)
            {
                case TTSType.Yukkuri:
                    content = new YukkuriConfigView();
                    break;

                case TTSType.HOYA:
                    content = new HoyaConfigView();
                    break;

                case TTSType.OpenJTalk:
                    content = new OpenJTalkConfigView();
                    break;

                case TTSType.Sasara:
                    try
                    {
                        // リモートからささらの設定を取得する
                        await Task.Run(() =>
                        {
                            TTSYukkuriConfig.Default.SasaraSettings.LoadRemoteConfig();
                        });
                    }
                    catch (Exception ex)
                    {
                        var message = new StringBuilder();
                        message.AppendLine("CeVIO Creative Studio との接続で例外が発生しました。");
                        message.AppendLine();
                        message.AppendLine(ex.ToString());

                        System.Windows.Forms.MessageBox.Show(
                            ActGlobals.oFormActMain,
                            message.ToString(),
                            "TTSゆっくりプラグイン",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Exclamation);

                        return;
                    }

                    content = new SasaraConfigView();
                    break;

                case TTSType.Boyomichan:
                    content = new BoyomiConfigView();
                    break;

                case TTSType.VOICEROID:
                    try
                    {
                        var ctrl = SpeechController.Default as VoiceroidSpeechController;
                        TTSYukkuriConfig.Default.VoiceroidSettings.Load();
                        var err = await ctrl?.Start();

                        if (!string.IsNullOrEmpty(err))
                        {
                            var message = new StringBuilder();
                            message.AppendLine("VOICEROIDの初期化でエラーが発生しました。");
                            message.AppendLine();
                            message.AppendLine(err);

                            System.Windows.Forms.MessageBox.Show(
                                ActGlobals.oFormActMain,
                                message.ToString(),
                                "TTSゆっくりプラグイン",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Exclamation);

                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        var message = new StringBuilder();
                        message.AppendLine("VOICEROIDの初期化で例外が発生しました。");
                        message.AppendLine();
                        message.AppendLine(ex.ToString());

                        System.Windows.Forms.MessageBox.Show(
                            ActGlobals.oFormActMain,
                            message.ToString(),
                            "TTSゆっくりプラグイン",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Exclamation);

                        return;
                    }

                    content = new VoiceroidConfigView();
                    break;

                default:
                    return;
            }

            this.ContentGrid.Children.Clear();
            this.ContentGrid.Children.Add((UIElement)content);
        }
    }
}
