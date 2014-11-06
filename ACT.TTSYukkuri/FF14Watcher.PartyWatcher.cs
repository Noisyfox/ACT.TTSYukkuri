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

            // パーティリストに存在しないメンバの前回の状態を消去する
            foreach (var previouseMember in previouseParyMemberList)
            {
                if (!partyList.Any(x => x.ID == previouseMember.ID))
                {
                    this.previouseParyMemberList.Remove(previouseMember);
                }
            }

            foreach (var partyMember in partyList)
            {
                if (!TTSYukkuriConfig.Default.OptionSettings.EnableSelf)
                {
                    // 自分を除外する
                    if (partyMember.ID == player.ID)
                    {
                        continue;
                    }
                }

                // このPTメンバの現在の状態を取得する
                var hp = partyMember.CurrentHP;
                var hpp =
                    partyMember.MaxHP != 0 ?
                    ((decimal)partyMember.CurrentHP / (decimal)partyMember.MaxHP) * 100m :
                    0m;

                var mp = partyMember.CurrentMP;
                var mpp =
                    partyMember.MaxMP != 0 ?
                    ((decimal)partyMember.CurrentMP / (decimal)partyMember.MaxMP) * 100m :
                    0m;

                var tp = partyMember.CurrentTP;
                var tpp = ((decimal)partyMember.CurrentTP / 1000m) * 100m;

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
                        HPRate = hpp,
                        MPRate = mpp,
                        TPRate = tpp
                    };

                    this.previouseParyMemberList.Add(previousePartyMember);
                }

                // 読上げ用の名前「ジョブ名＋イニシャル」とする
                var pcname =
                    GetJobNameToSpeak(partyMember.Job) +
                    partyMember.Name.Trim().Substring(0, 1);

                // 読上げ用のテキストを編集する
                var hpTextToSpeak = TTSYukkuriConfig.Default.OptionSettings.HPTextToSpeack;
                var mpTextToSpeak = TTSYukkuriConfig.Default.OptionSettings.MPTextToSpeack;
                var tpTextToSpeak = TTSYukkuriConfig.Default.OptionSettings.TPTextToSpeack;

                hpTextToSpeak = hpTextToSpeak.Replace("<pcname>", pcname);
                hpTextToSpeak = hpTextToSpeak.Replace("<hp>", hp.ToString());
                hpTextToSpeak = hpTextToSpeak.Replace("<hpp>", decimal.ToInt32(hpp).ToString());
                hpTextToSpeak = hpTextToSpeak.Replace("<mp>", mp.ToString());
                hpTextToSpeak = hpTextToSpeak.Replace("<mpp>", decimal.ToInt32(mpp).ToString());
                hpTextToSpeak = hpTextToSpeak.Replace("<tp>", tp.ToString());
                hpTextToSpeak = hpTextToSpeak.Replace("<tpp>", decimal.ToInt32(tpp).ToString());

                mpTextToSpeak = mpTextToSpeak.Replace("<pcname>", pcname);
                mpTextToSpeak = mpTextToSpeak.Replace("<hp>", hp.ToString());
                mpTextToSpeak = mpTextToSpeak.Replace("<hpp>", decimal.ToInt32(hpp).ToString());
                mpTextToSpeak = mpTextToSpeak.Replace("<mp>", mp.ToString());
                mpTextToSpeak = mpTextToSpeak.Replace("<mpp>", decimal.ToInt32(mpp).ToString());
                mpTextToSpeak = mpTextToSpeak.Replace("<tp>", tp.ToString());
                mpTextToSpeak = mpTextToSpeak.Replace("<tpp>", decimal.ToInt32(tpp).ToString());

                tpTextToSpeak = tpTextToSpeak.Replace("<pcname>", pcname);
                tpTextToSpeak = tpTextToSpeak.Replace("<hp>", hp.ToString());
                tpTextToSpeak = tpTextToSpeak.Replace("<hpp>", decimal.ToInt32(hpp).ToString());
                tpTextToSpeak = tpTextToSpeak.Replace("<mp>", mp.ToString());
                tpTextToSpeak = tpTextToSpeak.Replace("<mpp>", decimal.ToInt32(mpp).ToString());
                tpTextToSpeak = tpTextToSpeak.Replace("<tp>", tp.ToString());
                tpTextToSpeak = tpTextToSpeak.Replace("<tpp>", decimal.ToInt32(tpp).ToString());

                // HPをチェックして読上げる
                if (TTSYukkuriConfig.Default.OptionSettings.EnabledHPWatch &&
                    !string.IsNullOrWhiteSpace(hpTextToSpeak))
                {
                    if (hpp <= (decimal)TTSYukkuriConfig.Default.OptionSettings.HPThreshold &&
                        previousePartyMember.HPRate > (decimal)TTSYukkuriConfig.Default.OptionSettings.HPThreshold)
                    {
                        this.Speak(hpTextToSpeak);
                    }
                    else
                    {
                        if (hpp <= decimal.Zero && previousePartyMember.HPRate != decimal.Zero)
                        {
                            this.Speak(pcname + "、せんとうふのう。");
                        }
                    }
                }

                // MPをチェックして読上げる
                if (TTSYukkuriConfig.Default.OptionSettings.EnabledMPWatch &&
                    !string.IsNullOrWhiteSpace(mpTextToSpeak))
                {
                    if (mpp <= (decimal)TTSYukkuriConfig.Default.OptionSettings.MPThreshold &&
                        previousePartyMember.MPRate > (decimal)TTSYukkuriConfig.Default.OptionSettings.MPThreshold)
                    {
                        this.Speak(mpTextToSpeak);
                    }
                    else
                    {
                        if (mpp <= decimal.Zero && previousePartyMember.MPRate != decimal.Zero)
                        {
                            this.Speak(pcname + "、MPなし。");
                        }
                    }
                }

                // TPをチェックして読上げる
                if (TTSYukkuriConfig.Default.OptionSettings.EnabledTPWatch &&
                    !string.IsNullOrWhiteSpace(tpTextToSpeak))
                {
                    if (tpp <= (decimal)TTSYukkuriConfig.Default.OptionSettings.TPThreshold &&
                        previousePartyMember.TPRate > (decimal)TTSYukkuriConfig.Default.OptionSettings.TPThreshold)
                    {
                        this.Speak(tpTextToSpeak);
                    }
                    else
                    {
                        if (tpp <= decimal.Zero && previousePartyMember.TPRate != decimal.Zero)
                        {
                            this.Speak(pcname + "、TPなし。");
                        }
                    }
                }

                // 今回の状態を保存する
                previousePartyMember.HPRate = hpp;
                previousePartyMember.MPRate = mpp;
                previousePartyMember.TPRate = tpp;
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
