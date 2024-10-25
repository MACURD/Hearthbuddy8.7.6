using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Buddy.Coroutines;
using log4net;
using Triton.Bot.Settings;
using Triton.Common;
using Triton.Common.LogUtilities;
using Triton.Game;
using Triton.Game.Abstraction;
using Triton.Game.Data;
using Triton.Game.Mapping;

namespace Triton.Bot.Logic.Bots.DefaultBot
{
	public class DefaultBot : IRunnable, IAuthored, IBase, IBot, IConfigurable
	{
		private static readonly ILog ilog_0;

		private Coroutine coroutine_0;

		private EmoteType emoteType_0;

		private DateTime dateTime_0;

		private DateTime dateTime_1 = DateTime.Now;

		private readonly Dictionary<string, Stopwatch> dictionary_0 =
			new Dictionary<string, Stopwatch>();

		private bool bool_2;

		private UserControl userControl_0;

		private readonly Stopwatch stopwatch_0 = new Stopwatch();

		private int int_0;

		private bool bool_4;

		/// <summary> The name of the Bot. </summary>
		public string Name
		{
			get { return "天梯脚本"; }
		}

		/// <summary> The description of the Bot. </summary>
		public string Description
		{
			get { return "【琴弦上的宇宙】修改的天梯脚本."; }
		}

		/// <summary>The author of the Bot.</summary>
		public string Author
		{
			get { return "开源软件"; }
		}

		/// <summary>The version of the Bot.</summary>
		public string Version
		{
			get { return "1.0.0.0"; }
		}

		public override string ToString()
		{
			return Name + ": " + Description;
		}

		public JsonSettings Settings
		{
			get { return DefaultBotSettings.Instance; }
		}

		static DefaultBot()
		{
			ilog_0 = Common.LogUtilities.Logger.GetLoggerInstanceForType();
		}

		public void Initialize() { }

		public void Deinitialize() { }

		private void method_11(object sender, RoutedEventArgs e)
		{
			DefaultBotSettings.Instance.NeedsToCacheCustomDecks = true;
		}

