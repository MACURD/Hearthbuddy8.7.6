using Buddy.Coroutines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Triton.Game;
using Triton.Game.Mapping;

namespace HREngine.Bots
{
    static class Extensions
    {
        /// <summary>
        /// 扩展方法，用于异步执行卡片的卡组操作。
        /// </summary>
        /// <param name="card">需要卡组操作的HSCard对象。</param>
        /// <param name="timeout">等待超时时间（毫秒），默认500毫秒。</param>
        internal static async Task DeckAction(this HSCard card, int timeout = 500)
        {
            // 异步拿起卡片，等待指定时间
            await card.Pickup(timeout);

            // 开始计时器，用于判断超时
            Stopwatch stopwatch = Stopwatch.StartNew();
            bool canTradeOrForge = false;

            // 在指定的超时时间内循环检测ZoneMgr是否已初始化
            while (stopwatch.ElapsedMilliseconds < timeout)
            {
                // 如果ZoneMgr尚未初始化，每隔50毫秒检查一次
                if (ZoneMgr.Get() == null)
                {
                    await Coroutine.Sleep(50);
                }
                else
                {
                    // ZoneMgr已初始化，标记为可进行卡组操作并退出循环
                    canTradeOrForge = true;
                    break;
                }
            }

            // 如果超时且无法进行卡组操作，直接返回
            if (!canTradeOrForge)
                return;

            // 查找卡组操作区域的碰撞器，用于确定卡片放置位置
            Collider collider = Board.Get().FindCollider("DeckActionArea");

            // 获取卡组操作区域的中心点
            var center = collider.Bounds.m_Center;

            // 模拟人类操作，将光标移动到卡组操作区域的中心点
            await Client.MoveCursorHumanLike(center);

            // 稍作延迟，模拟真实用户的点击行为
            await Coroutine.Sleep(1);

            // 在卡组操作区域的中心点执行左键点击操作，完成卡组操作
            Client.LeftClickAt(center);
        }

        /// <summary>
        /// 鼠标左击卡牌
        /// </summary>
        /// <param name="card"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        internal static async Task LeftClickCard(this HSCard card, int timeout = 500)
        {
            Vector3 center = Client.CardInteractPoint(card.Card);
            await Client.MoveCursorHumanLike(center);
            Client.LeftClickAt(center);
        }

        internal static ConstructorInfo hsCardCtor = typeof(HSCard).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
            null, new Type[] { typeof(Entity) }, null);
        static PerFrameCachedValue<Dictionary<int, HSCard>> cachedCardsDict;
        static int maxEntityId = 100;
        internal static Dictionary<int, HSCard> AllCardsDict
        {
            get
            {
                if (cachedCardsDict == null)
                {
                    cachedCardsDict = new PerFrameCachedValue<Dictionary<int, HSCard>>(() =>
                    {
                        var dict = new Dictionary<int, HSCard>();
                        var id = 0;
                        var paramArr = new object[1];
                        while (id++ < maxEntityId)
                        {
                            var e = TritonHs.GameState.GetEntity(id);
                            if (e != null)
                            {
                                paramArr[0] = e;
                                dict[id] = (HSCard)hsCardCtor.Invoke(paramArr);
                                maxEntityId = Math.Max(id + 30, maxEntityId);
                            }
                        }
                        return dict;
                    });
                }
                return cachedCardsDict;
            }
        }

        internal static List<HSCard> GetAllCards()
        {
            return AllCardsDict.Values.ToList();
        }
        /// <summary>
        /// 每局游戏新开时重置初始的最大ID
        /// </summary>
        internal static void ResetMaxId()
        {
            maxEntityId = 100;
        }
    }
}
