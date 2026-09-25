using System;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x02000005 RID: 5
internal class IniFile
{
	// Token: 0x0600000A RID: 10
	[DllImport("kernel32")]
	private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

	// Token: 0x0600000B RID: 11
	[DllImport("kernel32")]
	private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

	// Token: 0x0600000C RID: 12 RVA: 0x000020CD File Offset: 0x000002CD
	public IniFile(string INIPath)
	{
		this.path = INIPath;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000020DC File Offset: 0x000002DC
	public void Write(string Section, string Key, string Value)
	{
		IniFile.WritePrivateProfileString(Section, Key, Value, this.path);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000020F0 File Offset: 0x000002F0
	public string Read(string Section, string Key)
	{
		StringBuilder stringBuilder = new StringBuilder(255);
		IniFile.GetPrivateProfileString(Section, Key, "", stringBuilder, 255, this.path);
		return stringBuilder.ToString();
	}

	// Token: 0x04000002 RID: 2
	public string path;
}
