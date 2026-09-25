using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace TinhKiemAuto
{
	// Token: 0x020000F0 RID: 240
	public static class Utils
	{
		// Token: 0x06000CA2 RID: 3234 RVA: 0x00052290 File Offset: 0x00050490
		public static T DeepClone<T>(T obj)
		{
			T result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, obj);
				memoryStream.Position = 0L;
				result = (T)((object)binaryFormatter.Deserialize(memoryStream));
			}
			return result;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x000522E8 File Offset: 0x000504E8
		public static uint SizeOf(this Type t)
		{
			return (uint)Marshal.SizeOf(t);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x000522F0 File Offset: 0x000504F0
		public static string WriteTempData(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			string text = null;
			try
			{
				text = Path.GetTempFileName();
			}
			catch (IOException)
			{
				text = Path.Combine(Directory.GetCurrentDirectory(), Path.GetRandomFileName());
			}
			try
			{
				File.WriteAllBytes(text, data);
			}
			catch
			{
				text = null;
			}
			return text;
		}
	}
}