		//绑定各种控件下拉框和初始值
		public UserControl Control
		{
			get
			{
				if (userControl_0 != null) return userControl_0;
				using (FileStream stream = new 
					FileStream("Bots/DefaultBot/SettingsGui.xaml", FileMode.Open))
				{
					UserControl userControl = (UserControl)XamlReader.Load(stream);

					//对战模式
					if (!Wpf.SetupComboBoxItemsBinding(userControl, "ConstructedRulesComboBox", "AllConstructedRules", BindingMode.OneWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupComboBoxItemsBinding failed for 'ConstructedRulesComboBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}
					if (!Wpf.SetupComboBoxSelectedItemBinding(userControl, "ConstructedRulesComboBox", "ConstructedGameRule", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupComboBoxSelectedItemBinding failed for 'ConstructedRulesComboBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}

					//卡组名称
					if (!Wpf.SetupTextBoxBinding(userControl, "ConstructedCustomDeckTextBox", "ConstructedCustomDeck", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'ConstructedCustomDeckTextBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}

					//开局自动打招呼
					if (!Wpf.SetupCheckBoxBinding(userControl, "AutoGreetCheckBox", "AutoGreet", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupCheckBoxBinding failed for 'AutoGreetCheckBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}

					//重新缓存卡组
					if (!Wpf.SetupCheckBoxBinding(userControl, "NeedsToCacheCustomDecksCheckBox", "NeedsToCacheCustomDecks", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupCheckBoxBinding failed for 'NeedsToCacheCustomDecksCheckBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}
					Wpf.FindControlByName<Button>(userControl,
						"RecacheCustomDecksButton").Click += method_11;

					//自动设置炉石窗口
					if (!Wpf.SetupCheckBoxBinding(userControl, "ReleaseLimitCheckBox", "ReleaseLimit", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupCheckBoxBinding failed for 'AutoGreetCheckBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}
					if (!Wpf.SetupTextBoxBinding(userControl, "ReleaseLimitWTextBox",
						"ReleaseLimitW", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'ReleaseLimitWTextBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}
					if (!Wpf.SetupTextBoxBinding(userControl, "ReleaseLimitHTextBox",
						"ReleaseLimitH", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'ReleaseLimitHTextBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}

					//保持天梯排名
					if (!Wpf.SetupCheckBoxBinding(userControl, "AutoConcedeAfterConstructedWinCheckBox", "AutoConcedeAfterConstructedWin", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupCheckBoxBinding failed for 'AutoConcedeAfterConstructedWinCheckBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}

					//普通互投拿千胜头像
					if (!Wpf.SetupCheckBoxBinding(userControl, "NormalConcedeCheckBox", 
						"NormalConcede", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupCheckBoxBinding failed for 'NormalConcedeCheckBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}

					//急速投降至互投区
					if (!Wpf.SetupCheckBoxBinding(userControl, "ForceConcedeAtMulliganCheckBox", "ForceConcedeAtMulligan", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupCheckBoxBinding failed for 'ForceConcedeAtMulliganCheckBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}

					//投降延时
					if (!Wpf.SetupTextBoxBinding(userControl, "AutoConcedeMinDelayMsTextBox", "AutoConcedeMinDelayMs", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'AutoConcedeMinDelayMsTextBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}
					if (!Wpf.SetupTextBoxBinding(userControl, "AutoConcedeMaxDelayMsTextBox", "AutoConcedeMaxDelayMs", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupTextBoxBinding failed for 'AutoConcedeMaxDelayMsTextBox'.");
						throw new Exception("The SettingsControl could not be created.");
					}

					//全局动画速度
					if (!Wpf.SetupLabelBinding(userControl, "SliderShopSpeedRatioTextLabel", "SliderShopSpeedRatioText", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupLabelBinding failed for 'SliderShopSpeedRatioTextLabel'.");
						throw new Exception("The SettingsControl could not be created.");
					}
					if (!Wpf.SetupSliderBinding(userControl, "SliderShopSpeedRatioSlider", "SliderShopSpeedRatio", BindingMode.TwoWay, DefaultBotSettings.Instance))
					{
						ilog_0.DebugFormat("[SettingsControl] SetupSliderBinding failed for 'SliderShopSpeedRatioSlider'.");
						throw new Exception("The SettingsControl could not be created.");
					}


					userControl_0 = userControl;
				}
				return userControl_0;
			}
		}


		//寻找Stopwatch
		private Stopwatch GetStopwatch(string string_0)
		{
			Stopwatch value;
			if (dictionary_0.TryGetValue(string_0, out value))
			{
				return value;
			}
			value = new Stopwatch();
			dictionary_0.Add(string_0, value);
			return value;
		}

		//游戏开局前处理
		private void NewGameEventArgsFunc(object sender, NewGameEventArgs e)
		{
			ilog_0.InfoFormat("[游戏开局前]");
		}

		//游戏开始时处理
		private bool bNeedAutoGreet;
		private void MulliganConfirmEventArgsFunc(object sender, MulliganConfirmEventArgs e)
		{
			ilog_0.InfoFormat("[游戏开始时]");
			if (!bNeedAutoGreet) bNeedAutoGreet = true;
		}

		//游戏结束后处理
		private void GameOverEventArgsFunc(object sender, GameOverEventArgs e)
		{
			ilog_0.InfoFormat("[游戏结束后]");
			if (e.Result == GameOverFlag.Victory)
			{
				if(DefaultBotSettings.Instance.AutoConcedeAfterConstructedWin)
				{
					DefaultBotSettings.Instance.NeedNowConcede = true;
					ilog_0.InfoFormat("[游戏结束后] 已设置下局立即投降，因为我们本局赢了，并且设置了保持排名(赢1投1).");
				}
				else if(DefaultBotSettings.Instance.NormalConcede)
				{
					DefaultBotSettings.Instance.NeedNowConcede = true;
					ilog_0.InfoFormat("[游戏结束后] 已设置下局立即投降，因为我们本局互投赢了，默认下局投降，提高效率.");
				}
				else if (DefaultBotSettings.Instance.ForceConcedeAtMulligan)
				{
					DefaultBotSettings.Instance.NeedNowConcede = true;
					ilog_0.InfoFormat("[游戏结束后] 已设置下局立即投降，因为我们设置了极速投降至互投区.");
				}
			}
			else if(e.Result == GameOverFlag.Defeat)
			{
				if(DefaultBotSettings.Instance.AutoConcedeAfterConstructedWin)
				{
					DefaultBotSettings.Instance.NeedNowConcede = false;
					ilog_0.InfoFormat("[游戏结束后] 已取消下局立即投降，因为我们本局输了，并且设置了保持排名(赢1投1).");
				}
				else if(DefaultBotSettings.Instance.NormalConcede)
				{
					DefaultBotSettings.Instance.NeedNowConcede = false;
					ilog_0.InfoFormat("[游戏结束后] 已取消下局立即投降，因为我们本局互投输了，默认下局等待一会再投降.");
				}
				else if (DefaultBotSettings.Instance.ForceConcedeAtMulligan)
				{
					DefaultBotSettings.Instance.NeedNowConcede = true;
					ilog_0.InfoFormat("[游戏结束后] 已设置下局立即投降，因为我们设置了极速投降至互投区.");
				}
			}
		}

		public bool ShouldAcceptFriendlyChallenge(string name)
		{
			return false;
		}

		//随机点点
		private async Task ClientRandomClick(string scene, int sleepTime)
		{
			int x = Client.Random.Next(Screen.Width / 2, Screen.Width / 2 + 5);
			int y = Client.Random.Next(Screen.Height / 2, Screen.Height - 20);
			ilog_0.DebugFormat("{0} 检测到异常情况，将随机点击(X={1},Y={2})处理...",scene,x,y);
			Client.LeftClickAt(x,y);
			await Coroutine.Sleep(sleepTime);
		}

		//登录界面处理
		private async Task SceneLoginProc(Login login_0)
		{
			ilog_0.InfoFormat("[登录]");

			//异常处理
			PresenceMgr presenceMgr = PresenceMgr.Get();
			if (presenceMgr == null)
			{
				ilog_0.DebugFormat("[登录] 当前场景为空.");
				return;
			}

			//正常处理
			List<MonoEnum> statusEnums = presenceMgr.
				GetStatusEnums(BnetPresenceMgr.Get().GetMyPlayer());
			if (statusEnums != null && statusEnums.Count != 0)
			{
				foreach (MonoEnum item in statusEnums)
				{
					PresenceStatus presenceStatus = item.AsEnum<PresenceStatus>();
					switch (presenceStatus)
					{
						case PresenceStatus.TUTORIAL_PREGAME:
							Client.LeftClickAt(Box.Get().GetEventSpell(BoxEventType.STARTUP_TUTORIAL).Transform.Position);
							await Coroutine.Sleep(3000);
							break;
						default:
							await ClientRandomClick("[登录]", 2500);
							return;
					}
				}
			}
			else
			{
				await ClientRandomClick("[登录]", 2500);
			}
		}

		//主菜单界面处理
		private async Task SceneHubProc(PegasusScene pegasusScene_0)
		{
			bool_2 = false;
			ilog_0.InfoFormat("[主菜单]");

			//异常处理
			PresenceMgr presenceMgr = PresenceMgr.Get();
			if (presenceMgr == null)
			{
				ilog_0.DebugFormat("[主菜单] 当前场景为空.");
				return;
			}

			//正常处理
			List<MonoEnum> statusEnums = presenceMgr.
				GetStatusEnums(BnetPresenceMgr.Get().GetMyPlayer());
			if (statusEnums != null && statusEnums.Count != 0)
			{
				foreach (MonoEnum item in statusEnums)
				{
					PresenceStatus presenceStatus = item.AsEnum<PresenceStatus>();
					TritonHs.smethod_4(presenceMgr, ref presenceStatus);
					switch (presenceStatus)
					{
						case PresenceStatus.HUB:
						case PresenceStatus.QUESTLOG:
						case PresenceStatus.COLLECTION:
							{
								QuestLog questLog = QuestLog.Get();

								//关闭任务界面
								if (questLog != null &&
									ShownUIMgr.Get().m_shownUI == ShownUIMgr.UI_WINDOW.QUEST_LOG)
								{
									ilog_0.InfoFormat("[主菜单] 检测到任务，现在关闭任务界面.");
									Client.LeftClickAt(
										Client.Random.Next(Screen.Width / 2, Screen.Width / 2 + 5),
										Client.Random.Next(Screen.Height / 2, Screen.Height / 2 + 5));
									await Coroutine.Sleep(3000);
									break;
								}

								//正在进行
								if (DefaultBotSettings.Instance.NeedsToCacheCustomDecks)
								{
									ilog_0.Info("[主菜单] 需要缓存卡组，现在点击\"收藏\"按钮.");
									if (!TritonHs.ClickCollectionButton(logReason: true))
									{
										ilog_0.ErrorFormat("[主菜单] 无法点击\"收藏\"按钮.");
										await ClientRandomClick("[主菜单]", 2500);
										break;
									}
									await Coroutine.Sleep(3000);
									return;
								}

								//对战
								ilog_0.Info("[主菜单] 现在点击\"传统对战\"按钮.");
								if (!TritonHs.ClickPlayButton(logReason: true))
								{
									ilog_0.ErrorFormat("[主菜单] 无法点击\"传统对战\"按钮.");
									await ClientRandomClick("[主菜单]", 2500);
									break;
								}
								await Coroutine.Sleep(3000);
								return;
							}
						case PresenceStatus.STORE:
							{
								if (StoreManager.Get() != null)
								{
									Client.LeftClickAtDialog(
										BnetBar.Get().m_currentTime.Transform.Position);
									await Coroutine.Sleep(1000);
									break;
								}
								return;
							}
						default:
							await ClientRandomClick("[主菜单]", 2500);
							return;
					}
				}
			}
			else
			{
				await ClientRandomClick("[主菜单]", 2500);
			}
		}

		//我的收藏界面-卡组缓存
		private async Task CollectionCard(CollectionManagerScene collectionManagerScene_0)
		{
			ilog_0.InfoFormat("[卡组缓存]");

			//异常处理
			CollectionDeckTray collectionDeckTray = CollectionDeckTray.Get();
			if (collectionDeckTray == null)
			{
				ilog_0.DebugFormat("[卡组缓存] 当前场景为空.");
				await Coroutine.Sleep(Client.Random.Next(1000, 2000));
				return;
			}

			//正常处理
			CollectionManager collectionManager = CollectionManager.Get();
			if (collectionManager != null && collectionManager.IsFullyLoaded())
			{
				Stopwatch stopwatch = GetStopwatch("CollectionManagerScene_COLLECTION");
				if (stopwatch.IsRunning && stopwatch.ElapsedMilliseconds >= 1000)
				{
					TraySection editingTraySection = collectionDeckTray.m_decksContent.m_editingTraySection;
					CollectionDeck taggedDeck = CollectionManager.Get().GetTaggedDeck(CollectionManager.DeckTag.Editing);
					if (taggedDeck != null && editingTraySection != null)
					{
						if (!collectionManager.GetDeck(taggedDeck.ID).NetworkContentsLoaded())
						{
							ilog_0.DebugFormat("[卡组缓存] !m_netContentsLoaded.");
							await Coroutine.Sleep(Client.Random.Next(1000, 2000));
							stopwatch.Reset();
						}
						else
						{
							ilog_0.InfoFormat("[卡组缓存] 卡组内卡牌已全部选择完成, 现在点击\"完成\"按钮.");
							Client.LeftClickAt(collectionDeckTray.m_doneButton.m_ButtonText.Transform.Position);
							await Coroutine.Sleep(Client.Random.Next(1000, 2000));
							stopwatch.Reset();
						}
						return;
					}
					if (!DefaultBotSettings.Instance.NeedsToCacheCustomDecks)
					{
						ilog_0.InfoFormat("[卡组缓存] 缓存卡组已完成，现在离开\"收藏\"界面.");
						Client.LeftClickAt(collectionDeckTray.m_doneButton.m_ButtonText.Transform.Position);
						await Coroutine.Sleep(Client.Random.Next(1000, 2000));
						stopwatch.Reset();
						return;
					}
					try
					{
						Utility.smethod_4();
					}
					catch (Exception arg)
					{
						ilog_0.ErrorFormat("[卡组缓存] 未知异常{0}，无法处理.", arg);
						return;
					}
					List<CollectionDeckBoxVisual> list = new List<CollectionDeckBoxVisual>();
					foreach (TraySection traySection in collectionDeckTray.m_decksContent.m_traySections)
					{
						CollectionDeckBoxVisual deckBox = traySection.m_deckBox;
						long deckID = deckBox.GetDeckID();
						string text = deckBox.m_deckName.Text;
						FormatType formatType = deckBox.m_formatType;
						if (deckID == -1 || deckBox.IsLocked() || !deckBox.IsEnabled())
						{
							continue;
						}
						if (!collectionManager.GetDeck(deckID).m_netContentsLoaded)
						{
							if (Utility.smethod_2(deckID, text, formatType))
							{
								ilog_0.InfoFormat("[卡组缓存] 保存卡组...");
								list.Add(deckBox);
							}
						}
						else
						{
							ilog_0.InfoFormat("[卡组缓存] 卡组之前已保存.");
						}
					}
					if (list.Any())
					{
						ilog_0.InfoFormat("[卡组缓存] 随机选择1个卡组...");
						list.ElementAt(Client.Random.Next(0, list.Count)).TriggerTap();
						await Coroutine.Sleep(Client.Random.Next(1000, 2000));
						stopwatch.Reset();
						return;
					}
					List<CustomDeckCache> list2 = new List<CustomDeckCache>();
					foreach (CustomDeckCache customDeck in MainSettings.Instance.CustomDecks)
					{
						if (collectionManager.GetDeck(customDeck.DeckId) == null)
						{
							list2.Add(customDeck);
						}
					}
					if (list2.Any())
					{
						ilog_0.DebugFormat("[卡组缓存] 现在删除[{0}]个不存在的卡组.", list2.Count);
						foreach (CustomDeckCache item in list2)
						{
							MainSettings.Instance.CustomDecks.Remove(item);
							try
							{
								File.Delete(CustomDeckCache.GetFileNameFor(item.DeckId));
							}
							catch (Exception arg)
							{
								ilog_0.ErrorFormat("[卡组缓存] 未知异常{0}，无法处理.", arg);
							}
						}
					}
					MainSettings.Instance.LastDeckCachePid = TritonHs.Memory.Process.Id;
					MainSettings.Instance.Save();
					DefaultBotSettings.Instance.NeedsToCacheCustomDecks = false;
					GameEventManager.Instance.method_8();
					stopwatch.Reset();
				}
				else
				{
					if (!stopwatch.IsRunning)
					{
						stopwatch.Restart();
					}
					ilog_0.DebugFormat("[卡组缓存] 正在处理，请稍等.");
					await Coroutine.Sleep(Client.Random.Next(1000, 2000));
				}
			}
			else
			{
				ilog_0.DebugFormat("[卡组缓存] 界面未完成加载成功，请稍等.");
				await Coroutine.Sleep(Client.Random.Next(1000, 2000));
			}
		}

		//我的收藏界面
		private async Task SceneCollectionManagerProc(CollectionManagerScene collectionManagerScene_0)
		{
			ilog_0.InfoFormat("[我的收藏]");

			//异常处理
			PresenceMgr presenceMgr = PresenceMgr.Get();
			if (presenceMgr == null)
			{
				ilog_0.DebugFormat("[我的收藏] 当前场景为空.");
				return;
			}

			//正常处理
			List<MonoEnum> statusEnums = presenceMgr.
				GetStatusEnums(BnetPresenceMgr.Get().GetMyPlayer());
			if (statusEnums != null && statusEnums.Count != 0)
			{
				foreach (MonoEnum item in statusEnums)
				{
					PresenceStatus presenceStatus = item.AsEnum<PresenceStatus>();
					TritonHs.smethod_4(presenceMgr, ref presenceStatus);
					switch (presenceStatus)
					{
						case PresenceStatus.COLLECTION:
						case PresenceStatus.DECKEDITOR:
							await CollectionCard(collectionManagerScene_0);
							continue;
						default:
							await ClientRandomClick("[我的收藏]", 2500);
							return;
					}
				}
			}
			else
			{
				await ClientRandomClick("[我的收藏]", 2500);
			}
		}

		//卡组选择
		private static async Task<bool> DeckPickerMethod1(HeroPickerButton heroPickerButton_0, List<HeroPickerButton> list_1, string string_1)
		{
			CustomDeckCache customDeckCache = MainSettings.Instance.CustomDecks.FirstOrDefault((CustomDeckCache customDeckCache_0) => customDeckCache_0.Name.Equals(string_1, StringComparison.OrdinalIgnoreCase));
			if (heroPickerButton_0 != null && heroPickerButton_0.m_classLabel.Text.Equals(string_1, StringComparison.OrdinalIgnoreCase))
			{
				if (heroPickerButton_0.m_locked || !heroPickerButton_0.m_isDeckValid)
				{
					ilog_0.ErrorFormat("[卡组选择] 卡组\"{0}\"无法选中，可能是被锁定或者无效卡组.", string_1);
					BotManager.Stop();
					await Coroutine.Yield();
					return false;
				}
			}
			else
			{
				bool flag = false;
				foreach (HeroPickerButton item in list_1)
				{
					if (!item.m_locked && item.m_isDeckValid)
					{
						if (item.m_classLabel.Text.Equals(string_1, StringComparison.OrdinalIgnoreCase))
						{
							heroPickerButton_0 = item;
							flag = true;
							break;
						}
						if (customDeckCache != null && customDeckCache.HeroCardId.Equals(item.m_fullDef.m_entityDef.m_cardId, StringComparison.OrdinalIgnoreCase))
						{
							heroPickerButton_0 = item;
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					ilog_0.ErrorFormat("[卡组选择] 没有任何可用卡组.");
					BotManager.Stop();
					await Coroutine.Yield();
					return false;
				}
				if (heroPickerButton_0.m_locked || !heroPickerButton_0.m_isDeckValid)
				{
					ilog_0.ErrorFormat("[卡组选择] 卡组\"{0}\"无法选中，可能是被锁定或者无效卡组.", string_1);
					BotManager.Stop();
					await Coroutine.Yield();
					return false;
				}
				Vector3 position = heroPickerButton_0.Transform.Position;
				Client.MouseOver(position);
				HighlightState highlightState = heroPickerButton_0.m_highlightState;
				if (highlightState == null || (highlightState.m_CurrentState != ActorStateType.HIGHLIGHT_MOUSE_OVER && highlightState.m_CurrentState != ActorStateType.HIGHLIGHT_PRIMARY_ACTIVE))
				{
					ilog_0.ErrorFormat("[卡组选择] 卡组\"{0}\"按钮不高亮，无法选中.", string_1);
					return false;
				}
				Client.LeftClickAt(position);
				await Coroutine.Sleep(3000);
			}
			DefaultBotSettings.Instance.LastDeckId = heroPickerButton_0.m_preconDeckID;
			return true;
		}

		//卡组选择
		private static async Task<bool> DeckPickerMethod2(List<CustomDeckPage> list_1, 
			DeckPickerTrayDisplay deckPickerTrayDisplay_0, string string_1)
		{
			CollectionDeckBoxVisual collectionDeckBoxVisual = null;
			if (list_1 != null && list_1.Count > 0)
			{
				for (int i = 0; i < list_1.Count; i++)
				{
					collectionDeckBoxVisual = list_1[i].m_customDecks.FirstOrDefault((CollectionDeckBoxVisual x) => x.m_deckName.Text.Equals(string_1));
					if (collectionDeckBoxVisual != null)
					{
						ilog_0.DebugFormat("[卡组选择] 在第{0}页找到卡组\"{1}\"，现在切换到页面{2}.",
							 i + 1, string_1, i + 1);
						deckPickerTrayDisplay_0.ShowPage(i);
						break;
					}
				}
			}
			bool result;
			if (collectionDeckBoxVisual == null)
			{
				ilog_0.ErrorFormat("[卡组选择] 未找到卡组\"{0}\".", string_1);
				BotManager.Stop();
				await Coroutine.Yield();
				result = false;
			}
			else if (collectionDeckBoxVisual.IsValid() && !collectionDeckBoxVisual.IsLocked())
			{
				Vector3 position = collectionDeckBoxVisual.Transform.Position;
				Client.MouseOver(position);
				HighlightState highlightState = collectionDeckBoxVisual.m_highlightState;
				if (highlightState != null && (highlightState.m_CurrentState == ActorStateType.HIGHLIGHT_MOUSE_OVER || highlightState.m_CurrentState == ActorStateType.HIGHLIGHT_PRIMARY_ACTIVE))
				{
					Client.LeftClickAt(position);
					DefaultBotSettings.Instance.LastDeckId = collectionDeckBoxVisual.m_deckID;
					await Coroutine.Sleep(3000);
					ilog_0.DebugFormat("[卡组选择] 已选中卡组\"{0}\".", string_1);
					return true;
				}
				ilog_0.ErrorFormat("[卡组选择] 卡组\"{0}\"按钮不高亮，无法选中.", string_1);
				result = false;
			}
			else
			{
				ilog_0.ErrorFormat("[卡组选择] 卡组\"{0}\"无法选中，可能是被锁定或者无效卡组.", string_1);
				BotManager.Stop();
				await Coroutine.Yield();
				result = false;
			}
			return result;
		}

		//卡组选择返回按钮
		private async Task ClickBackButton(TournamentScene tournamentScene_0)
		{
			DeckPickerTrayDisplay deckPickerTrayDisplay = DeckPickerTrayDisplay.Get();
			if (deckPickerTrayDisplay == null)
			{
				ilog_0.DebugFormat("[卡组选择] 场景异常.");
				return;
			}
			Client.LeftClickAt(deckPickerTrayDisplay.m_backButton.m_ButtonText.Transform.Position);
			await Coroutine.Sleep(2500);
		}

		//卡组选择界面
		private async Task DeckPickerProc(TournamentScene tournamentScene_0)
		{
			GameEventManager.Instance.method_6();
			await Coroutine.Yield();
			if (DefaultBotSettings.Instance.NeedsToCacheCustomDecks)
			{
				ilog_0.InfoFormat("[卡组选择] 返回主菜单去缓存卡组.");
				await ClickBackButton(tournamentScene_0);
				return;
			}
			GameEventManager.Instance.method_7();
			await Coroutine.Yield();

			//异常处理
			DeckPickerTrayDisplay deckPickerTrayDisplay = DeckPickerTrayDisplay.Get();
			if (deckPickerTrayDisplay == null)
			{
				ilog_0.DebugFormat("[卡组选择] 场景为空.");
				return;
			}
			if (!deckPickerTrayDisplay.IsLoaded())
			{
				ilog_0.DebugFormat("[卡组选择] 界面未加载成功.");
				return;
			}
			if (string.IsNullOrEmpty(DefaultBotSettings.Instance.ConstructedCustomDeck))
			{
				ilog_0.ErrorFormat("[卡组选择] 请先设置对战卡组名称.");
				BotManager.Stop();
				await Coroutine.Yield();
				return;
			}

			//正常处理
			FormatType formatType = Options.Get().GetFormatType();
			bool inRankedPlayMode = Options.Get().GetInRankedPlayMode();
			bool flag = false;
			if (DefaultBotSettings.Instance.ConstructedGameRule == VisualsFormatType.休闲)
			{
				if (inRankedPlayMode)
				{
					flag = true;
					ilog_0.InfoFormat("[卡组选择] 切换到\"休闲\"模式.");
				}
			}
			else if (DefaultBotSettings.Instance.ConstructedGameRule == VisualsFormatType.狂野)
			{
				if (formatType != FormatType.FT_WILD || !inRankedPlayMode)
				{
					flag = true;
					ilog_0.InfoFormat("[卡组选择] 切换到\"狂野\"模式.");
				}
			}
			else if (DefaultBotSettings.Instance.ConstructedGameRule == VisualsFormatType.标准)
			{
				if (formatType != FormatType.FT_STANDARD || !inRankedPlayMode)
				{
					flag = true;
					ilog_0.InfoFormat("[卡组选择] 切换到\"标准\"模式.");
				}
			}
			else if (DefaultBotSettings.Instance.ConstructedGameRule == VisualsFormatType.幻变)
			{
				if (formatType != FormatType.FT_TWIST || !inRankedPlayMode)
				{
					flag = true;
					ilog_0.InfoFormat("[卡组选择] 切换到\"幻变\"模式.");
				}
			}
			else if (DefaultBotSettings.Instance.ConstructedGameRule == VisualsFormatType.经典)
			{
				ilog_0.ErrorFormat("[卡组选择] 经典模式已经GG, 暴雪SGDSB！");
				BotManager.Stop();
				await Coroutine.Yield();
				return;
			}
			else
			{
				ilog_0.ErrorFormat("[卡组选择] 无法处理的对战模式:{0}.", DefaultBotSettings.Instance.ConstructedGameRule);
				BotManager.Stop();
				await Coroutine.Yield();
				return;
			}
			if (flag)
			{
				deckPickerTrayDisplay.SwitchFormatTypeAndRankedPlayMode(DefaultBotSettings.Instance.ConstructedGameRule);
				await Coroutine.Sleep(1500);
				return;
			}

			//卡组处理
			List<CustomDeckPage> customPages = deckPickerTrayDisplay.m_customPages;
			if (customPages != null)
			{
				foreach (CustomDeckPage item in customPages)
				{
					if (!item.AreAllCustomDecksReady())
					{
						ilog_0.DebugFormat("[卡组选择] 卡组未准备好.");
						await Coroutine.Sleep(1000);
						return;
					}
				}
				ilog_0.InfoFormat("[卡组选择] 准备选择设置的卡组...");
				if (!(await DeckPickerMethod2(customPages, deckPickerTrayDisplay,
					DefaultBotSettings.Instance.ConstructedCustomDeck)))
				{
					ilog_0.DebugFormat("[卡组选择] 卡组选择失败.");
					return;
				}
			}
			else
			{
				ilog_0.InfoFormat("[卡组选择] 准备选择设置的卡组...");
				if (!(await DeckPickerMethod1(deckPickerTrayDisplay.m_selectedHeroButton,
					deckPickerTrayDisplay.m_heroButtons, 
					DefaultBotSettings.Instance.ConstructedCustomDeck)))
				{
					ilog_0.DebugFormat("[卡组选择] 卡组选择失败.");
					return;
				}
			}
			if (deckPickerTrayDisplay.m_rankedPlayDisplay == null)
			{
				ilog_0.DebugFormat("[卡组选择] 开始按钮为空.");
				return;
			}
			TransitionPopup transitionPopup = GameMgr.Get().m_transitionPopup;
			if (transitionPopup != null && transitionPopup.IsShown())
			{
				ilog_0.InfoFormat("[卡组选择] \"匹配\"界面显示正常.");
				await Coroutine.Sleep(1000);
				return;
			}
			PlayButton playButton = deckPickerTrayDisplay.m_playButton;
			UberText newPlayButtonText = playButton.m_newPlayButtonText;
			if (!playButton.IsEnabled())
			{
				ilog_0.InfoFormat("[卡组选择] \"{0}\"按钮无法点击.", newPlayButtonText.Text);
				return;
			}
			UberText newPlayButtonText2 = playButton.m_newPlayButtonText;
			Vector3 center = newPlayButtonText2.m_TextMeshGameObject.Renderer.Bounds.m_Center;
			ilog_0.InfoFormat("[卡组选择] 现在点击\"{0}\"按钮.", newPlayButtonText2.Text);
			Client.LeftClickAt(center);
			await Coroutine.Sleep(3000);
		}

		//正在匹配界面
		private async Task PlayQueueProc(TournamentScene tournamentScene_0)
		{
			if (DeckPickerTrayDisplay.Get() == null)
			{
				ilog_0.DebugFormat("[正在匹配] 场景异常.");
				return;
			}
			await Coroutine.Sleep(1000);
			TransitionPopup transitionPopup = GameMgr.Get().m_transitionPopup;
			if (transitionPopup != null && transitionPopup.IsShown())
			{
				if (DefaultBotSettings.Instance.NeedsToCacheCustomDecks)
				{
					ilog_0.InfoFormat("[正在匹配] 现在返回主菜单缓存卡组.");
					UIBButton cancelButton = transitionPopup.m_cancelButton;
					if (cancelButton.IsEnabled())
					{
						ilog_0.InfoFormat("[正在匹配] 正在取消匹配...");
						Client.LeftClickAt(cancelButton.m_RootObject.Transform.Position);
						await Coroutine.Sleep(1000);
					}
					else
					{
						ilog_0.InfoFormat("[正在匹配] 无法取消.");
						await Coroutine.Sleep(1000);
					}
				}
				else
				{
					ilog_0.InfoFormat("[正在匹配] 请稍等...");
					await Coroutine.Sleep(1000);
				}
			}
			else
			{
				ilog_0.InfoFormat("[正在匹配] \"匹配\"界面显示异常.");
				await ClickBackButton(tournamentScene_0);
			}
		}

		//卡组选择界面
		private async Task SceneTournamentProc(TournamentScene tournamentScene_0)
		{
			//异常处理
			if (!tournamentScene_0.IsLoaded())
			{
				ilog_0.DebugFormat("[卡组选择] 场景未加载成功.");
				return;
			}
			if (!tournamentScene_0.m_deckPickerIsLoaded)
			{
				ilog_0.DebugFormat("[卡组选择] 卡组未锁定.");
				return;
			}
			PresenceMgr presenceMgr = PresenceMgr.Get();
			if (presenceMgr == null)
			{
				ilog_0.DebugFormat("[卡组选择] 当前场景为空.");
				return;
			}

			//正常处理
			List<MonoEnum> statusEnums = presenceMgr.
				GetStatusEnums(BnetPresenceMgr.Get().GetMyPlayer());
			if (statusEnums != null && statusEnums.Count != 0)
			{
				foreach (MonoEnum item in statusEnums)
				{
					PresenceStatus presenceStatus = item.AsEnum<PresenceStatus>();
					TritonHs.smethod_4(presenceMgr, ref presenceStatus);
					switch (presenceStatus)
					{
						case PresenceStatus.PLAY_DECKPICKER:
							await DeckPickerProc(tournamentScene_0);
							break;
						case PresenceStatus.PLAY_QUEUE:
							await PlayQueueProc(tournamentScene_0);
							break;
						case PresenceStatus.HUB:
							ilog_0.DebugFormat("[卡组选择] 已切换到{0}，请稍等.", presenceStatus);
							await Coroutine.Sleep(5000);
							break;
						default:
							await ClientRandomClick("[卡组选择]", 2500);
							return;
					}
				}
			}
			else
			{
				await ClientRandomClick("[卡组选择]", 2500);
			}
		}

		//对战准备时
		private async Task GameplaySceneBeginMulligan(PegasusScene pegasusScene_0, bool bool_5 = false)
		{
			if (bool_5)
			{
				await Coroutine.Sleep(1000);
				return;
			}
			Stopwatch stopwatch = GetStopwatch("GameplayScene_BEGIN_MULLIGAN");
			MulliganManager mulliganManager = MulliganManager.Get();
			if (mulliganManager == null)
			{
				ilog_0.DebugFormat("[对战准备] 场景为空.");
				stopwatch.Reset();
				return;
			}
			if (!mulliganManager.IsMulliganActive())
			{
				ilog_0.DebugFormat("[对战准备] 场景未激活.");
				stopwatch.Reset();
				return;
			}
			if (!mulliganManager.introComplete)
			{
				ilog_0.DebugFormat("[对战准备] 准备工作未完成.");
				stopwatch.Reset();
				return;
			}
			if (!mulliganManager.m_waitingForUserInput)
			{
				ilog_0.DebugFormat("[对战准备] 请稍等...");
				stopwatch.Reset();
				return;
			}
			NormalButton mulliganButton = mulliganManager.GetMulliganButton();
			if (mulliganButton == null)
			{
				ilog_0.DebugFormat("[对战准备] 按钮为空.");
				stopwatch.Reset();
			}
			else if (stopwatch.IsRunning && stopwatch.ElapsedMilliseconds >= 3000L)
			{
				stopwatch.Reset();

				//写日志
				await RoutineManager.CurrentRoutine.Logic("new_game", null);
				await Coroutine.Yield();

				//立即投降
				if (DefaultBotSettings.Instance.NeedNowConcede)
				{
					ilog_0.InfoFormat("[对战准备] 立即投降开始...");
					TritonHs.Concede(logReason: true);
					bool_4 = true;
					await Coroutine.Sleep(1000);
					return;
				}
				Vector3 position = mulliganButton.Transform.Position;
				MulliganData mulliganData = new MulliganData();
				await RoutineManager.CurrentRoutine.Logic("mulligan", mulliganData);
				await Coroutine.Yield();
				List<bool> handCardsMarkedForReplace = mulliganManager.m_handCardsMarkedForReplace;
				for (int i = 0; i < mulliganData.Cards.Count; i++)
				{
					Game.Abstraction.Card card = mulliganData.Cards[i];
					if (mulliganData.Mulligans[i])
					{
						if (!handCardsMarkedForReplace[i])
						{
							ilog_0.InfoFormat("[对战准备] 弃掉 {0}.", card.Entity.Id);
							Client.LeftClickAt(card.InteractPoint);
							await Coroutine.Sleep(1000);
						}
					}
					else if (handCardsMarkedForReplace[i])
					{
						ilog_0.InfoFormat("[对战准备] 保留 {0}.", card.Entity.Id);
						Client.LeftClickAt(card.InteractPoint);
						await Coroutine.Sleep(1000);
					}
				}
				bool flag = true;
				handCardsMarkedForReplace = mulliganManager.m_handCardsMarkedForReplace;
				for (int j = 0; j < mulliganData.Cards.Count; j++)
				{
					if (mulliganData.Mulligans[j])
					{
						if (!handCardsMarkedForReplace[j])
						{
							flag = false;
							break;
						}
					}
					else if (handCardsMarkedForReplace[j])
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					ilog_0.InfoFormat("[对战准备] 准备完成.");
				}
				else
				{
					ilog_0.ErrorFormat("[对战准备] 准备完成.");
				}
				GameEventManager.Instance.method_1();
				Client.LeftClickAt(position);
				await Coroutine.Sleep(2500);
			}
			else
			{
				if (!stopwatch.IsRunning)
				{
					stopwatch.Restart();
				}
				ilog_0.DebugFormat("[对战准备] 请稍等...");
				await Coroutine.Sleep(1000);
			}
		}

		//开局自动打招呼
		private async Task SayHello()
		{
			if (emoteType_0 == EmoteType.INVALID)
			{
				return;
			}
			DateTime now = DateTime.Now;
			if (now < dateTime_0 || (now - dateTime_1).TotalMilliseconds < 5000.0)
			{
				return;
			}
			EmoteHandler emoteHandler;
			while (true)
			{
				emoteHandler = EmoteHandler.Get();
				if (emoteHandler.m_emotesShown)
				{
					break;
				}
				ilog_0.InfoFormat("[自动打招呼] 表情未显示，先弹出表情选项.");
				Client.RightClickAt(GameState.Get().GetFriendlySidePlayer().GetHero()
					.m_card.m_actor.m_meshRenderer.Bounds.m_Center);
				await Coroutine.Sleep(1000);
			}
			ilog_0.InfoFormat("[自动打招呼] 现在选择表情 {0}.", emoteType_0);
			if (!emoteHandler.EmoteSpamBlocked())
			{
				bool flag = false;
				foreach (EmoteOption availableEmote in emoteHandler.m_availableEmotes)
				{
					bool flag2 = false;
					if (availableEmote.m_EmoteType == emoteType_0)
					{
						flag2 = true;
					}
					if (emoteType_0 == EmoteType.GREETINGS && availableEmote.m_EmoteType == EmoteType.HALLOWS_END)
					{
						flag2 = true;
					}
					if (flag2)
					{
						string text = availableEmote.m_Text.Text;
						UberText text2 = availableEmote.m_Text;
						ilog_0.InfoFormat("[自动打招呼] 现在使用表情 {0}.", text);
						Client.LeftClickAt(text2.Transform.Position);
						dateTime_1 = DateTime.Now;
						emoteType_0 = EmoteType.INVALID;
						await Coroutine.Sleep(1000);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					ilog_0.DebugFormat("[自动打招呼] {0}表情不存在，请稍等.", emoteType_0);
					dateTime_1 = DateTime.Now + TimeSpan.FromMinutes(1.0);
					emoteType_0 = EmoteType.INVALID;
				}
			}
			else
			{
				ilog_0.DebugFormat("[自动打招呼] 表情无效，请稍等.");
				dateTime_1 = DateTime.Now + TimeSpan.FromMinutes(1.0);
				emoteType_0 = EmoteType.INVALID;
			}
		}

		//战斗中界面
		private async Task GameplaySceneMainAction(PegasusScene pegasusScene_0, bool bool_5 = false)
		{
			if (bool_5)
			{
				await Coroutine.Sleep(1000);
				return;
			}
			if (bNeedAutoGreet && DefaultBotSettings.Instance.AutoGreet)
			{
				ilog_0.InfoFormat("[战斗中] 准备自动打招呼...");
				emoteType_0 = EmoteType.GREETINGS;
				dateTime_0 = DateTime.Now + TimeSpan.FromSeconds(Client.Random.Next(3, 5));
				bNeedAutoGreet = false;
			}
			await SayHello();
			Stopwatch stopwatch = GetStopwatch("GameplayScene_MAIN_ACTION_Friendly");
			Stopwatch stopwatch2 = GetStopwatch("GameplayScene_MAIN_ACTION_Opponent");
			if (DefaultBotSettings.Instance.NormalConcede)
			{
				if (!stopwatch_0.IsRunning)
				{
					stopwatch_0.Start();
					int_0 = Client.Random.Next(DefaultBotSettings.Instance.AutoConcedeMinDelayMs, DefaultBotSettings.Instance.AutoConcedeMaxDelayMs);
					ilog_0.InfoFormat("[战斗中] {0} ms后准备投降.", int_0);
				}
				if (stopwatch_0.ElapsedMilliseconds > int_0)
				{
					if (TritonHs.Concede(logReason: true))
					{
						bool_4 = true;
						stopwatch_0.Reset();
						await Coroutine.Sleep(1000);
						return;
					}
				}
				else
				{
					ilog_0.InfoFormat("[战斗中] {0} ms后准备投降.", int_0 - stopwatch_0.ElapsedMilliseconds);
				}
			}
			if (GameState.Get().IsFriendlySidePlayerTurn())
			{
				stopwatch2.Reset();
				EndTurnButton endTurnButton = EndTurnButton.Get();
				if (endTurnButton != null && !endTurnButton.IsInWaitingState())
				{
					if (!stopwatch.IsRunning || stopwatch.ElapsedMilliseconds < 3000L)
					{
						if (!stopwatch.IsRunning)
						{
							stopwatch.Restart();
						}
						ilog_0.DebugFormat("[战斗中] 请稍等...");
					}
					else
					{
						await RoutineManager.CurrentRoutine.Logic("our_turn", null);
						await Coroutine.Sleep(500);
					}
				}
				else
				{
					stopwatch.Reset();
					ilog_0.InfoFormat("[战斗中] 准备开始/结束回合.");
					await Coroutine.Sleep(250);
				}
			}
			else
			{
				stopwatch.Reset();
				if (!stopwatch2.IsRunning)
				{
					stopwatch2.Restart();
				}
				await RoutineManager.CurrentRoutine.Logic("opponent_turn", null);
				await Coroutine.Yield();
			}
		}

		//战斗中界面
		private async Task GameplaySceneMainCombat(PegasusScene pegasusScene_0, bool bool_5 = false)
		{
			if (bool_5)
			{
				await Coroutine.Sleep(1000);
				return;
			}
			Stopwatch stopwatch = GetStopwatch("GameplayScene_MAIN_COMBAT_Friendly");
			Stopwatch stopwatch2 = GetStopwatch("GameplayScene_MAIN_COMBAT_Opponent");
			if (GameState.Get().IsFriendlySidePlayerTurn())
			{
				stopwatch2.Reset();
				if (!stopwatch.IsRunning || stopwatch.ElapsedMilliseconds < 1000L)
				{
					if (!stopwatch.IsRunning)
					{
						stopwatch.Restart();
					}
					ilog_0.DebugFormat("[战斗中] 请稍等...");
				}
				else
				{
					await RoutineManager.CurrentRoutine.Logic("our_turn_combat", null);
					await Coroutine.Sleep(500);
				}
			}
			else
			{
				stopwatch.Reset();
				if (!stopwatch2.IsRunning)
				{
					stopwatch2.Restart();
				}
				await RoutineManager.CurrentRoutine.Logic("opponent_turn_combat", null);
				await Coroutine.Yield();
			}
		}

		//战斗结束界面
		private async Task GameplaySceneGameOver(PegasusScene pegasusScene_0, bool bool_5 = false)
		{
			Stopwatch stopwatch = GetStopwatch("GameplayScene_FINAL_GAMEOVER");
			EndGameScreen endGameScreen = EndGameScreen.Get();
			if (endGameScreen == null)
			{
				stopwatch.Reset();
				ilog_0.DebugFormat("[战斗结束] 场景为空.");
			}
			else if (!endGameScreen.m_shown)
			{
				stopwatch.Reset();
				ilog_0.DebugFormat("[战斗结束] 结束界面未显示.");
			}
			else if (stopwatch.IsRunning && stopwatch.ElapsedMilliseconds >= 3000L)
			{
				string realClassName = endGameScreen.RealClassName;
				if (!bool_5)
				{
					if (realClassName == "VictoryScreen")
					{
						GameEventManager.Instance.method_5(GameOverFlag.Victory, bool_0: false);
					}
					else if (realClassName == "DefeatScreen")
					{
						bool flag = false;
						if (bool_4)
						{
							flag = true;
							bool_4 = false;
						}
						GameEventManager.Instance.method_5(GameOverFlag.Defeat, flag);
					}
				}
				await Coroutine.Yield();
				ilog_0.InfoFormat("[战斗结束] 本局 [{0}]，点击继续...",
					realClassName == "DefeatScreen"?"失败":"胜利");
				Client.LeftClickAt(endGameScreen.m_continueText.Transform.Position);
				await Coroutine.Sleep(2500);
				stopwatch.Reset();
			}
			else
			{
				if (!stopwatch.IsRunning)
				{
					stopwatch.Restart();
				}
				ilog_0.DebugFormat("[战斗结束] 请稍等...");
				await Coroutine.Sleep(1000);
			}
		}

		//战斗界面处理
		private async Task GamePlayAllProc(PegasusScene pegasusScene_0)
		{
			GameEntity gameEntity = GameState.Get().GetGameEntity();
			TAG_STEP tAG_STEP = (TAG_STEP)gameEntity.GetTag(GAME_TAG.STEP);
			if (GameState.Get().IsGameOver() && tAG_STEP == TAG_STEP.MAIN_ACTION)
			{
				TAG_STEP tAG_STEP2 = tAG_STEP;
				tAG_STEP = TAG_STEP.FINAL_GAMEOVER;
				ilog_0.InfoFormat("[游戏战斗] 修正状态：{0}=>{1}.",tAG_STEP2, tAG_STEP);
			}
			switch (tAG_STEP)
			{
				default:
					ilog_0.InfoFormat("[游戏战斗] {0}.", tAG_STEP);
					break;
				case TAG_STEP.BEGIN_MULLIGAN:
					await GameplaySceneBeginMulligan(pegasusScene_0);
					break;
				case TAG_STEP.MAIN_ACTION:
					await GameplaySceneMainAction(pegasusScene_0);
					break;
				case TAG_STEP.MAIN_COMBAT:
					await GameplaySceneMainCombat(pegasusScene_0);
					break;
				case TAG_STEP.FINAL_WRAPUP:
				case TAG_STEP.FINAL_GAMEOVER:
					await GameplaySceneGameOver(pegasusScene_0);
					break;
			}
		}

		//游戏战斗界面
		private async Task SceneGamePlayProc(Gameplay gameplay_0)
		{
			//异常处理
			if (!gameplay_0.AreCriticalAssetsLoaded())
			{
				ilog_0.DebugFormat("[游戏战斗] 场景未加载成功.");
				return;
			}
			PresenceMgr presenceMgr = PresenceMgr.Get();
			if (presenceMgr == null)
			{
				ilog_0.DebugFormat("[游戏战斗] 当前场景为空.");
				return;
			}

			//正常处理
			List<MonoEnum> statusEnums = presenceMgr.
				GetStatusEnums(BnetPresenceMgr.Get().GetMyPlayer());
			if (statusEnums != null && statusEnums.Count != 0)
			{
				int num = 0;
				if (num < statusEnums.Count)
				{
					PresenceStatus presenceStatus = statusEnums[num].AsEnum<PresenceStatus>();
					TritonHs.smethod_4(presenceMgr, ref presenceStatus);
					if (presenceStatus == PresenceStatus.PLAY_QUEUE)
					{
						presenceStatus = PresenceStatus.PLAY_GAME;
					}
					GameEventManager.Instance.method_0(presenceStatus);
					switch (presenceStatus)
					{
						case PresenceStatus.PLAY_GAME:
						case PresenceStatus.PLAY_RANKED_STANDARD:
						case PresenceStatus.PLAY_RANKED_WILD:
						case PresenceStatus.PLAY_RANKED_CLASSIC:
						case PresenceStatus.PLAY_RANKED_TWIST:
						case PresenceStatus.PLAY_CASUAL_STANDARD:
						case PresenceStatus.PLAY_CASUAL_WILD:
						case PresenceStatus.PLAY_CASUAL_CLASSIC:
						case PresenceStatus.PLAY_CASUAL_TWIST:
						case PresenceStatus.PRACTICE_GAME:
						case PresenceStatus.TUTORIAL_GAME:
							await GamePlayAllProc(gameplay_0);
							return;
						default:
							await ClientRandomClick(string.Format("[游戏战斗][{0}]", presenceStatus), 2500);
							return;
					}
				}
			}
			else
			{
				await ClientRandomClick("[游戏战斗]", 2500);
			}
		}

		//关闭任务界面
		private async Task<bool> CloseQuestScreen()
		{
			WelcomeQuests welcomeQuests = WelcomeQuests.Get();
			if (welcomeQuests == null)
			{
				return false;
			}
			if (!welcomeQuests.m_headlineBanner.GameObject.Active)
			{
				return false;
			}
			await Coroutine.Yield();
			ilog_0.InfoFormat("[任务] 现在关闭任务界面.");
			welcomeQuests.m_clickCatcher.TriggerPress();
			welcomeQuests.m_clickCatcher.TriggerRelease();
			await Coroutine.Sleep(2500);
			return true;
		}

		//场景处理
		private async Task SceneAllProc()
		{
			while (true)
			{
				//HearthstoneApplication.s_mode = ApplicationMode.INTERNAL;
				await Coroutine.Yield();
				PegasusScene scene = SceneMgr.Get().GetScene();
				SceneMgr.Mode mode = SceneMgr.Get().GetMode();
				GameType gameType = GameMgr.Get().GetGameType();
				GameState gameState = GameState.Get();
				if (!(await CloseQuestScreen())) //关闭任务界面+广告界面
				{
					switch (mode)
					{
						case SceneMgr.Mode.LETTUCE_BOUNTY_BOARD:
						case SceneMgr.Mode.LETTUCE_COLLECTION:
						case SceneMgr.Mode.LETTUCE_BOUNTY_TEAM_SELECT:
						case SceneMgr.Mode.LETTUCE_MAP:
						case SceneMgr.Mode.LETTUCE_VILLAGE:
						case SceneMgr.Mode.LETTUCE_PLAY:
						case SceneMgr.Mode.LETTUCE_COOP:
						case SceneMgr.Mode.LETTUCE_FRIENDLY:
						case SceneMgr.Mode.LETTUCE_PACK_OPENING:
						{
							SceneMgr.Get().SetNextMode(SceneMgr.Mode.HUB);
							await Coroutine.Sleep(5000);
							break;
						}
						case SceneMgr.Mode.LOGIN:
							await SceneLoginProc(new Login(scene.Address));
							break;
						case SceneMgr.Mode.HUB:
							await SceneHubProc(scene);
							break;
						case SceneMgr.Mode.COLLECTIONMANAGER:
							await SceneCollectionManagerProc(new CollectionManagerScene(scene.Address));
							break;
						case SceneMgr.Mode.TOURNAMENT:
							await SceneTournamentProc(new TournamentScene(scene.Address));
							break;
						case SceneMgr.Mode.GAMEPLAY:
						{
							if (gameType == GameType.GT_MERCENARIES_PVE ||
								gameType == GameType.GT_MERCENARIES_PVP ||
								gameType == GameType.GT_MERCENARIES_PVE_COOP)
							{
								if (gameState == null || !gameState.IsGameOver())
								{
									TritonHs.Concede(logReason: true);
									await Coroutine.Sleep(1000);
								}
								else
								{
									await ClientRandomClick("[炉石兄弟全局]", 2500);
								}
							}
							else 
							{
								await SceneGamePlayProc(Gameplay.Get());
							}
							break;
						}
						case SceneMgr.Mode.FATAL_ERROR:
							await ClientRandomClick("[炉石兄弟全局]", 2500);
							break;
						case SceneMgr.Mode.STARTUP:
						case SceneMgr.Mode.INVALID:
						case SceneMgr.Mode.PACKOPENING:
						case SceneMgr.Mode.FRIENDLY:
						case SceneMgr.Mode.DRAFT:
						case SceneMgr.Mode.CREDITS:
						case SceneMgr.Mode.RESET:
						case SceneMgr.Mode.ADVENTURE:
						case SceneMgr.Mode.TAVERN_BRAWL:
						default:				
						{
							await ClientRandomClick("[炉石兄弟全局]", 2500);
							break;
						}			
					}
				}
			}
		}

		//脚本启动
		public void Start()
		{
			coroutine_0 = new Coroutine(() => SceneAllProc());
			bool_2 = false;
			GameEventManager.Instance.Start();
			ProcessHookManager.Enable();
			GameEventManager.NewGame += NewGameEventArgsFunc;
			GameEventManager.GameOver += GameOverEventArgsFunc;
			GameEventManager.MulliganConfirm += MulliganConfirmEventArgsFunc;
			InactivePlayerKicker.Get().SetShouldCheckForInactivity(check: false);

			BnetPresenceMgr bnet = BnetPresenceMgr.Get();
			BnetBattleTag battleTag = bnet.GetMyPlayer().GetAccount().GetBattleTag();
			string hashCode = (battleTag.GetName() + "#" +
				battleTag.GetNumber()).GetHashCode().ToString();
			JsonSettings.SetMyHashCode(hashCode);
			DevSettings.Instance.CurrAccountHashCode = hashCode;

            DefaultBotSettings.Instance.ReloadFile();

			if (DefaultBotSettings.Instance.ReleaseLimit)
			{
				try
				{
					Screen.SetResolution(DefaultBotSettings.Instance.ReleaseLimitW,
						DefaultBotSettings.Instance.ReleaseLimitH, 3, 0);
				}
				catch (Exception e)
				{
					ilog_0.ErrorFormat("SetResolution An exception occurred: {0}.", e);
				}
			}
			try
			{
				TimeScaleMgr.Get().SetGameTimeScale(DefaultBotSettings.Instance.SliderShopSpeedRatio);
			}
			catch (Exception e)
			{
				ilog_0.ErrorFormat("SetGameTimeScale An exception occurred: {0}.", e);
			}

			PluginManager.Start();
			RoutineManager.Start();
		}

		//脚本停止
		public void Stop()
		{
			if (coroutine_0 != null)
			{
				coroutine_0.Dispose();
				coroutine_0 = null;
			}
			GameEventManager.GameOver -= GameOverEventArgsFunc;
			GameEventManager.NewGame -= NewGameEventArgsFunc;
			GameEventManager.MulliganConfirm -= MulliganConfirmEventArgsFunc;
			GameEventManager.Instance.Stop();
			PluginManager.Stop();
			RoutineManager.Stop();
			ProcessHookManager.Disable();
		}

		//脚本循环
		public void Tick()
		{
			if (coroutine_0.IsFinished)
			{
				ilog_0.DebugFormat("脚本已经停止 {0}", coroutine_0.Status);
				BotManager.Stop();
				return;
			}
			if (!TritonHs.IsClientInUsableState(logReason: true))
			{
				BotManager.MsBeforeNextTick += 750;
				coroutine_0.Dispose();
				coroutine_0 = new Coroutine(() => SceneAllProc());
				return;
			}
			if (!bool_2 && ChatMgr.Get().FriendListFrame != null)
			{
				ilog_0.ErrorFormat("[脚本循环] 好友聊天列表不为空.");
				Client.LeftClickAtDialog(BnetBar.Get().
					m_friendButton.m_OnlineCountText.Transform.Position);
				BotManager.MsBeforeNextTick += 1000;
				coroutine_0.Dispose();
				coroutine_0 = new Coroutine(() => SceneAllProc());
				return;
			}
			var unhandled = false;
			if (TritonHs.HandleDialog(ShouldAcceptFriendlyChallenge, out unhandled))
			{
				if (unhandled)
				{
					BotManager.Stop();
				}
				BotManager.MsBeforeNextTick += 3000;
				coroutine_0.Dispose();
				coroutine_0 = new Coroutine(() => SceneAllProc());
				return;
			}
			GameEventManager.Instance.Tick();
			PluginManager.Tick();
			RoutineManager.Tick();
			try
			{
				coroutine_0.Resume();//bot动作
			}
			catch
			{
				coroutine_0.Dispose();
				coroutine_0 = new Coroutine(() => SceneAllProc());
				throw;
			}
		}
	}
}
