namespace ACT.TTSYukkuri
{
    using System.Collections.Generic;
    using System.Linq;

    using ACT.TTSYukkuri.Config;

    /// <summary>
    /// FF14を監視する パーティメンバ監視部分
    /// </summary>
    public partial class FF14Watcher
    {
        /// <summary>
        /// 直前のパーティメンバ情報
        /// </summary>
        private List<PreviousPartyMemberStatus> previouseParyMemberList = new List<PreviousPartyMemberStatus>();

        /// <summary>
        /// パーティを監視する
        /// </summary>
        public void WatchParty()
        {
            // パーティメンバの情報を取得する
            var player = this.GetPlayer();
            var partyList = this.GetCombatantListParty();

            // パーティが自分だけになったら直前のリストをクリアする
            if (partyList.Count < 2)
            {
                this.previouseParyMemberList.Clear();
            }

            foreach (var partyMember in partyList)
            {
                // 自分を除外する
                if (partyMember.ID == player.ID)
                {
                    continue;
                }

                // このPTメンバの現在の状態を取得する
                var hpRate =
                    partyMember.MaxHP != 0 ?
                    ((decimal)partyMember.CurrentHP / (decimal)partyMember.MaxHP) * 100m :
                    0m;

                var mpRate =
                    partyMember.MaxMP != 0 ?
                    ((decimal)partyMember.CurrentMP / (decimal)partyMember.MaxMP) * 100m :
                    0m;

                var tpRate = ((decimal)partyMember.CurrentTP / 1000m) * 100m;

                // このPTメンバの直前の情報を取得する
                var previousePartyMember = (
                    from x in this.previouseParyMemberList
                    where
                    x.ID == partyMember.ID
                    select
                    x).FirstOrDefault();

                if (previousePartyMember == null)
                {
                    previousePartyMember = new PreviousPartyMemberStatus()
                    {
                        ID = partyMember.ID,
                        Name = partyMember.Name,
                        HPRate = hpRate,
                        MPRate = mpRate,
                        TPRate = tpRate
                    };

                    this.previouseParyMemberList.Add(previousePartyMember);
                }

                // 読上げ用の名前「ジョブ名＋イニシャル」とする
                var nameToSpeak =
                    GetJobNameToSpeak(partyMember.Job) +
                    partyMember.Name.Trim().Substring(0, 1);

                // HPをチェックして読上げる
                if (TTSYukkuriConfig.Default.OptionSettings.EnabledHPWatch)
                {
                    if (hpRate <= (decimal)TTSYukkuriConfig.Default.OptionSettings.HPThreshold &&
                        previousePartyMember.HPRate > (decimal)TTSYukkuriConfig.Default.OptionSettings.HPThreshold)
                    {
                        this.Speak(
                            nameToSpeak +
                            "、えいちぴー" +
                            decimal.ToInt32(hpRate).ToString() +
                            "%。");
                    }
                    else
                    {
                        if (hpRate <= decimal.Zero && previousePartyMember.HPRate != decimal.Zero)
                        {
                            this.Speak(
                                nameToSpeak +
                                "、せんとうふのう。");
                        }
                    }
                }

                // MPをチェックして読上げる
                if (TTSYukkuriConfig.Default.OptionSettings.EnabledMPWatch)
                {
                    if (mpRate <= (decimal)TTSYukkuriConfig.Default.OptionSettings.MPThreshold &&
                        previousePartyMember.MPRate > (decimal)TTSYukkuriConfig.Default.OptionSettings.MPThreshold)
                    {
                        this.Speak(
                            nameToSpeak +
                            "、えむぴー" +
                            decimal.ToInt32(mpRate).ToString() +
                            "%。");
                    }
                    else
                    {
                        if (mpRate <= decimal.Zero && previousePartyMember.MPRate != decimal.Zero)
                        {
                            this.Speak(
                                nameToSpeak +
                                "、えむぴーなし。");
                        }
                    }
                }

                // TPをチェックして読上げる
                if (TTSYukkuriConfig.Default.OptionSettings.EnabledTPWatch)
                {
                    if (tpRate <= (decimal)TTSYukkuriConfig.Default.OptionSettings.TPThreshold &&
                        previousePartyMember.TPRate > (decimal)TTSYukkuriConfig.Default.OptionSettings.TPThreshold)
                    {
                        this.Speak(
                            nameToSpeak +
                            "、てぃぴー" +
                            decimal.ToInt32(tpRate).ToString() +
                            "%");
                    }
                    else
                    {
                        if (tpRate <= decimal.Zero && previousePartyMember.TPRate != decimal.Zero)
                        {
                            this.Speak(
                                nameToSpeak +
                                "、てぃぴーなし。");
                        }
                    }
                }

                // 今回の状態を保存する
                previousePartyMember.HPRate = hpRate;
                previousePartyMember.MPRate = mpRate;
                previousePartyMember.TPRate = tpRate;
            }
        }

        /// <summary>
        /// 直前のPTメンバステータス
        /// </summary>
        private class PreviousPartyMemberStatus
        {
            /// <summary>
            /// ID
            /// </summary>
            public uint ID { get; set; }

            /// <summary>
            /// 名前
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// HP率
            /// </summary>
            public decimal HPRate { get; set; }

            /// <summary>
            /// MP率
            /// </summary>
            public decimal MPRate { get; set; }

            /// <summary>
            /// TP率
            /// </summary>
            public decimal TPRate { get; set; }
        }
    }
}
