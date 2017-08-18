﻿namespace ACT.TTSYukkuri.Sasara
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using FFXIV.Framework.TTS.Common;
    using FFXIV.Framework.TTS.Common.Models;

    /// <summary>
    /// ささらTTS設定Panel
    /// </summary>
    public partial class SasaraSettingsPanel : UserControl
    {
        /// <summary>
        /// シングルトンinstance
        /// </summary>
        private static SasaraSettingsPanel instance;

        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// 感情componentテーブル
        /// </summary>
        private SasaraSettingsDataSet.SasaraComponentsDataTable componentsTable =
            new SasaraSettingsDataSet.SasaraComponentsDataTable();

        /// <summary>
        /// さららTalker設定
        /// </summary>
        private CevioTalkerModel talker;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SasaraSettingsPanel()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// シングルトンinstance
        /// </summary>
        public static SasaraSettingsPanel Default
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new SasaraSettingsPanel();
                    }

                    return instance;
                }
            }
        }

        /// <summary>
        /// 感情GridView CellValidated
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void KanjoDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            foreach (var row in this.componentsTable)
            {
                if (row.Value < 0)
                {
                    row.Value = 0;
                }

                if (row.Value > 100)
                {
                    row.Value = 100;
                }
            }

            // 設定を保存する
            this.SaveSettings();
        }

        /// <summary>
        /// 感情GridView DataError
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void KanjoDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageBox.Show(
                    this,
                    "0～100の値を入力して下さい");
            }
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void SasaraSettingsPanel_Load(object sender, EventArgs e)
        {
            this.talker = RemoteTTSClient.Instance.TTSModel.GetCevioTalker();

            // キャストコンボボックスを設定する
            var casts = this.talker.AvailableCasts;
            this.castComboBox.Items.AddRange(casts);
            this.castComboBox.TextChanged += (s1, e1) =>
            {
                this.talker.Cast = this.castComboBox.Text;

                RemoteTTSClient.Instance.TTSModel.SetCevioTalker(this.talker);

                this.talker = RemoteTTSClient.Instance.TTSModel.GetCevioTalker();

                var components = this.talker.Components;
                for (int i = 0; i < components.Count; i++)
                {
                    var c = components[i];

                    var component = this.componentsTable
                        .Where(x => x.Id == c.Id)
                        .FirstOrDefault();

                    if (component == null)
                    {
                        this.componentsTable.AddSasaraComponentsRow(
                            c.Id,
                            c.Name,
                            c.Value,
                            this.talker.Cast);
                    }
                }

                this.kanjoDataGridView.AutoGenerateColumns = false;
                this.kanjoDataGridView.DataSource = componentsTable
                    .Where(x => x.Cast == this.talker.Cast)
                    .ToList();

                // 設定を保存する
                this.SaveSettings();
            };

            // 音量関係のテキストボックスを設定する
            this.onryoTextBox.Leave += (s1, e1) =>
            {
                this.ValidateParameter(s1 as TextBox);
            };

            this.hayasaTextBox.Leave += (s1, e1) =>
            {
                this.ValidateParameter(s1 as TextBox);
            };

            this.takasaTextBox.Leave += (s1, e1) =>
            {
                this.ValidateParameter(s1 as TextBox);
            };

            this.seishitsuTextBox.Leave += (s1, e1) =>
            {
                this.ValidateParameter(s1 as TextBox);
            };

            this.yokuyoTextBox.Leave += (s1, e1) =>
            {
                this.ValidateParameter(s1 as TextBox);
            };

            // 設定をロードする
            this.onryoTextBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Onryo.ToString();
            this.hayasaTextBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Hayasa.ToString();
            this.takasaTextBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Takasa.ToString();
            this.seishitsuTextBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Seishitsu.ToString();
            this.yokuyoTextBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Yokuyo.ToString();
            this.componentsTable = TTSYukkuriConfig.Default.SasaraSettings.Components;
            this.castComboBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Cast;

            // ささらを設定する
            TTSYukkuriConfig.Default.SetSasara();
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        private void SaveSettings()
        {
            TTSYukkuriConfig.Default.SasaraSettings.Onryo = uint.Parse(this.onryoTextBox.Text);
            TTSYukkuriConfig.Default.SasaraSettings.Hayasa = uint.Parse(this.hayasaTextBox.Text);
            TTSYukkuriConfig.Default.SasaraSettings.Takasa = uint.Parse(this.takasaTextBox.Text);
            TTSYukkuriConfig.Default.SasaraSettings.Seishitsu = uint.Parse(this.seishitsuTextBox.Text);
            TTSYukkuriConfig.Default.SasaraSettings.Yokuyo = uint.Parse(this.yokuyoTextBox.Text);

            TTSYukkuriConfig.Default.SasaraSettings.Cast = this.castComboBox.Text;
            TTSYukkuriConfig.Default.SasaraSettings.Components = this.componentsTable;

            // 設定を保存する
            TTSYukkuriConfig.Default.Save();

            // ささらを設定する
            TTSYukkuriConfig.Default.SetSasara();
        }

        /// <summary>
        /// Parameterを検証する
        /// </summary>
        /// <param name="parameterTextBox">検証対象のParameterテキストボックス</param>
        private void ValidateParameter(
            TextBox parameterTextBox)
        {
            uint i;
            if (uint.TryParse(parameterTextBox.Text, out i))
            {
                if (i < 0)
                {
                    parameterTextBox.Text = "0";
                }

                if (i > 100)
                {
                    parameterTextBox.Text = "100";
                }
            }
            else
            {
                parameterTextBox.Text = "0";
            }

            // 設定を保存する
            this.SaveSettings();
        }
    }
}
