using System;
using System.IO;
using System.Threading;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200004E RID: 78
	internal class LogManager
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000E361 File Offset: 0x0000C561
		// (set) Token: 0x06000267 RID: 615 RVA: 0x0000E368 File Offset: 0x0000C568
		public static LogTypes LogTypeToWrite { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000E370 File Offset: 0x0000C570
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		public static string LogPath
		{
			get
			{
				object obj = LogManager.mutex;
				lock (obj)
				{
					if (LogManager._LogPath == string.Empty)
					{
						LogManager._LogPath = AppDomain.CurrentDomain.BaseDirectory + "log/";
						if (!Directory.Exists(LogManager._LogPath))
						{
							Directory.CreateDirectory(LogManager._LogPath);
						}
					}
				}
				return LogManager._LogPath;
			}
			set
			{
				object obj = LogManager.mutex;
				lock (obj)
				{
					LogManager._LogPath = value;
				}
				if (!Directory.Exists(LogManager._LogPath))
				{
					Directory.CreateDirectory(LogManager._LogPath);
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000E438 File Offset: 0x0000C638
		// (set) Token: 0x0600026B RID: 619 RVA: 0x0000E4B0 File Offset: 0x0000C6B0
		public static string ExceptionPath
		{
			get
			{
				object obj = LogManager.mutex;
				lock (obj)
				{
					if (LogManager._ExceptionPath == string.Empty)
					{
						LogManager._ExceptionPath = AppDomain.CurrentDomain.BaseDirectory + "Exception/";
						if (!Directory.Exists(LogManager._ExceptionPath))
						{
							Directory.CreateDirectory(LogManager._ExceptionPath);
						}
					}
				}
				return LogManager._ExceptionPath;
			}
			set
			{
				object obj = LogManager.mutex;
				lock (obj)
				{
					LogManager._ExceptionPath = value;
				}
				if (!Directory.Exists(LogManager._ExceptionPath))
				{
					Directory.CreateDirectory(LogManager._ExceptionPath);
				}
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000E500 File Offset: 0x0000C700
		private static void WriteLog(string logFile, string logMsg)
		{
			try
			{
				StreamWriter streamWriter = File.AppendText(string.Concat(new string[]
				{
					LogManager.LogPath,
					logFile,
					"_",
					DateTime.Now.ToString("yyyyMMdd"),
					".log"
				}));
				string value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + logMsg;
				bool enableDbgView = LogManager.EnableDbgView;
				streamWriter.WriteLine(value);
				streamWriter.Close();
			}
			catch (Exception exception)
			{
				DebugTextLog.LogException(exception);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000E594 File Offset: 0x0000C794
		private static void _WriteException(string exceptionMsg)
		{
			try
			{
				StreamWriter streamWriter = File.CreateText(LogManager.ExceptionPath + "Exception_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
				streamWriter.WriteLine(exceptionMsg);
				streamWriter.Close();
			}
			catch (Exception exception)
			{
				DebugTextLog.LogException(exception);
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000E5F4 File Offset: 0x0000C7F4
		public static void WriteLog(LogTypes logType, string logMsg)
		{
			if (logType < LogManager.LogTypeToWrite)
			{
				return;
			}
			object obj = LogManager.mutex;
			lock (obj)
			{
				LogManager.WriteLog(logType.ToString(), logMsg);
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000E644 File Offset: 0x0000C844
		public static void WriteException(string exceptionMsg)
		{
			object obj = LogManager.mutex;
			lock (obj)
			{
				LogManager._WriteException(exceptionMsg);
			}
		}

		// Token: 0x0400023C RID: 572
		private static Timer timer;

		// Token: 0x0400023D RID: 573
		public static bool Enabled = true;

		// Token: 0x0400023E RID: 574
		private static Random rnd = new Random();

		// Token: 0x0400023F RID: 575
		private static int BaseV = 614;

		// Token: 0x04000240 RID: 576
		private static double BasrRate = 0.001;

		// Token: 0x04000241 RID: 577
		public static bool EnableDbgView = false;

		// Token: 0x04000242 RID: 578
		private static string _LogPath = string.Empty;

		// Token: 0x04000243 RID: 579
		private static string _ExceptionPath = string.Empty;

		// Token: 0x04000244 RID: 580
		private static object mutex = new object();
	}
}
