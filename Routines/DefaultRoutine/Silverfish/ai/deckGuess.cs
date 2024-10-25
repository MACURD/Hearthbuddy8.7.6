using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HREngine.Bots
{
    public class deckGuess
    {
        /// <summary>
        /// 常见套牌代码
        /// </summary>
        public static Dictionary<string, string> deckDatabase = new Dictionary<string, string>()
        {
            // 2024.08.29 狂野天梯构筑 瞎贼牧萨骑DK战术猎德法
            // 瞎子
            {"AAEBAea5AwSRvALHpAbEuAb8wAYN6bAD8skDvtIDpeIEq+IE5OQF/KgG17gG17oG1sAG2MAG9sAGrcUGAAED87MGx6QG9rMGx6QG6N4Gx6QGAAA=", "狂野快攻瞎" },
            {"AAEBAea5AwSongbHpAb8wAa6wQYNr6AE5OQFsvUFhY4G7p4G/KgG17gG1sAG2MAG9sAGrcUGl8oGy8oGAAED87MGx6QG9rMGx6QG6N4Gx6QGAAA=", "狂野海盗瞎" },
            // 贼
            {"AAEBAaIHBPyjA+WwBMygBdOyBQ2CtAL1uwL9wQKqywPn3QP3nwT7pQS3swT03QTBoQXfwwW/9wXQuAYAAA==", "狂野巨人贼" },
            {"AAEBAaIHApG8AsekBg6MAvsP6bADqssD890DiskEmtsE16MF07IFv/cF1p4G/KUG/KgGyfQGAAED87MGx6QG9rMGx6QG6N4Gx6QGAAA=", "狂野剑鱼贼" },
            {"AAEBAaIHBpG8AsygBdejBb/3BbPBBurEBgyMAvW7AumwA6rLA/PdA/afBIrJBO6eBvylBq2nBvyoBsn0BgAA", "海盗矿锄贼" },
            // 牧师
            {"AAEBAa0GApG8Arv3Aw6hBJEP6bADurYD184Do/cDrfcDjYEE3aQFxKgG/KgG17oG1cEG3PMGAAA=", "狂野快攻牧" },
            {"AAEBAa0GKPcTwxaDuwK1uwLwzwKXhwP8owP9sAPXzgP21gP44wP36AOf6wOt9wO79wO+nwTwnwSEowSLowTlsATHsgS43ASX7wSGgwX9xAXm5AWt6QXP9gXI+AWFjgbDnAbQngbRngbTngaYoAavqAbEqAbGqAbCtgaPzwYAAAEDgxT9xAW42QT9xAXRngb9xAUAAA==", "狂野宇宙牧" },
            {"AAEBAa0GAqS2BO+RBQ7lBNHBAtjBAuuKA+LeA/vfA62KBISjBLjcBKSRBbvHBaLpBe33BfGpBgAA", "狂野控制牧" },
            // 萨满
            {"AAEBAaoIBo31BcekBpy4BvzABrrOBqXTBgzl5AX26AX08gWH+wXDjwbslQaopwb8qAbWwAbYwAb2wAbR0AYAAQPzswbHpAb2swbHpAbo3gbHpAYAAA==", "狂野海盗萨" },
            {"AAEBAaoIBDPN9AKx2QSN9QUNvgbWD9qlA/q0BLLBBIbUBKrZBL3lBPTyBaL6BcOPBpyeBsGeBgAA", "狂野偶数萨" },
            {"AAEBAaoIBOm2BLzOBL/OBPajBg0z7wGTCZEOzg/2vQKPlAO1rQPW9QPgtQS8tgT08gWkpwYAAA==", "狂野大哥萨" },
            // 骑士
            {"AAEBAZ8FBPjSAo7TAoetA4/OAw3VE94Uz4YDy80Dg94Di/gD1awEktQEgeIEwOIE1oAGlpYGorMGAAA=", "中速天启骑" },
            {"AAEBAZ8FBNn+ApXNA7+ABNK5Bg3dCtjHAsDRA8zrA9C9BNO9BOLTBNaABsG2BtS4BsG/BvLJBs7VBgAA", "中速法术骑" },
            {"AAEBAZ8FCL+ABOWwBJfvBIyDBfboBa6DBtK5Bsy/BhDdCt4U2McC2f4CwNEDzOsD0L0E070E4tMEjZYF1oAGwbYG1LgGwb8G8skGztUGAAA=", "中速控制骑" },
            // 死骑
            {"AAEBAfHhBASLkga9sQba5Qbk5QYNseYEh/YEsvcEmIEFlMoFkqAGurEGvLEG9rEGi7cG1uUG1+UG2eUGAAA=", "污手DK"},
            {"AAEBAfHhBAKoigTh5QYO/+MEhuQEkOQEkeQEhfYEh/YEsvcEs/cEmIEFrqEF0J4G1J4G0+UG4OUGAAA=", "邪DK"},
            {"AAEBAfHhBAT++AWT+wXt/wX/lwYN9eMEh/YE9fcFgvgF8vgFu/kF+PkF6/8FyoMG0IMG9YwG94wG85EGAAA=", "疫病DK"},
            {"AAEBAfHhBAjt/wX/lwbHpAakwAaowAbIyQaWywa6zgYL9eMEhY4GlJUGkqAGubEGu7EG/7oGv74Gx8kG/8kGlM8GAAED9bMGx6QG97MGx6QG6N4Gx6QGAAA=", "殡淇淋DK"},
            // 战士
            {"AAEBAQcGkAPx+wKT0AOMtwSfngaPqAYM1ASNEKS2A/mMBI7UBI+VBfDNBbTRBaL6BaH7BZyeBs+eBgAA", "狂野机克战" },
            {"AAEBAQcIk9ADvIoE5bAEl+8EpfYF+L0GycEG+skGENQEz+cCpLYD+YwE+owEiaAEjLcEjtQEj5UF8M0FofsFn54G7KkG1boGjr8G88oGAAA=", "狂野奥丁战" },
            // 术士
            {"AAEBAf0GAo+CA4T7Aw7y0AL40AL6/gLLuQOVzQObzQPXzgPB0QOD+wPO+gXEngajoAbDuAbw5gYAAA==", "自残任务术" },
            {"AAEBAf0GBo+CA52pA4T7A8jrBaOgBp64Bgzy0AL40AL6/gLLuQOVzQObzQPXzgPB0QOD+wPO+gXEngbDuAYAAA==", "黑眼任务术" },
            {"AAEBAf0GAo+CA8ekBg7OB40QvLYC8tAC3IYD/aQD1dED0OEDxYAEmJIFq5IFuLgF9qEGlcoGAAED87MGx6QG9rMGx6QG6N4Gx6QGAAA=", "狂野动物园" },
            // 猎人
            {"AAEBAR8EoIAD25EE6sQGltMGDee9AtSLA/+6A5T8A8OABKmfBNLkBOT1BZn2BeqlBty4Bs7ABovWBgAA", "狂野融合猎" },
            {"AAEBAR8G/q8CwI8D25EE5bAEl+8E/cQFEccPnM0Cq9AC8pYDn6UD9roDnssDlPwDqZ8E458Ema0EqqQF0Z4GjaEGnaIG6KUGzsAGAAEDuwX9xAWAB/3EBbjZBP3EBQAA", "中速亡语猎" },
            // 德鲁伊
            {"AAEBAZICBuwV/esCmu4Cn/MF8foFscEGDI/2Aoz7AvDUA6+ABK6fBLClBK7ABKLpBf2NBomhBtmxBvvlBgAA", "狂野换家德" },
            {"AAEBAZICEIUX/esCj/YC9fwCxf0CiYsE5bAEuNkEl+8E/cQFn/MFqZUGx6QG1boGpLsGpbsGDOkB+AfkCIoOntICiuADr4AErsAEhO8E2/oFiaEGsc4GAAEGoM0C/cQF+7AD/cQFqrEG/cQF9LMGx6QG97MGx6QG6N4Gx6QGAAA=", "豪华换家德" },
            {"AAEBAZICKF/kCIoO7BXDFoUXj/YC9fwCxf0C/KMDjK4Dr4AEiYsEpY0ElKUEz6wEpa0E5bAErsAEuNkEwd8El+8E4qQF/cQFn/MFyPgF2/oFu5UGv5UGwZUG2pwG0Z4GiaEGx6QGr6gG76kGh7EG1boGusEG88oGAAABBoSwBP3EBb6ZBv3EBevmBv3EBfSzBsekBvezBsekBujeBsekBgAA", "狂野宇宙德" },
            // 法师
            {"AAEBAf0EAvyeBOWwBA7mBLT8AvfRA4XkA+DDBez2BfGABtaYBvKbBoieBuepBoG/BoDKBsvQBgAA", "伙伴法术法" },
            {"AAEBAf0EBMAB6PcDvO0E26EFDawBn5sD9KsD0ewDrvcDio0E7PYF1pgGsp4G5aYG5bgGgb8GheYGAAA=", "任务法术法" },
            {"AAEBAf0EBPXnAvSrA/yeBOz2BQ3AAeYEtPwC99EDheQD4MMF8YAG1pgG8psG56kGgb8GgMoGy9AGAAA=", "学徒法术法" },
        };

        /// <summary>
        /// 解析套牌码，匹配常见卡组
        /// </summary>
        /// <returns>猜测卡组名称，默认空字符串</returns>
        public static string guessEnemyDeck(Playfield p)
        {
            Hrtprozis.Instance.enemyDeckName = "";
            Hrtprozis.Instance.similarity = 50;
            foreach (var deck in deckDatabase)
            {
                int similarity = calSimilarity(deck.Key);
                // 相似度阈值
                if(similarity > Hrtprozis.Instance.similarity)
                {
                    Hrtprozis.Instance.similarity = similarity;
                    Hrtprozis.Instance.guessEnemyDeck = new Dictionary<string, int>();
                    // 以对手视角计算
                    Playfield enemyPlayField = new Playfield(p, true);
                    calDeck(deck.Key, enemyPlayField);
                    Hrtprozis.Instance.enemyDeckName = deck.Value;
                }
            }
            return Hrtprozis.Instance.enemyDeckName;
        }

        /// <summary>
        /// 计算套牌相似度,会和对手坟场和场面比对（仅可收藏卡牌）
        /// </summary>
        /// <param name="deck">卡牌代码</param>
        /// <returns>相似度,百分比形式</returns>
        public static int calSimilarity(string deck)
        {
            List<string> decks = new List<string>();

            // 解析卡组代码
            byte[] bytes = Convert.FromBase64String(deck);

            int i = 0;
            // 保留字节，始终为 0
            if (bytes[i++] != 0) return 0;
            // 版本号始终为 1
            if (bytes[i++] != 1) return 0;
            // 模式 1 狂野 2 标准 
            i++;
            // 英雄卡牌数量默认 1 
            int numHeros = bytes[i++];
            // 读取英雄
            for(int j = 0; j < numHeros; j++)
            {
                ulong result = 0;
                int length = 0;
                while (i < bytes.Length)
                {
                    var value = (ulong)bytes[i] & 0x7f;
                    result |= value << 7 * (length++);
                    if ((bytes[i++] & 0x80) != 0x80)
                        break;
                }
                // 可以在这里判断英雄职业，如果和当前职业不符直接退出
                if (CardDB.Instance.getCardDataFromDbfID(result + "").Class != (int)Hrtprozis.Instance.enemyHeroStartClass) return 0;
            }
            // 放入一张的卡牌
            int numOne = bytes[i++];
            for (int j = 0; j < numOne; j++)
            {
                ulong result = 0;
                int length = 0;
                while (i < bytes.Length)
                {
                    var value = (ulong)bytes[i] & 0x7f;
                    result |= value << 7 * (length++);
                    if ((bytes[i++] & 0x80) != 0x80)
                        break;
                }
                // 加入卡组
                decks.Add("" + result);
            }
            // 放入两张的卡牌
            int numTwo = bytes[i++];
            for (int j = 0; j < numTwo; j++)
            {
                ulong result = 0;
                int length = 0;
                while (i < bytes.Length)
                {
                    var value = (ulong)bytes[i] & 0x7f;
                    result |= value << 7 * (length++);
                    if ((bytes[i++] & 0x80) != 0x80)
                        break;
                }
                // 加入卡组
                decks.Add("" + result);
            }
            // 放入N张的卡牌(理论上应该是 0)
            int numMore = bytes[i++];
            for (int j = 0; j < numMore; j++)
            {
                ulong result = 0;
                int length = 0;
                while (i < bytes.Length)
                {
                    var value = (ulong)bytes[i] & 0x7f;
                    result |= value << 7 * (length++);
                    if ((bytes[i++] & 0x80) != 0x80)
                        break;
                }
                // 加入卡组
                decks.Add("" + result);
            }

            int maxCount = 0;
            int sameCount = 0;
            // 比对对手任务...话说这就已经可以直接确定了吧...
            if(Questmanager.Instance.enemyQuest.maxProgress != 1000)
            {
                CardDB.Card card = CardDB.Instance.getCardDataFromID(Questmanager.Instance.enemyQuest.Id);
                if (card.Collectable)
                {
                    if (decks.Contains(card.dbfId))
                        sameCount++;
                    maxCount++;
                }
            }
            // 武器
            if(Hrtprozis.Instance.enemyWeapon.Durability > 0)
            {
                CardDB.Card card = Hrtprozis.Instance.enemyWeapon.card;
                if (card.Collectable)
                {
                    if (decks.Contains(card.dbfId))
                        sameCount++;
                    maxCount++;
                }
            }
            // 比对场面上的卡
            foreach(Minion m in Hrtprozis.Instance.enemyMinions)
            {
                if (!m.handcard.card.Collectable) continue;
                if( decks.Contains(m.handcard.card.dbfId) )
                    sameCount++;
                maxCount++;
            }
            // 比对坟场
            foreach(var deckCard in Probabilitymaker.Instance.enemyGraveyard)
            {
                CardDB.Card card = CardDB.Instance.getCardDataFromID(deckCard.Key);
                if (!card.Collectable) continue;
                if (decks.Contains(card.dbfId))
                    sameCount++;
                maxCount++;
            }

            if (deck == "AAEBAaoIBIQXqO4DxtED4AUN26AE2wPKqwOV8AO1mAOLzgKU6AOdwgKz6AOMlAOnCN3sA78XAA==")
            {
               sameCount = 0;
                maxCount = 0;
                // 鱼人萨衍生物判断
                // 比对场面上的卡
                foreach (Minion m in Hrtprozis.Instance.enemyMinions)
                {
                    if (!m.handcard.card.Collectable) continue;
                    if (m.handcard.card.race == CardDB.Race.MURLOC || decks.Contains(m.handcard.card.dbfId))
                        sameCount++;
                    maxCount++;
                }
                // 比对坟场
                foreach (var deckCard in Probabilitymaker.Instance.enemyGraveyard)
                {
                    CardDB.Card card = CardDB.Instance.getCardDataFromID(deckCard.Key);
                    if (!card.Collectable) continue;
                    if (card.race == CardDB.Race.MURLOC || decks.Contains(card.dbfId))
                        sameCount++;
                    maxCount++;
                }
            }

            if (maxCount > 0)
            {
                return sameCount * 100 / maxCount;
            }
            return 0;
        }

        /// <summary>
        /// 记录套牌信息，对手下回合斩杀线
        /// </summary>
        /// <param name="deck">卡牌代码</param>
        public static void calDeck(string deck, Playfield p)
        {
            Hrtprozis.Instance.enemyDeckCode = deck;
            // 解析卡组代码
            byte[] bytes = Convert.FromBase64String(deck);

            int i = 3;
            // 英雄卡牌数量默认 1 
            int numHeros = bytes[i++];
            // 读取英雄
            for (int j = 0; j < numHeros; j++)
            {
                ulong result = 0;
                int length = 0;
                while (i < bytes.Length)
                {
                    var value = (ulong)bytes[i] & 0x7f;
                    result |= value << 7 * (length++);
                    if ((bytes[i++] & 0x80) != 0x80)
                        break;
                }
            }
            // 放入一张的卡牌
            int numOne = bytes[i++];
            for (int j = 0; j < numOne; j++)
            {
                ulong result = 0;
                int length = 0;
                while (i < bytes.Length)
                {
                    var value = (ulong)bytes[i] & 0x7f;
                    result |= value << 7 * (length++);
                    if ((bytes[i++] & 0x80) != 0x80)
                        break;
                }
                // 加入卡组
                Hrtprozis.Instance.guessEnemyDeck.Add(""+ result, 1);
            }
            // 放入两张的卡牌
            int numTwo = bytes[i++];
            for (int j = 0; j < numTwo; j++)
            {
                ulong result = 0;
                int length = 0;
                while (i < bytes.Length)
                {
                    var value = (ulong)bytes[i] & 0x7f;
                    result |= value << 7 * (length++);
                    if ((bytes[i++] & 0x80) != 0x80)
                        break;
                }
                // 加入卡组
                Hrtprozis.Instance.guessEnemyDeck.Add("" + result, 2);
            }
            // 放入N张的卡牌(理论上应该是 0)
            int numMore = bytes[i++];
            for (int j = 0; j < numMore; j++)
            {
                ulong result = 0;
                int length = 0;
                while (i < bytes.Length)
                {
                    var value = (ulong)bytes[i] & 0x7f;
                    result |= value << 7 * (length++);
                    if ((bytes[i++] & 0x80) != 0x80)
                        break;
                }
                // 加入卡组
                Hrtprozis.Instance.guessEnemyDeck.Add("" + result, 3);
            }


            // 武器
            if (Hrtprozis.Instance.enemyWeapon.Durability > 0)
            {
                CardDB.Card card = Hrtprozis.Instance.enemyWeapon.card;
                if (card.Collectable)
                {
                    if (Hrtprozis.Instance.guessEnemyDeck.ContainsKey(card.dbfId))
                    {
                        Hrtprozis.Instance.guessEnemyDeck[card.dbfId]--;
                        if (Hrtprozis.Instance.guessEnemyDeck[card.dbfId] < 0)
                        {
                            Hrtprozis.Instance.guessEnemyDeck.Remove(card.dbfId);
                        }
                    }
                }

            }
            if (Probabilitymaker.Instance.enemyGraveyard.ContainsKey(CardDB.cardIDEnum.SW_428t2))
            {
                // 古夫
                Hrtprozis.Instance.guessEnemyDeck.Add("67884", 1);
            }
            if (Probabilitymaker.Instance.enemyGraveyard.ContainsKey(CardDB.cardIDEnum.SCH_514))
            {
                if (Hrtprozis.Instance.guessEnemyDeck.ContainsKey("49018"))
                {
                    Hrtprozis.Instance.guessEnemyDeck["49018"]++;
                }else
                {
                    Hrtprozis.Instance.guessEnemyDeck.Add("49018", 1);
                }
            }

            // 比对坟场
            foreach (var deckCard in Probabilitymaker.Instance.enemyGraveyard)
            {
                CardDB.Card card = CardDB.Instance.getCardDataFromID(deckCard.Key);
                if (Hrtprozis.Instance.guessEnemyDeck.ContainsKey(card.dbfId))
                {
                    Hrtprozis.Instance.guessEnemyDeck[card.dbfId]-= deckCard.Value;
                    if (Hrtprozis.Instance.guessEnemyDeck[card.dbfId] < 0)
                    {
                        Hrtprozis.Instance.guessEnemyDeck.Remove(card.dbfId);
                    }
                }
                // TODO 特殊任务衍生卡
            }
            // 比对场面上的卡
            foreach (Minion m in Hrtprozis.Instance.enemyMinions)
            {
                if (Hrtprozis.Instance.guessEnemyDeck.ContainsKey(m.handcard.card.dbfId))
                {
                    Hrtprozis.Instance.guessEnemyDeck[m.handcard.card.dbfId]--;
                    if (Hrtprozis.Instance.guessEnemyDeck[m.handcard.card.dbfId] < 0)
                    {
                        Hrtprozis.Instance.guessEnemyDeck.Remove(m.handcard.card.dbfId);
                    }
                }
            }
            if (Hrtprozis.Instance.guessEnemyDeck.ContainsKey("8659"))
            {
                if (Hrtprozis.Instance.guessEnemyDeck.ContainsKey("49018"))
                {
                    Hrtprozis.Instance.guessEnemyDeck["49018"]++;
                }
                else
                {
                    Hrtprozis.Instance.guessEnemyDeck.Add("49018", 1);
                }
            }

            calDirectDmg(p);
        }

        /// <summary>
        /// 交换函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        /// <summary>
        /// 计算对手直伤伤害
        /// 只计算对手打出手牌一半（向上取整）可能造成的最高伤害
        /// </summary>
        public static int calDirectDmg(Playfield enemyPlayField)
        {
            enemyPlayField.owncards = new List<Handmanager.Handcard>();
            foreach(var item in Hrtprozis.Instance.guessEnemyDeck)
            {
                CardDB.Card card = CardDB.Instance.getCardDataFromDbfID(item.Key);
                // 加入到对手可能的手牌数量
                for(int i = 0; i < item.Value; i++)
                {
                    enemyPlayField.owncards.Add(new Handmanager.Handcard(card) { manacost = card.cost});
                }
            }
            int nextTurnMana = enemyPlayField.ownMaxMana + 1;
            nextTurnMana = nextTurnMana > 10 ? 10 : nextTurnMana;
            // 最多计算对手手牌数量的一半（向上取整）
            int calMax = (enemyPlayField.enemyAnzCards+1) / 2 + (enemyPlayField.enemyAnzCards + 1) % 2;
            Hrtprozis.Instance.enemyDirectDmg = enemyPlayField.calDirectDmg(nextTurnMana, false, true, enemyPlayField.owncards.Count, calMax);
            return Hrtprozis.Instance.enemyDirectDmg;
        }
    }
}
