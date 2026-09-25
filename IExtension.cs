using System;
using System.IO;

namespace ProtoBuf
{
	// Token: 0x0200001F RID: 31
	public interface IExtension
	{
		// Token: 0x060000FD RID: 253
		Stream BeginAppend();

		// Token: 0x060000FE RID: 254
		void EndAppend(Stream stream, bool commit);

		// Token: 0x060000FF RID: 255
		Stream BeginQuery();

		// Token: 0x06000100 RID: 256
		void EndQuery(Stream stream);

		// Token: 0x06000101 RID: 257
		int GetLength();
	}
}
