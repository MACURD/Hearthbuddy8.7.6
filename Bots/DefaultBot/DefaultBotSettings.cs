using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using log4net;
using Newtonsoft.Json;
using Triton.Bot.Settings;
using Triton.Common;
using Triton.Common.LogUtilities;
using Triton.Game.Mapping;
using Hearthbuddy.Windows;
using GreyMagic;

namespace Triton.Bot.Logic.Bots.DefaultBot
{
	public class DefaultBotSettings : JsonSettings
	{
		private static readonly ILog ilog_0 = Common.LogUtilities.Logger.GetLoggerInstanceForType();

		private static DefaultBotSettings defaultBotSettings_0;

		public DefaultBotSettings():base(GetSettingsFilePath(
			Configuration.Instance.Name, string.Format("{0}.json", "DefaultBot")))
		{
			
		}

		public static DefaultBotSettings Instance
		{
			get
			{
				DefaultBotSettings result;
				if ((result = defaultBotSettings_0) == null)
				{
					result = (defaultBotSettings_0 = new DefaultBotSettings());
				}
				return result;
			}
		}

		public void ReloadFile()
		{
			Reload(GetSettingsFilePath(Configuration.Instance.Name,
				string.Format("{0}.json", "DefaultBot" + GetMyHashCode())));
			if (CommandLine.Arguments.Exists("rule"))
			{
				ConstructedGameRule = (VisualsFormatType)(int.Parse(CommandLine.Arguments.Single("rule")) + 1);
				ilog_0.ErrorFormat("[中控设置] 传统对战模式 = {0}.", ConstructedGameRule);
			}
			if (CommandLine.Arguments.Exists("deck"))
			{
				ConstructedCustomDeck = CommandLine.Arguments.Single("deck");
				ilog_0.ErrorFormat("[中控设置] 对战卡组名称 = {0}.", ConstructedCustomDeck);
			}
            if (CommandLine.Arguments.Exists("width"))
            {
				ReleaseLimit = true;
                ReleaseLimitW = int.Parse(CommandLine.Arguments.Single("width"));
                ilog_0.ErrorFormat("[中控设置] 炉石窗口宽度 = {0}.", ReleaseLimitW);
            }
            if (CommandLine.Arguments.Exists("height"))
            {
                ReleaseLimit = true;
                ReleaseLimitH = int.Parse(CommandLine.Arguments.Single("height"));
                ilog_0.ErrorFormat("[中控设置] 炉石窗口高度 = {0}.", ReleaseLimitH);
            }
        }

		//下拉框数据
		private ObservableCollection<VisualsFormatType> observableCollection_3;
		[JsonIgnore]
		public ObservableCollection<VisualsFormatType> AllConstructedRules
		{
			get
			{
				ObservableCollection<VisualsFormatType> result;
				if ((result = observableCollection_3) == null)
				{
					ObservableCollection<VisualsFormatType> observableCollection = new ObservableCollection<VisualsFormatType>();
					observableCollection.Add(VisualsFormatType.狂野);
					observableCollection.Add(VisualsFormatType.标准);
					observableCollection.Add(VisualsFormatType.经典);
					observableCollection.Add(VisualsFormatType.休闲);
					observableCollection.Add(VisualsFormatType.幻变);
					observableCollection_3 = observableCollection;
					result = observableCollection;
				}
				return result;
			}
		}

