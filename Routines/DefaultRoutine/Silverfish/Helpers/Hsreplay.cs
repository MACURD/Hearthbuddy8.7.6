using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using log4net;
using Triton.Common.LogUtilities;

namespace HREngine.Bots
{
    public class Hsreplay
    {
        private static readonly ILog ilog_0 = Logger.GetLoggerInstanceForType();

        public static List<CardStats> AllCardStats = new List<CardStats>();

        public static List<CardStats> StandardCardStats = new List<CardStats>();

        private static Hsreplay instance;

        public static Hsreplay Instance
        {
            get
            {
                if (instance == null)
                {
                    Helpfunctions.Instance.ErrorLog("开始加载Hsreplay卡牌数据");
                    ilog_0.Info("开始加载Hsreplay卡牌数据");
                    var dt = DateTime.Now;
                    instance = new Hsreplay();
                    AllCardStats = LoadAllCardStats();
                    Helpfunctions.Instance.ErrorLog("加载完毕，总共 " + (AllCardStats.Count + StandardCardStats.Count) + " 条数据，总计用时: " + (DateTime.Now - dt).TotalSeconds + " s");
                    ilog_0.Info("加载完毕，总共 " + (AllCardStats.Count + StandardCardStats.Count) + " 条数据，总计用时: " + (DateTime.Now - dt).TotalSeconds + " s");
                }
                return instance;
            }
        }

        public static List<CardStats> LoadAllCardStats()
        {
            string directoryPath = Path.Combine(AppContext.BaseDirectory, "Routines", "DefaultRoutine", "Silverfish", "Hsreplay");
            var allCardStats = new List<CardStats>();

            // 获取目录下所有的JSON文件
            foreach (string file in Directory.GetFiles(directoryPath, "*.json"))
            {
                // 读取JSON文件内容
                string jsonContent = File.ReadAllText(file);

                // 反序列化为CardStats对象列表
                var cardStatsList = JsonConvert.DeserializeObject<List<CardStats>>(jsonContent);

                // 处理每个对象中的null值
                foreach (var cardStats in cardStatsList)
                {
                    if (cardStats.IncludedPopularity == null) cardStats.IncludedPopularity = 0.0;
                    if (cardStats.IncludedCount == null) cardStats.IncludedCount = 0.0;
                    if (cardStats.IncludedWinrate == null) cardStats.IncludedWinrate = 0.0;
                    if (cardStats.AvgTurnsInHand == null) cardStats.AvgTurnsInHand = 0.0;
                    if (cardStats.AvgTurnPlayedOn == null) cardStats.AvgTurnPlayedOn = 0.0;
                    if (cardStats.KeepPercentage == null) cardStats.KeepPercentage = 0.0;
                    if (cardStats.OpeningHandWinrate == null) cardStats.OpeningHandWinrate = 0.0;
                    if (cardStats.WinrateWhenDrawn == null) cardStats.WinrateWhenDrawn = 0.0;
                    if (cardStats.WinrateWhenPlayed == null) cardStats.WinrateWhenPlayed = 0.0;
                }

                if (file.Contains("HsreplayStandard.json"))
                {
                    StandardCardStats.AddRange(cardStatsList);
                }
                else
                {
                    // 将处理后的对象列表添加到字典中
                    allCardStats.AddRange(cardStatsList);
                }
            }
            return allCardStats;
        }
    }

    public class CardStats
    {
        [JsonProperty("dbf_id")]
        public string DbfId { get; set; }

        [JsonProperty("included_popularity")]
        public double? IncludedPopularity { get; set; }

        [JsonProperty("included_count")]
        public double? IncludedCount { get; set; }

        [JsonProperty("included_winrate")]
        public double? IncludedWinrate { get; set; }

        [JsonProperty("times_played")]
        public int TimesPlayed { get; set; }

        [JsonProperty("avg_turns_in_hand")]
        public double? AvgTurnsInHand { get; set; }

        [JsonProperty("avg_turn_played_on")]
        public double? AvgTurnPlayedOn { get; set; }

        [JsonProperty("keep_percentage")]
        public double? KeepPercentage { get; set; }

        [JsonProperty("opening_hand_winrate")]
        public double? OpeningHandWinrate { get; set; }

        [JsonProperty("winrate_when_drawn")]
        public double? WinrateWhenDrawn { get; set; }

        [JsonProperty("winrate_when_played")]
        public double? WinrateWhenPlayed { get; set; }

        // 构造函数处理 null 转为 0 的逻辑
        public CardStats(string dbfId, double includedPopularity, double includedCount, double includedWinrate, int timesPlayed,
                         double avgTurnsInHand, double avgTurnPlayedOn, double keepPercentage, double openingHandWinrate,
                         double winrateWhenDrawn, double winrateWhenPlayed)
        {
            DbfId = dbfId;
            IncludedPopularity = includedPopularity;
            IncludedCount = includedCount;
            IncludedWinrate = includedWinrate;
            TimesPlayed = timesPlayed;
            AvgTurnsInHand = avgTurnsInHand;
            AvgTurnPlayedOn = avgTurnPlayedOn;
            KeepPercentage = keepPercentage;
            OpeningHandWinrate = openingHandWinrate;
            WinrateWhenDrawn = winrateWhenDrawn;
            WinrateWhenPlayed = winrateWhenPlayed;
        }
    }
}
