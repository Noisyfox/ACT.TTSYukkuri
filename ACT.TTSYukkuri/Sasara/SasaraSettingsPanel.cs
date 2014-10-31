namespace ACT.TTSYukkuri.Sasara
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Windows.Forms;

    using ACT.TTSYukkuri.Config;
    using CeVIO.Talk.RemoteService;

    /// <summary>
    /// ささらTTS設定Panel
    /// </summary>
    public partial class SasaraSettingsPanel : UserControl
    {
        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// シングルトンinstance
        /// </summary>
        private static SasaraSettingsPanel instance;

        /// <summary>
        /// ささらリモートインターフェースクラス
        /// </summary>
        private Talker talker;

        /// <summary>
        /// 感情componentテーブル
        /// </summary>
        private SasaraSettingsDataSet.SasaraComponentsDataTable componentsTable = new SasaraSettingsDataSet.SasaraComponentsDataTable();

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
        /// コンストラクタ
        /// </summary>
        public SasaraSettingsPanel()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void SasaraSettingsPanel_Load(object sender, EventArgs e)
        {
            this.talker = SasaraSpeechController.Talker;

            // キャストコンボボックスを設定する
            var casts = Talker.AvailableCasts;
            this.castComboBox.Items.AddRange(casts);
            this.castComboBox.TextChanged += (s1, e1) =>
            {
                this.talker.Cast = this.castComboBox.Text;

                var components = this.talker.Components;
                for (int i = 0; i < components.Count; i++)
                {
                    var c = components[i];

                    if (!this.componentsTable.Any(x => x.Id == c.Id))
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

            // 設定をロードする
            this.onryoTextBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Onryo.ToString();
            this.hayasaTextBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Hayasa.ToString();
            this.takasaTextBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Takasa.ToString();
            this.seishitsuTextBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Seishitsu.ToString();
            this.componentsTable = TTSYukkuriConfig.Default.SasaraSettings.Components;
            this.castComboBox.Text = TTSYukkuriConfig.Default.SasaraSettings.Cast;

            // ささらを設定する
            this.SetSasara();
        }

        /// <summary>
        /// 感情GridView DataError
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void kanjoDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageBox.Show(
                    this,
                    "0～100の値を入力して下さい");
            }
        }

        /// <summary>
        /// 感情GridView CellValidated
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private void kanjoDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
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

        /// <summary>
        /// 設定を保存する
        /// </summary>
        private void SaveSettings()
        {
            TTSYukkuriConfig.Default.SasaraSettings.Onryo = uint.Parse(this.onryoTextBox.Text);
            TTSYukkuriConfig.Default.SasaraSettings.Hayasa = uint.Parse(this.hayasaTextBox.Text);
            TTSYukkuriConfig.Default.SasaraSettings.Takasa = uint.Parse(this.takasaTextBox.Text);
            TTSYukkuriConfig.Default.SasaraSettings.Seishitsu = uint.Parse(this.seishitsuTextBox.Text);

            TTSYukkuriConfig.Default.SasaraSettings.Cast = this.castComboBox.Text;
            TTSYukkuriConfig.Default.SasaraSettings.Components = this.componentsTable;

            // 設定を保存する
            TTSYukkuriConfig.Default.Save();

            // ささらを設定する
            this.SetSasara();
        }

        /// <summary>
        /// ささらを設定する
        /// </summary>
        private void SetSasara()
        {
            this.talker.Cast = TTSYukkuriConfig.Default.SasaraSettings.Cast;
            this.talker.Volume = TTSYukkuriConfig.Default.SasaraSettings.Onryo;
            this.talker.Speed = TTSYukkuriConfig.Default.SasaraSettings.Hayasa;
            this.talker.Tone = TTSYukkuriConfig.Default.SasaraSettings.Takasa;
            this.talker.Alpha = TTSYukkuriConfig.Default.SasaraSettings.Seishitsu;

            foreach (var componet in TTSYukkuriConfig.Default.SasaraSettings.Components)
            {
                var componetOnSasara = this.talker.Components
                    .Where(x => x.Id == componet.Id).FirstOrDefault();

                if (componetOnSasara != null)
                {
                    componetOnSasara.Value = componet.Value;
                }
            }
        }
    }
}