		//当前对战模式
		private VisualsFormatType visualsFormatType;
		[DefaultValue(VisualsFormatType.狂野)]
		public VisualsFormatType ConstructedGameRule
		{
			get { return visualsFormatType; }
			set
			{
				if (!value.Equals(visualsFormatType))
				{
					visualsFormatType = value;
					NotifyPropertyChanged(() => ConstructedGameRule);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 对战模式 = {0}.", visualsFormatType);
			}
		}

		//卡组名称
		private string string_2;
		[DefaultValue("请选择卡组")]
		public string ConstructedCustomDeck
		{
			get{return string_2;}
			set
			{
				string text = value;
				if (text == null) text = string.Empty;
				if (!text.Equals(string_2))
				{
					string_2 = text;
					NotifyPropertyChanged(() => ConstructedCustomDeck);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 卡组名称 = {0}.", string_2);
			}
		}

		//上次卡组ID
		private long long_0;
		public long LastDeckId
		{
			get{return long_0;}
			internal set{long_0 = value;}
		}

		//开局自动打招呼
		private bool bool_3;
		[DefaultValue(false)]
		public bool AutoGreet
		{
			get{return bool_3;}
			set
			{
				if (!value.Equals(bool_3))
				{
					bool_3 = value;
					NotifyPropertyChanged(() => AutoGreet);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 自动打招呼 = {0}.", bool_3);
			}
		}

		//需要缓存卡组
		private bool bool_8;
		[JsonIgnore]
		[DefaultValue(true)]
		public bool NeedsToCacheCustomDecks
		{
			get{return bool_8;}
			set
			{
				if (!value.Equals(bool_8))
				{
					bool_8 = value;
					NotifyPropertyChanged(() => NeedsToCacheCustomDecks);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 需要缓存卡组 = {0}.", bool_8);
			}
		}

		//炉石窗口宽度
		private int int_w = 144;
		[DefaultValue(144)]
		public int ReleaseLimitW
		{
			get { return int_w; }
			set
			{
				if (!value.Equals(int_w))
				{
					int_w = value;
					if (int_w < 120) int_w = 120;
					if (int_w > 1920) int_w = 1920;
					if (ReleaseLimitH!=(int_w / 4 * 3))
					{
                        ReleaseLimitH = int_w / 4 * 3;
                    }
					NotifyPropertyChanged(() => ReleaseLimitW);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 炉石窗口宽度 = {0}.", int_w);
				try
				{
					if(BotManager.IsRunning && ReleaseLimit)
						Screen.SetResolution(int_w, int_h, 3, 0);
				}
				catch (Exception e)
				{
					ilog_0.ErrorFormat("An exception occurred: {0}.", e);
				}
			}
		}

		//炉石窗口高度
		private int int_h = 108;
		[DefaultValue(108)]
		public int ReleaseLimitH
		{
			get { return int_h; }
			set
			{
				if (!value.Equals(int_h))
				{
					int_h = value;
					if (int_h < 90) int_h = 90;
					if (int_h > 1080) int_h = 1080;
					NotifyPropertyChanged(() => ReleaseLimitH);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 炉石窗口高度 = {0}.", int_h);
			}
		}

		//设置炉石窗口大小
		private bool bool_33;
		[DefaultValue(true)]
		public bool ReleaseLimit
		{
			get { return bool_33; }
			set
			{
				if (!value.Equals(bool_33))
				{
					bool_33 = value;
					NotifyPropertyChanged(() => ReleaseLimit);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 自动设置炉石窗口宽高 = {0}.", bool_33);
				try
				{
					if (bool_33)
					{
						if (BotManager.IsRunning)
							Screen.SetResolution(int_w, int_h, 3, 0);
					}
					else
					{
						if (BotManager.IsRunning)
							Screen.SetResolution(1280, 720, 3, 0);
					}
				}
				catch (Exception e)
				{
					ilog_0.ErrorFormat("An exception occurred: {0}.", e);
				}
			}
		}

		//保持排名(赢1投1)
		private bool bool_4;
		[DefaultValue(false)]
		public bool AutoConcedeAfterConstructedWin
		{
			get {return bool_4;}
			set
			{
				if (!value.Equals(bool_4))
				{
					bool_4 = value;
					NotifyPropertyChanged(() => AutoConcedeAfterConstructedWin);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 保持排名(赢1投1) = {0}.", bool_4);
				if (AutoConcedeAfterConstructedWin)
				{
					if (ForceConcedeAtMulligan) ForceConcedeAtMulligan = false;
					if (NormalConcede) NormalConcede =false;
				}
				if(NeedNowConcede) NeedNowConcede = false;
			}
		}

		//普通互投拿千胜头像
		private bool bool_12;
		[DefaultValue(false)]
		public bool NormalConcede
		{
			get { return bool_12; }
			set
			{
				if (!value.Equals(bool_12))
				{
					bool_12 = value;
					NotifyPropertyChanged(() => NormalConcede);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 普通互投拿千胜头像 = {0}.", bool_12);
				if (NormalConcede)
				{
					if (AutoConcedeAfterConstructedWin) AutoConcedeAfterConstructedWin = false;
					if (ForceConcedeAtMulligan) ForceConcedeAtMulligan = false;
				}
				if (NeedNowConcede) NeedNowConcede = false;
			}
		}

		//急速投降至互投区
		private bool bool_11;
		[DefaultValue(false)]
		public bool ForceConcedeAtMulligan
		{
			get { return bool_11; }
			set
			{
				if (!value.Equals(bool_11))
				{
					bool_11 = value;
					NotifyPropertyChanged(() => ForceConcedeAtMulligan);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 急速投降至互投区 = {0}.", bool_11);
				if (ForceConcedeAtMulligan)
				{
					if (AutoConcedeAfterConstructedWin) AutoConcedeAfterConstructedWin = false;
					if (NormalConcede) NormalConcede = false;
					if (!NeedNowConcede) NeedNowConcede = true;
				}
				else
				{
					if (NeedNowConcede) NeedNowConcede = false;
				}
			}
		}

		//内置极速投降参数
		private bool bool_888;
		[DefaultValue(false)]
		[JsonIgnore]
		public bool NeedNowConcede
		{
			get { return bool_888; }
			set
			{
				if (!value.Equals(bool_888))
				{
					bool_888 = value;
					if (value && !NormalConcede &&
						!AutoConcedeAfterConstructedWin && !ForceConcedeAtMulligan)
					{
						bool_888 = false;
					}
					NotifyPropertyChanged(() => NeedNowConcede);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 立即投降 = {0}.", bool_888);
			}
		}

		//投降最小延时
		private int int_0 = 1500;
		[DefaultValue(1500)]
		public int AutoConcedeMinDelayMs
		{
			get{return int_0;}
			set
			{
				if (!value.Equals(int_0))
				{
					int_0 = value;
					if (int_0 < 0) int_0 = 0;
					if (int_0 > int_1) int_0 = int_1;
					NotifyPropertyChanged(() => AutoConcedeMinDelayMs);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 投降最小延时(ms) = {0}.", int_0);
			}
		}

		//投降最大延时
		private int int_1 = 3000;
		[DefaultValue(3000)]
		public int AutoConcedeMaxDelayMs
		{
			get{return int_1;}
			set
			{
				if (!value.Equals(int_1))
				{
					int_1 = value;
					if (int_1 < 0) int_1 = 0;
					if (int_1 < int_0) int_1 = int_0;
					NotifyPropertyChanged(() => AutoConcedeMaxDelayMs);
				}
				ilog_0.InfoFormat("[天梯脚本设置] 投降最大延时(ms)  = {0}.", int_1);
			}
		}

		//全局动画速度
		private string s_001;
		[DefaultValue("1.0")]
        public string SliderShopSpeedRatioText
		{
            get{return s_001;}
            set
            {
                if (!value.Equals(s_001))
                {
                    s_001 = value;
                    NotifyPropertyChanged(() => SliderShopSpeedRatioText);
                }
                ilog_0.InfoFormat("[天梯脚本设置] 全局动画速度(齿轮) = {0}.", s_001);
            }
        }

		//全局动画速度滑动条
		private float i_001;
		private int i_002;
		private float i_013;
		[DefaultValue(1.0f)]
        public float SliderShopSpeedRatio
        {
            get{return i_001;}
            set
            {
				if (!value.Equals(i_001))
				{
					i_001 = value;
					NotifyPropertyChanged(() => SliderShopSpeedRatio);
					Configuration.Instance.SaveAll();
				}
				try
				{
					if (value > 1 && value < 2)
					{
						float f = value;
						int i = (int)(f * 100);
						f = (float)(i * 1.0) / 100;
						if (i_013 != f)
						{
							SliderShopSpeedRatioText = f.ToString("F1");
							if (BotManager.IsRunning)
								TimeScaleMgr.Get().SetGameTimeScale((float)f);
							i_013 = f;
						}
					}
					else
					{
						int a = (int)value;
						if (i_002 != a)
						{
							SliderShopSpeedRatioText = a.ToString("F1");
							if (BotManager.IsRunning)
								TimeScaleMgr.Get().SetGameTimeScale((float)a);
							i_002 = a;
						}
					}
				}
				catch (Exception e)
				{
					ilog_0.ErrorFormat("An exception occurred: {0}.", e);
				}
            }
        }
	}
}
