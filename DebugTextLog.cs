using System;
using ClientTools.Log;

// Token: 0x02000002 RID: 2
public static class DebugTextLog
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static void LogException(Exception exception)
	{
		if (DebugTextLog.DebugLog != null)
		{
			DebugTextLog.DebugLog.LogException(exception);
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
	public static void LogError(params object[] messages)
	{
		if (DebugTextLog.DebugLog != null)
		{
			DebugTextLog.DebugLog.LogError(messages);
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002078 File Offset: 0x00000278
	public static void Log(params object[] messages)
	{
		if (DebugTextLog.DebugLog != null)
		{
			DebugTextLog.DebugLog.Log(messages);
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0000208C File Offset: 0x0000028C
	public static void LogStackMsg(string message)
	{
		if (DebugTextLog.DebugLog != null)
		{
			DebugTextLog.DebugLog.LogStackMsg(message);
		}
	}

	// Token: 0x04000001 RID: 1
	public static IDebugTextLog DebugLog;
}
