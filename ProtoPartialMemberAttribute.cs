using System;

namespace ProtoBuf
{
	// Token: 0x0200002F RID: 47
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ProtoPartialMemberAttribute : ProtoMemberAttribute
	{
		// Token: 0x06000149 RID: 329 RVA: 0x0000A688 File Offset: 0x00008888
		public ProtoPartialMemberAttribute(int tag, string memberName) : base(tag)
		{
			if (Helpers.IsNullOrEmpty(memberName))
			{
				throw new ArgumentNullException("memberName");
			}
			this.memberName = memberName;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000A6AB File Offset: 0x000088AB
		public string MemberName
		{
			get
			{
				return this.memberName;
			}
		}

		// Token: 0x040001BA RID: 442
		private readonly string memberName;
	}
}
