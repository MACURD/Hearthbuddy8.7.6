using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HREngine.Bots;

namespace HREngine.Bots
{
    public class CardHelper
    {
        private static readonly Dictionary<string, Type> SimTypesDict;

        static CardHelper()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var baseType = typeof(SimTemplate);
            SimTypesDict = assembly.GetTypes().AsParallel().Where(
                t => t.Namespace == "HREngine.Bots" && t.BaseType == baseType).ToDictionary(t => t.Name, t => t);
        }

        // 根据id获取对应sim
        public static SimTemplate GetCardSimulation(CardDB.cardIDEnum tempCardIdEnum)
        {
            SimTemplate result = new SimTemplate();

            switch (tempCardIdEnum)
            {
                // 战士皮肤
                case CardDB.cardIDEnum.VAN_HERO_01bp:
                case CardDB.cardIDEnum.CS2_102_H1:
                case CardDB.cardIDEnum.CS2_102_H2:
                case CardDB.cardIDEnum.CS2_102_H3:
                case CardDB.cardIDEnum.CS2_102_H4:
                case CardDB.cardIDEnum.HERO_01dbp:
                case CardDB.cardIDEnum.HERO_01fbp:
                case CardDB.cardIDEnum.VAN_CS2_102_H3:
                    tempCardIdEnum = CardDB.cardIDEnum.HERO_01bp;
                    break;
                // 萨满皮肤
                case CardDB.cardIDEnum.VAN_HERO_02bp:
                case CardDB.cardIDEnum.CS2_049_H1:
                case CardDB.cardIDEnum.CS2_049_H2:
                case CardDB.cardIDEnum.CS2_049_H3:
                case CardDB.cardIDEnum.CS2_049_H4:
                case CardDB.cardIDEnum.CS2_049_H5:
                case CardDB.cardIDEnum.HERO_02fbp:
                case CardDB.cardIDEnum.HERO_02mbp:
                    tempCardIdEnum = CardDB.cardIDEnum.HERO_02bp;
                    break;
                // 潜行者皮肤
                case CardDB.cardIDEnum.CS2_083b_H1:
                case CardDB.cardIDEnum.CS2_083b_H2:
                case CardDB.cardIDEnum.HERO_03dbp:
                case CardDB.cardIDEnum.VAN_HERO_03bp:
                    tempCardIdEnum = CardDB.cardIDEnum.HERO_03bp;
                    break;
                // 圣骑士皮肤
                case CardDB.cardIDEnum.HERO_04lbp:
                case CardDB.cardIDEnum.CS2_101_H1:
                case CardDB.cardIDEnum.CS2_101_H2:
                case CardDB.cardIDEnum.CS2_101_H3:
                case CardDB.cardIDEnum.CS2_101_H4:
                case CardDB.cardIDEnum.HERO_04fbp:
                case CardDB.cardIDEnum.HERO_04ebp:
                    tempCardIdEnum = CardDB.cardIDEnum.HERO_04bp;
                    break;
                // 猎人皮肤
                case CardDB.cardIDEnum.VAN_HERO_05bp:
                case CardDB.cardIDEnum.DS1h_292_H1:
                case CardDB.cardIDEnum.DS1h_292_H2:
                case CardDB.cardIDEnum.DS1h_292_H3:
                case CardDB.cardIDEnum.HERO_05dbp:
                    tempCardIdEnum = CardDB.cardIDEnum.HERO_05bp;
                    break;
                // 德鲁伊皮肤
                case CardDB.cardIDEnum.VAN_HERO_06bp:
                case CardDB.cardIDEnum.CS2_017_HS1:
                case CardDB.cardIDEnum.CS2_017_HS2:
                case CardDB.cardIDEnum.CS2_017_HS3:
                case CardDB.cardIDEnum.CS2_017_HS4:
                case CardDB.cardIDEnum.HERO_06ebp:
                case CardDB.cardIDEnum.HERO_06fbp:
                case CardDB.cardIDEnum.HERO_06zbp:
                    tempCardIdEnum = CardDB.cardIDEnum.HERO_06bp;
                    break;
                // 术士皮肤
                case CardDB.cardIDEnum.VAN_HERO_07bp:
                case CardDB.cardIDEnum.CS2_056_H1:
                case CardDB.cardIDEnum.CS2_056_H2:
                case CardDB.cardIDEnum.CS2_056_H3:
                case CardDB.cardIDEnum.HERO_07dbp:
                case CardDB.cardIDEnum.HERO_07ebp:
                    tempCardIdEnum = CardDB.cardIDEnum.HERO_07bp;
                    break;
                // 法师皮肤
                case CardDB.cardIDEnum.CS2_034_H1:
                case CardDB.cardIDEnum.CS2_034_H2:
                case CardDB.cardIDEnum.CS2_034_H3:
                case CardDB.cardIDEnum.CS2_034_H4:
                case CardDB.cardIDEnum.HERO_08ebp:
                case CardDB.cardIDEnum.HERO_08fbp:
                case CardDB.cardIDEnum.TRLA_Mage_01:
                case CardDB.cardIDEnum.VAN_HERO_08bp:
                case CardDB.cardIDEnum.HERO_08lbp:
                    tempCardIdEnum = CardDB.cardIDEnum.HERO_08bp;
                    break;
                // 牧师皮肤
                case CardDB.cardIDEnum.CS1h_001_H1:
                case CardDB.cardIDEnum.CS1h_001_H2:
                case CardDB.cardIDEnum.CS1h_001_H3:
                case CardDB.cardIDEnum.HERO_09dbp:
                case CardDB.cardIDEnum.VAN_HERO_09bp:
                    tempCardIdEnum = CardDB.cardIDEnum.HERO_09bp;
                    break;
                // 恶魔猎人皮肤
                case CardDB.cardIDEnum.HERO_10bbp:
                case CardDB.cardIDEnum.HERO_10bpe:
                case CardDB.cardIDEnum.HERO_10cbp:
                case CardDB.cardIDEnum.TB_HunterPrince_04:
                case CardDB.cardIDEnum.VAN_HERO_10bp:
                    tempCardIdEnum = CardDB.cardIDEnum.HERO_10bp;
                    break;
                // 异画幸运币
                case CardDB.cardIDEnum.DMF_COIN1:
                case CardDB.cardIDEnum.DMF_COIN2:
                case CardDB.cardIDEnum.LOOTA_BOSS_45p:
                case CardDB.cardIDEnum.BAR_COIN1:
                case CardDB.cardIDEnum.BAR_COIN2:
                case CardDB.cardIDEnum.BAR_COIN3:
                case CardDB.cardIDEnum.SW_COIN1:
                case CardDB.cardIDEnum.SW_COIN2:
                case CardDB.cardIDEnum.REV_COIN1:
                case CardDB.cardIDEnum.REV_COIN2:
                    tempCardIdEnum = CardDB.cardIDEnum.GAME_005;
                    break;
            }

            var className = string.Format("Sim_{0}", tempCardIdEnum);
            var simType = GetTypeByName(className);
            if (simType != null)
            {
                result = Activator.CreateInstance(simType) as SimTemplate;
            }
            //else
            //{
            //    Helpfunctions.Instance.ErrorLog("missing sim class: " + className);
            //}
            return result;
        }

        /// <summary>
        /// Gets a all Type instances matching the specified class name with just non-namespace qualified class name.
        /// </summary>
        /// <param name="className">Name of the class sought.</param>
        /// <returns>Types that have the class name specified. They may not be in the same namespace.</returns>
        public static Type GetTypeByName(string className)
        {
            Type t;
            if (SimTypesDict.TryGetValue(className, out t))
                return t;
            return null;
        }

        public static bool IsCardSimulationImplemented(SimTemplate cardSimulation)
        {
            var type = cardSimulation.GetType();
            var baseType = typeof(SimTemplate);
            bool implemented = type.IsSubclassOf(baseType);
            return implemented;
        }
    }
}
