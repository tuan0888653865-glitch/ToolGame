using System;

namespace TinhKiemAuto
{
	// Token: 0x020000B4 RID: 180
	public class LUA
	{
		// Token: 0x060009E2 RID: 2530 RVA: 0x000406FF File Offset: 0x0003E8FF
		public LUA(Game game)
		{
			this.game = game;
		}

		// Token: 0x17000253 RID: 595
		// (set) Token: 0x060009E3 RID: 2531 RVA: 0x0004070E File Offset: 0x0003E90E
		public int Index
		{
			set
			{
				Win.PostMessage(this.game.Handle, Global.HookMessage, value, 56);
			}
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00040729 File Offset: 0x0003E929
		public void GameProduceLoginMoveToCharacter(int index)
		{
			this.Index = index;
			Win.PostMessage(this.game.Handle, Global.HookMessage, 35, 105);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0004074C File Offset: 0x0003E94C
		public void QuestFrameMissionComplete(int index)
		{
			this.Index = index;
			Win.PostMessage(this.game.Handle, Global.HookMessage, 36, 105);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0004076F File Offset: 0x0003E96F
		public void SelectRoleEnterGame()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 37, 105);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0004078B File Offset: 0x0003E98B
		public void TheFireStove_FireButton_OnClick()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 38, 105);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x000407A7 File Offset: 0x0003E9A7
		public void TheFireStove_StoneButton_OnClick()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 39, 105);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x000407C3 File Offset: 0x0003E9C3
		public void TheFireStove_MessageBox_OK_Clicked()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 40, 105);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x000407DF File Offset: 0x0003E9DF
		public void Play_Ani(int index)
		{
			this.Index = index;
			Win.PostMessage(this.game.Handle, Global.HookMessage, 41, 105);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00040802 File Offset: 0x0003EA02
		public void LogOnSelectTail(int index)
		{
			this.Index = index;
			Win.PostMessage(this.game.Handle, Global.HookMessage, 42, 105);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00040825 File Offset: 0x0003EA25
		public void DataPoolReConnect()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 43, 105);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00040841 File Offset: 0x0003EA41
		public void LogOn_ExitToSelectServer()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 44, 105);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0004085D File Offset: 0x0003EA5D
		public void SelectServer(int index)
		{
			this.Index = index;
			Win.PostMessage(this.game.Handle, Global.HookMessage, 45, 105);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00040880 File Offset: 0x0003EA80
		public void TogleMissionOutline()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 46, 105);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0004089C File Offset: 0x0003EA9C
		public void AskRet2SelServer()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 47, 105);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x000408B8 File Offset: 0x0003EAB8
		public void PlayerCreateTeamSelf()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 48, 105);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x000408D4 File Offset: 0x0003EAD4
		public void OpenWindowMissionTrack()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 49, 105);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x000408F0 File Offset: 0x0003EAF0
		public void HuoDongRiChengNextClick()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 50, 105);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0004090C File Offset: 0x0003EB0C
		public void LoginOverTime()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 51, 105);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00040928 File Offset: 0x0003EB28
		public void YuanbaoShop(int list, int shop)
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, list, 57);
			Win.PostMessage(this.game.Handle, Global.HookMessage, shop, 58);
			Win.PostMessage(this.game.Handle, Global.HookMessage, 52, 105);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00040981 File Offset: 0x0003EB81
		public void ToggleYuanbaoShop()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 53, 105);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x000409A0 File Offset: 0x0003EBA0
		public void OutGhost()
		{
			string lua = "setmetatable(_G, {__index = Packet_Env }); Relive_Out_Ghost();";
			this.game.LuaDoOneLineString(lua);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x000409BF File Offset: 0x0003EBBF
		public void ReturnCount()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 55, 105);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x000409DB File Offset: 0x0003EBDB
		public void CountNil()
		{
			Win.PostMessage(this.game.Handle, Global.HookMessage, 56, 105);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x000409F7 File Offset: 0x0003EBF7
		public void Relive()
		{
			this.game.LuaDoOneLineString("Player:SendReliveMessage_Relive();");
		}

		// Token: 0x04000740 RID: 1856
		private Game game;
	}
}
