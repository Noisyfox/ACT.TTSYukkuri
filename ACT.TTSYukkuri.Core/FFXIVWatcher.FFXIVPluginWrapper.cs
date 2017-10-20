using System.Collections.Generic;
using System.Linq;

namespace ACT.TTSYukkuri
{
    /// <summary>
    /// FF14を監視する FF14Pluginラップ部分
    /// </summary>
    public partial class FFXIVWatcher
    {
        /// <summary>
        /// 戦闘メンバリストを取得する
        /// </summary>
        /// <returns>戦闘メンバリスト</returns>
        public IReadOnlyList<Combatant> GetCombatantList() => FFXIVPluginHelper.GetCombatantList();

        /// <summary>
        /// パーティの戦闘メンバリストを取得する
        /// </summary>
        /// <returns>パーティの戦闘メンバリスト</returns>
        public List<Combatant> GetCombatantListParty()
        {
            // 総戦闘メンバリストを取得する（周囲のPC, NPC, MOB等すべて）
            var combatListAll = this.GetCombatantList();

            // パーティメンバのIDリストを取得する
            int partyCount;
            var partyListById = FFXIVPluginHelper.GetCurrentPartyList(out partyCount);

            var combatListParty = new List<Combatant>();

            foreach (var partyMemberId in partyListById)
            {
                if (partyMemberId == 0)
                {
                    continue;
                }

                var partyMember = (
                    from x in combatListAll
                    where
                    x.ID == partyMemberId
                    select
                    x).FirstOrDefault();

                if (partyMember != null)
                {
                    combatListParty.Add(partyMember);
                }
            }

            return combatListParty;
        }

        /// <summary>
        /// プレイヤ情報を取得する
        /// </summary>
        /// <returns>プレイヤ情報</returns>
        public Combatant GetPlayer()
        {
            var list = FFXIVPluginHelper.GetCombatantList();

            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return new Combatant();
            }
        }
    }
}
