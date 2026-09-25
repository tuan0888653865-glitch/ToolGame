using System;

namespace ProtoBuf
{
	// Token: 0x0200002A RID: 42
	public class ProtoException : Exception
	{
		// Token: 0x06000126 RID: 294 RVA: 0x0000A3FB File Offset: 0x000085FB
		public ProtoException()
		{
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000A403 File Offset: 0x00008603
		public ProtoException(string message) : base(message)
		{
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000A40C File Offset: 0x0000860C
		public ProtoException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
