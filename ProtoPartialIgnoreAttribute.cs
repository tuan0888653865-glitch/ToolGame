using System;

namespace ProtoBuf
{
	// Token: 0x0200002E RID: 46
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ProtoPartialIgnoreAttribute : ProtoIgnoreAttribute
	{
		// Token: 0x06000147 RID: 327 RVA: 0x0000A65E File Offset: 0x0000885E
		public ProtoPartialIgnoreAttribute(string memberName)
		{
			if (Helpers.IsNullOrEmpty(memberName))
			{
				throw new ArgumentNullException("memberName");
			}
			this.memberName = memberName;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000A680 File Offset: 0x00008880
		public string MemberName
		{
			get
			{
				return this.memberName;
			}
		}

		// Token: 0x040001B9 RID: 441
		private readonly string memberName;
	}
}
