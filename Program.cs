using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x02000112 RID: 274
	internal static class Program
	{
		// Token: 0x06000EB6 RID: 3766 RVA: 0x00070BE8 File Offset: 0x0006EDE8
		[STAThread]
		private static void Main()
		{
			Mutex mutex = new Mutex(false, "MyUniqueMutexName");
			try
			{
				AntiDump.Initialize();
				if (mutex.WaitOne(0, false))
				{
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					Program.CheckTaiNguyen();
					Application.Run(new FrmMain());
				}
				else
				{
					MessageBox.Show("Đã có phiên bản [ChickenAuto] khác đang hoạt động\nNếu bạn chắc chắn rằng Auto không bật\nVui lòng khởi động lại máy tính và thử lại. ", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			finally
			{
				if (mutex != null)
				{
					mutex.Close();
					mutex = null;
				}
			}
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00070C60 File Offset: 0x0006EE60
		public static bool IsUserAdministrator()
		{
			bool result;
			try
			{
				result = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
			}
			catch (UnauthorizedAccessException)
			{
				result = false;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00070CAC File Offset: 0x0006EEAC
		internal static bool IsRunAsAdmin()
		{
			return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00070CC2 File Offset: 0x0006EEC2
		public static void CheckTaiNguyen()
		{
			TINHKIEM.FileInstall("TinhKiemAuto", "EasyHook.dll", Global.APPPath + "\\Bin\\EasyHook.dll");
			TINHKIEM.FileInstall("TinhKiemAuto", "Newtonsoft.Json.dll", Global.APPPath + "\\Newtonsoft.Json.dll");
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00070D00 File Offset: 0x0006EF00
		private static bool Elevate()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				UseShellExecute = true,
				WorkingDirectory = Environment.CurrentDirectory,
				FileName = Application.ExecutablePath,
				Verb = "runas"
			};
			bool result;
			try
			{
				Process.Start(startInfo);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
	}
}
