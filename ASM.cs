using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x0200007F RID: 127
	internal class ASM
	{
		// Token: 0x060004DC RID: 1244 RVA: 0x0001C674 File Offset: 0x0001A874
		public ASM(int processId)
		{
			this.ProcessId = processId;
			this.Id = ASM.OpenProcess(2035711, false, processId);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001C6A0 File Offset: 0x0001A8A0
		public int RunASM()
		{
			this.OPCode += "33C0C20400";
			this.ASMCode = new byte[this.OPCode.Length / 2];
			for (int i = 0; i < this.OPCode.Length / 2; i++)
			{
				this.ASMCode[i] = this.Hex2Byte(this.OPCode.Substring(i * 2, 2));
			}
			int num = ASM.VirtualAllocEx(this.Id, 0, this.ASMCode.Length, 4096U, 64U);
			ASM.WriteProcessMemory(this.Id, num, this.ASMCode, this.ASMCode.Length, 0);
			ASM.CloseHandle(ASM.CreateRemoteThread(this.Id, 0, 0, num, 0, 0, 0));
			return num;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001C760 File Offset: 0x0001A960
		public string Int2Hex(uint value, int n)
		{
			string text = value.ToString("X8");
			text = text.Substring(8 - n);
			string text2 = "";
			for (int i = 0; i < text.Length / 2; i++)
			{
				text2 += text.Substring(text.Length - i * 2 - 2, 2);
			}
			return text2;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001C7B8 File Offset: 0x0001A9B8
		public byte Hex2Byte(string hex)
		{
			byte result = 0;
			byte.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
			return result;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001C7DB File Offset: 0x0001A9DB
		public void Leave()
		{
			this.OPCode += "C9";
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001C7F3 File Offset: 0x0001A9F3
		public void Pushad()
		{
			this.OPCode += "60";
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001C80B File Offset: 0x0001AA0B
		public void Popad()
		{
			this.OPCode += "61";
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001C823 File Offset: 0x0001AA23
		public void Nop()
		{
			this.OPCode += "90";
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001C83B File Offset: 0x0001AA3B
		public void Ret()
		{
			this.OPCode += "C3";
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001C853 File Offset: 0x0001AA53
		public void RetA(uint i)
		{
			this.OPCode += this.Int2Hex(i, 4);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001C86E File Offset: 0x0001AA6E
		public void IN_AL_DX()
		{
			this.OPCode += "EC";
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001C886 File Offset: 0x0001AA86
		public void TEST_EAX_EAX()
		{
			this.OPCode += "85C0";
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001C89E File Offset: 0x0001AA9E
		public void Add_EAX_EDX()
		{
			this.OPCode += "03C2";
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001C8B6 File Offset: 0x0001AAB6
		public void Add_EBX_EAX()
		{
			this.OPCode += "03D8";
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001C8CE File Offset: 0x0001AACE
		public void Add_EAX_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "0305" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001C8EE File Offset: 0x0001AAEE
		public void Add_EBX_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "031D" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001C90E File Offset: 0x0001AB0E
		public void Add_EBP_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "032D" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001C92E File Offset: 0x0001AB2E
		public void Add_EAX(uint i)
		{
			this.OPCode = this.OPCode + "05" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001C94E File Offset: 0x0001AB4E
		public void Add_EBX(uint i)
		{
			this.OPCode = this.OPCode + "83C3" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001C96E File Offset: 0x0001AB6E
		public void Add_ECX(uint i)
		{
			this.OPCode = this.OPCode + "83C1" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001C98E File Offset: 0x0001AB8E
		public void Add_EDX(uint i)
		{
			this.OPCode = this.OPCode + "83C2" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001C9AE File Offset: 0x0001ABAE
		public void Add_ESI(uint i)
		{
			this.OPCode = this.OPCode + "83C6" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001C9CE File Offset: 0x0001ABCE
		public void Add_ESP(uint i)
		{
			this.OPCode = this.OPCode + "83C4" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001C9EE File Offset: 0x0001ABEE
		public void Call_EAX()
		{
			this.OPCode += "FFD0";
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001CA06 File Offset: 0x0001AC06
		public void Call_EBX()
		{
			this.OPCode += "FFD3";
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001CA1E File Offset: 0x0001AC1E
		public void Call_ECX()
		{
			this.OPCode += "FFD1";
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001CA36 File Offset: 0x0001AC36
		public void Call_EDX()
		{
			this.OPCode += "FFD2";
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001CA36 File Offset: 0x0001AC36
		public void Call_ESI()
		{
			this.OPCode += "FFD2";
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001CA4E File Offset: 0x0001AC4E
		public void Call_ESP()
		{
			this.OPCode += "FFD4";
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001CA66 File Offset: 0x0001AC66
		public void Call_EBP()
		{
			this.OPCode += "FFD5";
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001CA7E File Offset: 0x0001AC7E
		public void Call_EDI()
		{
			this.OPCode += "FFD7";
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001CA96 File Offset: 0x0001AC96
		public void Call_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "FF15" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001CAB6 File Offset: 0x0001ACB6
		public void Call_DWORD_Ptr_EAX()
		{
			this.OPCode += "FF10";
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001CACE File Offset: 0x0001ACCE
		public void Call_DWORD_Ptr_EBX()
		{
			this.OPCode += "FF13";
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001CAE6 File Offset: 0x0001ACE6
		public void Call_DWORD_Ptr_EDX_ADD(uint i)
		{
			this.OPCode = this.OPCode + "FF52" + this.Int2Hex(i, 8);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001CB08 File Offset: 0x0001AD08
		public void Cmp_EAX(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "83F8" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "3D" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001CB5A File Offset: 0x0001AD5A
		public void Cmp_EAX_EDX()
		{
			this.OPCode += "3BC2";
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001CB72 File Offset: 0x0001AD72
		public void Cmp_EAX_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "3B05" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001CB92 File Offset: 0x0001AD92
		public void Cmp_DWORD_Ptr_EAX(uint i)
		{
			this.OPCode = this.OPCode + "3905" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001CBB2 File Offset: 0x0001ADB2
		public void Dec_EAX()
		{
			this.OPCode += "48";
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001CBCA File Offset: 0x0001ADCA
		public void Dec_EBX()
		{
			this.OPCode += "4B";
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001CBE2 File Offset: 0x0001ADE2
		public void Dec_ECX()
		{
			this.OPCode += "49";
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001CBFA File Offset: 0x0001ADFA
		public void Dec_EDX()
		{
			this.OPCode += "4A";
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001CC12 File Offset: 0x0001AE12
		public void Idiv_EAX()
		{
			this.OPCode += "F7F8";
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001CC2A File Offset: 0x0001AE2A
		public void Idiv_EBX()
		{
			this.OPCode += "F7FB";
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001CC42 File Offset: 0x0001AE42
		public void Idiv_ECX()
		{
			this.OPCode += "F7F9";
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001CC5A File Offset: 0x0001AE5A
		public void Idiv_EDX()
		{
			this.OPCode += "F7FA";
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001CC72 File Offset: 0x0001AE72
		public void Imul_EAX_EDX()
		{
			this.OPCode += "0FAFC2";
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001CC8A File Offset: 0x0001AE8A
		public void Imul_EAX(uint i)
		{
			this.OPCode = this.OPCode + "6BC0" + this.Int2Hex(i, 2);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001CCAA File Offset: 0x0001AEAA
		public void ImulB_EAX(uint i)
		{
			this.OPCode = this.OPCode + "69C0" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001CCCA File Offset: 0x0001AECA
		public void Inc_EAX()
		{
			this.OPCode += "40";
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001CCE2 File Offset: 0x0001AEE2
		public void Inc_EBX()
		{
			this.OPCode += "43";
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001CCFA File Offset: 0x0001AEFA
		public void Inc_ECX()
		{
			this.OPCode += "41";
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001CD12 File Offset: 0x0001AF12
		public void Inc_EDX()
		{
			this.OPCode += "42";
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001CD2A File Offset: 0x0001AF2A
		public void Inc_EDI()
		{
			this.OPCode += "47";
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001CD42 File Offset: 0x0001AF42
		public void Inc_ESI()
		{
			this.OPCode += "46";
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001CD5A File Offset: 0x0001AF5A
		public void Inc_DWORD_Ptr_EAX()
		{
			this.OPCode += "FF00";
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001CD72 File Offset: 0x0001AF72
		public void Inc_DWORD_Ptr_EBX()
		{
			this.OPCode += "FF03";
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001CD8A File Offset: 0x0001AF8A
		public void Inc_DWORD_Ptr_ECX()
		{
			this.OPCode += "FF01";
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001CDA2 File Offset: 0x0001AFA2
		public void Inc_DWORD_Ptr_EDX()
		{
			this.OPCode += "FF02";
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001CDBA File Offset: 0x0001AFBA
		public void JMP_EAX()
		{
			this.OPCode += "FFE0";
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001CDD2 File Offset: 0x0001AFD2
		public void Mov_DWORD_Ptr_EAX(uint i)
		{
			this.OPCode = this.OPCode + "A3" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001CDF2 File Offset: 0x0001AFF2
		public void Mov_EAX(uint i)
		{
			this.OPCode = this.OPCode + "B8" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001CE12 File Offset: 0x0001B012
		public void Mov_EBX(uint i)
		{
			this.OPCode = this.OPCode + "BB" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001CE32 File Offset: 0x0001B032
		public void Mov_ECX(uint i)
		{
			this.OPCode = this.OPCode + "B9" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001CE52 File Offset: 0x0001B052
		public void Mov_EDX(uint i)
		{
			this.OPCode = this.OPCode + "BA" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001CE72 File Offset: 0x0001B072
		public void Mov_ESI(uint i)
		{
			this.OPCode = this.OPCode + "BE" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001CE92 File Offset: 0x0001B092
		public void Mov_ESP(uint i)
		{
			this.OPCode = this.OPCode + "BC" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001CEB2 File Offset: 0x0001B0B2
		public void Mov_EBP(uint i)
		{
			this.OPCode = this.OPCode + "BD" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001CED2 File Offset: 0x0001B0D2
		public void Mov_EDI(uint i)
		{
			this.OPCode = this.OPCode + "BF" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001CEF2 File Offset: 0x0001B0F2
		public void Mov_EBX_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "8B1D" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001CF12 File Offset: 0x0001B112
		public void Mov_ECX_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "8B0D" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001CF32 File Offset: 0x0001B132
		public void Mov_EAX_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "A1" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001CF52 File Offset: 0x0001B152
		public void Mov_EDX_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "8B15" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001CF72 File Offset: 0x0001B172
		public void Mov_ESI_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "8B35" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001CF92 File Offset: 0x0001B192
		public void Mov_ESP_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "8B25" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001CFB2 File Offset: 0x0001B1B2
		public void Mov_EBP_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "8B2D" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001CFD2 File Offset: 0x0001B1D2
		public void Mov_EAX_DWORD_Ptr_EAX()
		{
			this.OPCode += "8B00";
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001CFEA File Offset: 0x0001B1EA
		public void Mov_EAX_DWORD_Ptr_EBP()
		{
			this.OPCode += "8B4500";
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001D002 File Offset: 0x0001B202
		public void Mov_EAX_DWORD_Ptr_EBX()
		{
			this.OPCode += "8B03";
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001D01A File Offset: 0x0001B21A
		public void Mov_EAX_DWORD_Ptr_ECX()
		{
			this.OPCode += "8B01";
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001D032 File Offset: 0x0001B232
		public void Mov_EAX_DWORD_Ptr_EDX()
		{
			this.OPCode += "8B02";
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001D04A File Offset: 0x0001B24A
		public void Mov_EAX_DWORD_Ptr_EDI()
		{
			this.OPCode += "8B07";
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001D062 File Offset: 0x0001B262
		public void Mov_EAX_DWORD_Ptr_ESP()
		{
			this.OPCode += "8B0424";
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001D07A File Offset: 0x0001B27A
		public void Mov_EAX_DWORD_Ptr_ESI()
		{
			this.OPCode += "8B06";
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001D094 File Offset: 0x0001B294
		public void Mov_EAX_DWORD_Ptr_EAX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B40" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B80" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001D0E8 File Offset: 0x0001B2E8
		public void Mov_EAX_DWORD_Ptr_ESP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B4424" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B8424" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001D13C File Offset: 0x0001B33C
		public void Mov_EAX_DWORD_Ptr_EBX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B43" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B83" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001D190 File Offset: 0x0001B390
		public void Mov_EAX_DWORD_Ptr_ECX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B41" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B81" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001D1E4 File Offset: 0x0001B3E4
		public void Mov_EAX_DWORD_Ptr_EDX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B42" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B82" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001D238 File Offset: 0x0001B438
		public void Mov_EAX_DWORD_Ptr_EDI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B47" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B87" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001D28C File Offset: 0x0001B48C
		public void Mov_EAX_DWORD_Ptr_EBP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B45" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B85" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001D2E0 File Offset: 0x0001B4E0
		public void Mov_EAX_DWORD_Ptr_ESI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B46" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B86" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001D334 File Offset: 0x0001B534
		public void Mov_EBX_DWORD_Ptr_EAX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B58" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B98" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001D388 File Offset: 0x0001B588
		public void Mov_EBX_DWORD_Ptr_ESP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B5C24" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B9C24" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001D3DC File Offset: 0x0001B5DC
		public void Mov_EBX_DWORD_Ptr_EBX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B5B" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B9B" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001D430 File Offset: 0x0001B630
		public void Mov_EBX_DWORD_Ptr_ECX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B59" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B99" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001D484 File Offset: 0x0001B684
		public void Mov_EBX_DWORD_Ptr_EDX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B5A" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B9A" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001D4D8 File Offset: 0x0001B6D8
		public void Mov_EBX_DWORD_Ptr_EDI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B5F" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B9F" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001D52C File Offset: 0x0001B72C
		public void Mov_EBX_DWORD_Ptr_EBP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B5D" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B9D" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001D580 File Offset: 0x0001B780
		public void Mov_EBX_DWORD_Ptr_ESI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B5E" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B9E" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001D5D4 File Offset: 0x0001B7D4
		public void Mov_ECX_DWORD_Ptr_EAX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B48" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B88" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001D628 File Offset: 0x0001B828
		public void Mov_ECX_DWORD_Ptr_ESP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B4C24" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B8C24" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001D67C File Offset: 0x0001B87C
		public void Mov_ECX_DWORD_Ptr_EBX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B4B" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B8B" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001D6D0 File Offset: 0x0001B8D0
		public void Mov_ECX_DWORD_Ptr_ECX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B49" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B89" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001D724 File Offset: 0x0001B924
		public void Mov_ECX_DWORD_Ptr_EDX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B4A" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B8A" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001D778 File Offset: 0x0001B978
		public void Mov_ECX_DWORD_Ptr_EDI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B4F" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B8F" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001D7CC File Offset: 0x0001B9CC
		public void Mov_ECX_DWORD_Ptr_EBP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B4D" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B8D" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001D820 File Offset: 0x0001BA20
		public void Mov_ECX_DWORD_Ptr_ESI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B4E" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B8E" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001D874 File Offset: 0x0001BA74
		public void Mov_EDX_DWORD_Ptr_EAX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B50" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B90" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001D8C8 File Offset: 0x0001BAC8
		public void Mov_EDX_DWORD_Ptr_ESP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B5424" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B9424" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001D91C File Offset: 0x0001BB1C
		public void Mov_EDX_DWORD_Ptr_EBX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B53" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B93" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001D970 File Offset: 0x0001BB70
		public void Mov_EDX_DWORD_Ptr_ECX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B51" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B91" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001D9C4 File Offset: 0x0001BBC4
		public void Mov_EDX_DWORD_Ptr_EDX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B52" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B92" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001DA18 File Offset: 0x0001BC18
		public void Mov_EDX_DWORD_Ptr_EDI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B57" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B97" + this.Int2Hex(i, 8);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001DA6C File Offset: 0x0001BC6C
		public void Mov_EDX_DWORD_Ptr_EBP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B55" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B95" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001DAC0 File Offset: 0x0001BCC0
		public void Mov_EDX_DWORD_Ptr_ESI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8B56" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8B96" + this.Int2Hex(i, 8);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001DB12 File Offset: 0x0001BD12
		public void Mov_EBX_DWORD_Ptr_EAX()
		{
			this.OPCode += "8B18";
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001DB2A File Offset: 0x0001BD2A
		public void Mov_EBX_DWORD_Ptr_EBP()
		{
			this.OPCode += "8B5D00";
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001DB42 File Offset: 0x0001BD42
		public void Mov_EBX_DWORD_Ptr_EBX()
		{
			this.OPCode += "8B1B";
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001DB5A File Offset: 0x0001BD5A
		public void Mov_EBX_DWORD_Ptr_ECX()
		{
			this.OPCode += "8B19";
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001DB72 File Offset: 0x0001BD72
		public void Mov_EBX_DWORD_Ptr_EDX()
		{
			this.OPCode += "8B1A";
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001DB8A File Offset: 0x0001BD8A
		public void Mov_EBX_DWORD_Ptr_EDI()
		{
			this.OPCode += "8B1F";
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001DBA2 File Offset: 0x0001BDA2
		public void Mov_EBX_DWORD_Ptr_ESP()
		{
			this.OPCode += "8B1C24";
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001DBBA File Offset: 0x0001BDBA
		public void Mov_EBX_DWORD_Ptr_ESI()
		{
			this.OPCode += "8B1E";
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001DBD2 File Offset: 0x0001BDD2
		public void Mov_ECX_DWORD_Ptr_EAX()
		{
			this.OPCode += "8B08";
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001DBEA File Offset: 0x0001BDEA
		public void Mov_ECX_DWORD_Ptr_EBP()
		{
			this.OPCode += "8B4D00";
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001DC02 File Offset: 0x0001BE02
		public void Mov_ECX_DWORD_Ptr_EBX()
		{
			this.OPCode += "8B0B";
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001DC1A File Offset: 0x0001BE1A
		public void Mov_ECX_DWORD_Ptr_ECX()
		{
			this.OPCode += "8B09";
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001DC32 File Offset: 0x0001BE32
		public void Mov_ECX_DWORD_Ptr_EDX()
		{
			this.OPCode += "8B0A";
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001DC4A File Offset: 0x0001BE4A
		public void Mov_ECX_DWORD_Ptr_EDI()
		{
			this.OPCode += "8B0F";
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001DC62 File Offset: 0x0001BE62
		public void Mov_ECX_DWORD_Ptr_ESP()
		{
			this.OPCode += "8B0C24";
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001DC7A File Offset: 0x0001BE7A
		public void Mov_ECX_DWORD_Ptr_ESI()
		{
			this.OPCode += "8B0E";
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001DC92 File Offset: 0x0001BE92
		public void Mov_EDX_DWORD_Ptr_EAX()
		{
			this.OPCode += "8B10";
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001DCAA File Offset: 0x0001BEAA
		public void Mov_EDX_DWORD_Ptr_EBP()
		{
			this.OPCode += "8B5500";
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001DCC2 File Offset: 0x0001BEC2
		public void Mov_EDX_DWORD_Ptr_EBX()
		{
			this.OPCode += "8B13";
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001DCDA File Offset: 0x0001BEDA
		public void Mov_EDX_DWORD_Ptr_ECX()
		{
			this.OPCode += "8B11";
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001DCF2 File Offset: 0x0001BEF2
		public void Mov_EDX_DWORD_Ptr_EDX()
		{
			this.OPCode += "8B12";
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001DD0A File Offset: 0x0001BF0A
		public void Mov_EDX_DWORD_Ptr_EDI()
		{
			this.OPCode += "8B17";
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001DD22 File Offset: 0x0001BF22
		public void Mov_EDX_DWORD_Ptr_ESI()
		{
			this.OPCode += "8B16";
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001DD3A File Offset: 0x0001BF3A
		public void Mov_EDX_DWORD_Ptr_ESP()
		{
			this.OPCode += "8B1424";
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001DD52 File Offset: 0x0001BF52
		public void Mov_EAX_EBP()
		{
			this.OPCode += "8BC5";
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001DD6A File Offset: 0x0001BF6A
		public void Mov_EAX_EBX()
		{
			this.OPCode += "8BC3";
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001DD82 File Offset: 0x0001BF82
		public void Mov_EAX_ECX()
		{
			this.OPCode += "8BC1";
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001DD9A File Offset: 0x0001BF9A
		public void Mov_EAX_EDI()
		{
			this.OPCode += "8BC7";
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001DDB2 File Offset: 0x0001BFB2
		public void Mov_EAX_EDX()
		{
			this.OPCode += "8BC2";
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001DDCA File Offset: 0x0001BFCA
		public void Mov_EAX_ESI()
		{
			this.OPCode += "8BC6";
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001DDE2 File Offset: 0x0001BFE2
		public void Mov_EAX_ESP()
		{
			this.OPCode += "8BC4";
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001DDFA File Offset: 0x0001BFFA
		public void Mov_EBX_EBP()
		{
			this.OPCode += "8BDD";
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001DE12 File Offset: 0x0001C012
		public void Mov_EBX_EAX()
		{
			this.OPCode += "8BD8";
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001DE2A File Offset: 0x0001C02A
		public void Mov_EBX_ECX()
		{
			this.OPCode += "8BD9";
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001DE42 File Offset: 0x0001C042
		public void Mov_EBX_EDI()
		{
			this.OPCode += "8BDF";
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001DE5A File Offset: 0x0001C05A
		public void Mov_EBX_EDX()
		{
			this.OPCode += "8BDA";
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001DE72 File Offset: 0x0001C072
		public void Mov_EBX_ESI()
		{
			this.OPCode += "8BDE";
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001DE8A File Offset: 0x0001C08A
		public void Mov_EBX_ESP()
		{
			this.OPCode += "8BDC";
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0001DEA2 File Offset: 0x0001C0A2
		public void Mov_ECX_EBP()
		{
			this.OPCode += "8BCD";
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001DEBA File Offset: 0x0001C0BA
		public void Mov_ECX_EAX()
		{
			this.OPCode += "8BC8";
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001DED2 File Offset: 0x0001C0D2
		public void Mov_ECX_EBX()
		{
			this.OPCode += "8BCB";
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001DEEA File Offset: 0x0001C0EA
		public void Mov_ECX_EDI()
		{
			this.OPCode += "8BCF";
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001DF02 File Offset: 0x0001C102
		public void Mov_ECX_EDX()
		{
			this.OPCode += "8BCA";
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001DF1A File Offset: 0x0001C11A
		public void Mov_ECX_ESI()
		{
			this.OPCode += "8BCE";
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001DF32 File Offset: 0x0001C132
		public void Mov_ECX_ESP()
		{
			this.OPCode += "8BCC";
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001DF4A File Offset: 0x0001C14A
		public void Mov_EDX_EBP()
		{
			this.OPCode += "8BD5";
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001DF62 File Offset: 0x0001C162
		public void Mov_EDX_EBX()
		{
			this.OPCode += "8BD3";
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001DF7A File Offset: 0x0001C17A
		public void Mov_EDX_ECX()
		{
			this.OPCode += "8BD1";
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001DF92 File Offset: 0x0001C192
		public void Mov_EDX_EDI()
		{
			this.OPCode += "8BD7";
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001DFAA File Offset: 0x0001C1AA
		public void Mov_EDX_EAX()
		{
			this.OPCode += "8BD0";
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001DFC2 File Offset: 0x0001C1C2
		public void Mov_EDX_ESI()
		{
			this.OPCode += "8BD6";
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001DFDA File Offset: 0x0001C1DA
		public void Mov_EDX_ESP()
		{
			this.OPCode += "8BD4";
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001DFF2 File Offset: 0x0001C1F2
		public void Mov_ESI_EBP()
		{
			this.OPCode += "8BF5";
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001E00A File Offset: 0x0001C20A
		public void Mov_ESI_EBX()
		{
			this.OPCode += "8BF3";
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001E022 File Offset: 0x0001C222
		public void Mov_ESI_ECX()
		{
			this.OPCode += "8BF1";
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001E03A File Offset: 0x0001C23A
		public void Mov_ESI_EDI()
		{
			this.OPCode += "8BF7";
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001E052 File Offset: 0x0001C252
		public void Mov_ESI_EAX()
		{
			this.OPCode += "8BF0";
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001E06A File Offset: 0x0001C26A
		public void Mov_ESI_EDX()
		{
			this.OPCode += "8BF2";
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001E082 File Offset: 0x0001C282
		public void Mov_ESI_ESP()
		{
			this.OPCode += "8BF4";
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001E09A File Offset: 0x0001C29A
		public void Mov_ESP_EBP()
		{
			this.OPCode += "8BE5";
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001E0B2 File Offset: 0x0001C2B2
		public void Mov_ESP_EBX()
		{
			this.OPCode += "8BE3";
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001E0CA File Offset: 0x0001C2CA
		public void Mov_ESP_ECX()
		{
			this.OPCode += "8BE1";
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001E0E2 File Offset: 0x0001C2E2
		public void Mov_ESP_EDI()
		{
			this.OPCode += "8BE7";
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001E0FA File Offset: 0x0001C2FA
		public void Mov_ESP_EAX()
		{
			this.OPCode += "8BE0";
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001E112 File Offset: 0x0001C312
		public void Mov_ESP_EDX()
		{
			this.OPCode += "8BE2";
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001E12A File Offset: 0x0001C32A
		public void Mov_ESP_ESI()
		{
			this.OPCode += "8BE6";
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001E142 File Offset: 0x0001C342
		public void Mov_EDI_EBP()
		{
			this.OPCode += "8BFD";
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001E15A File Offset: 0x0001C35A
		public void Mov_EDI_EAX()
		{
			this.OPCode += "8BF8";
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001E172 File Offset: 0x0001C372
		public void Mov_EDI_EBX()
		{
			this.OPCode += "8BFB";
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001E18A File Offset: 0x0001C38A
		public void Mov_EDI_ECX()
		{
			this.OPCode += "8BF9";
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001E1A2 File Offset: 0x0001C3A2
		public void Mov_EDI_EDX()
		{
			this.OPCode += "8BFA";
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001E1BA File Offset: 0x0001C3BA
		public void Mov_EDI_ESI()
		{
			this.OPCode += "8BFE";
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001E1D2 File Offset: 0x0001C3D2
		public void Mov_EDI_ESP()
		{
			this.OPCode += "8BFC";
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001DE42 File Offset: 0x0001C042
		public void Mov_EBP_EDI()
		{
			this.OPCode += "8BDF";
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001E1EA File Offset: 0x0001C3EA
		public void Mov_EBP_EAX()
		{
			this.OPCode += "8BE8";
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001E202 File Offset: 0x0001C402
		public void Mov_EBP_EBX()
		{
			this.OPCode += "8BEB";
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001E21A File Offset: 0x0001C41A
		public void Mov_EBP_ECX()
		{
			this.OPCode += "8BE9";
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001E232 File Offset: 0x0001C432
		public void Mov_EBP_EDX()
		{
			this.OPCode += "8BEA";
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001E24A File Offset: 0x0001C44A
		public void Mov_EBP_ESI()
		{
			this.OPCode += "8BEE";
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001E262 File Offset: 0x0001C462
		public void Mov_EBP_ESP()
		{
			this.OPCode += "8BEC";
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001E27A File Offset: 0x0001C47A
		public void Push(uint i)
		{
			this.OPCode = this.OPCode + "68" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001E29A File Offset: 0x0001C49A
		public void Push_DWORD_Ptr(uint i)
		{
			this.OPCode = this.OPCode + "FF35" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001E2BA File Offset: 0x0001C4BA
		public void Push_EAX()
		{
			this.OPCode += "50";
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001E2D2 File Offset: 0x0001C4D2
		public void Push_ECX()
		{
			this.OPCode += "51";
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001E2EA File Offset: 0x0001C4EA
		public void Push_EDX()
		{
			this.OPCode += "52";
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001E302 File Offset: 0x0001C502
		public void Push_EBX()
		{
			this.OPCode += "53";
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001E31A File Offset: 0x0001C51A
		public void Push_ESP()
		{
			this.OPCode += "54";
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001E332 File Offset: 0x0001C532
		public void Push_EBP()
		{
			this.OPCode += "55";
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001E34A File Offset: 0x0001C54A
		public void Push_ESI()
		{
			this.OPCode += "56";
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001E362 File Offset: 0x0001C562
		public void Push_EDI()
		{
			this.OPCode += "57";
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001E37C File Offset: 0x0001C57C
		public void Lea_EAX_DWORD_Ptr_EAX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D40" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D80" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001E3D0 File Offset: 0x0001C5D0
		public void Lea_EAX_DWORD_Ptr_EBX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D43" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D83" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001E424 File Offset: 0x0001C624
		public void Lea_EAX_DWORD_Ptr_ECX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D41" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D81" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001E478 File Offset: 0x0001C678
		public void Lea_EAX_DWORD_Ptr_EDX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D42" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D82" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001E4CC File Offset: 0x0001C6CC
		public void Lea_EAX_DWORD_Ptr_ESI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D46" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D86" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001E520 File Offset: 0x0001C720
		public void Lea_EAX_DWORD_Ptr_ESP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D40" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D80" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001E574 File Offset: 0x0001C774
		public void Lea_EAX_DWORD_Ptr_EBP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D4424" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D8424" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001E5C8 File Offset: 0x0001C7C8
		public void Lea_EAX_DWORD_Ptr_EDI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D47" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D87" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001E61C File Offset: 0x0001C81C
		public void Lea_EBX_DWORD_Ptr_EAX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D58" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D98" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001E670 File Offset: 0x0001C870
		public void Lea_EBX_DWORD_Ptr_ESP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D5C24" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D9C24" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001E6C4 File Offset: 0x0001C8C4
		public void Lea_EBX_DWORD_Ptr_EBX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D5B" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D9B" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001E718 File Offset: 0x0001C918
		public void Lea_EBX_DWORD_Ptr_ECX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D59" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D99" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001E76C File Offset: 0x0001C96C
		public void Lea_EBX_DWORD_Ptr_EDX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D5A" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D9A" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001E7C0 File Offset: 0x0001C9C0
		public void Lea_EBX_DWORD_Ptr_EDI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D5F" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D9F" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001E814 File Offset: 0x0001CA14
		public void Lea_EBX_DWORD_Ptr_EBP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D5D" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D9D" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001E868 File Offset: 0x0001CA68
		public void Lea_EBX_DWORD_Ptr_ESI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D5E" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D9E" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001E8BC File Offset: 0x0001CABC
		public void Lea_ECX_DWORD_Ptr_EAX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D48" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D88" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001E910 File Offset: 0x0001CB10
		public void Lea_ECX_DWORD_Ptr_ESP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D4C24" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D8C24" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001E964 File Offset: 0x0001CB64
		public void Lea_ECX_DWORD_Ptr_EBX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D4B" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D8B" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001E9B8 File Offset: 0x0001CBB8
		public void Lea_ECX_DWORD_Ptr_ECX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D49" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D89" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001EA0C File Offset: 0x0001CC0C
		public void Lea_ECX_DWORD_Ptr_EDX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D4A" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D8A" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001EA60 File Offset: 0x0001CC60
		public void Lea_ECX_DWORD_Ptr_EDI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D4F" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D8F" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001EAB4 File Offset: 0x0001CCB4
		public void Lea_ECX_DWORD_Ptr_EBP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D4D" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D8D" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001EB08 File Offset: 0x0001CD08
		public void Lea_ECX_DWORD_Ptr_ESI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D4E" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D8E" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001EB5C File Offset: 0x0001CD5C
		public void Lea_EDX_DWORD_Ptr_EAX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D50" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D90" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001EBB0 File Offset: 0x0001CDB0
		public void Lea_EDX_DWORD_Ptr_ESP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D5424" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D9424" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001EC04 File Offset: 0x0001CE04
		public void Lea_EDX_DWORD_Ptr_EBX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D53" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D93" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001EC58 File Offset: 0x0001CE58
		public void Lea_EDX_DWORD_Ptr_ECX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D51" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D91" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001ECAC File Offset: 0x0001CEAC
		public void Lea_EDX_DWORD_Ptr_EDX_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D52" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D92" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001ED00 File Offset: 0x0001CF00
		public void Lea_EDX_DWORD_Ptr_EDI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D57" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D97" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001ED54 File Offset: 0x0001CF54
		public void Lea_EDX_DWORD_Ptr_EBP_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D55" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D95" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001EDA8 File Offset: 0x0001CFA8
		public void Lea_EDX_DWORD_Ptr_ESI_Add(uint i)
		{
			if (i <= 255U)
			{
				this.OPCode = this.OPCode + "8D56" + this.Int2Hex(i, 2);
				return;
			}
			this.OPCode = this.OPCode + "8D96" + this.Int2Hex(i, 8);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001EDFA File Offset: 0x0001CFFA
		public void Pop_EAX()
		{
			this.OPCode += "58";
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001EE12 File Offset: 0x0001D012
		public void Pop_EBX()
		{
			this.OPCode += "5B";
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001EE2A File Offset: 0x0001D02A
		public void Pop_ECX()
		{
			this.OPCode += "59";
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001EE42 File Offset: 0x0001D042
		public void Pop_EDX()
		{
			this.OPCode += "5A";
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001EE5A File Offset: 0x0001D05A
		public void Pop_ESI()
		{
			this.OPCode += "5E";
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001EE72 File Offset: 0x0001D072
		public void Pop_ESP()
		{
			this.OPCode += "5C";
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001EE8A File Offset: 0x0001D08A
		public void Pop_EDI()
		{
			this.OPCode += "5F";
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001EEA2 File Offset: 0x0001D0A2
		public void Pop_EBP()
		{
			this.OPCode += "5D";
		}

		// Token: 0x060005D3 RID: 1491
		[DllImport("kernel32.dll")]
		public static extern IntPtr CreateRemoteThread(IntPtr hProcess, int lpThreadAttributes, int dwStackSize, int lpStartAddress, int lpParameter, int dwCreationFlags, int lpThreadId);

		// Token: 0x060005D4 RID: 1492
		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int processAccess, bool bInheritHandle, int processId);

		// Token: 0x060005D5 RID: 1493
		[DllImport("kernel32.dll")]
		private static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x060005D6 RID: 1494
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

		// Token: 0x060005D7 RID: 1495
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern int VirtualAllocEx(IntPtr hProcess, int lpAddress, int dwSize, uint flAllocationType, uint flProtect);

		// Token: 0x060005D8 RID: 1496
		[DllImport("kernel32.dll")]
		public static extern bool VirtualFreeEx(IntPtr hProcess, int lpAddress, int dwSize, int dwFreeType);

		// Token: 0x040003B7 RID: 951
		public int ProcessId;

		// Token: 0x040003B8 RID: 952
		public IntPtr Id;

		// Token: 0x040003B9 RID: 953
		public string OPCode = "";

		// Token: 0x040003BA RID: 954
		public byte[] ASMCode;
	}
}
