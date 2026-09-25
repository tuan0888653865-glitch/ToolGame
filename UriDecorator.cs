using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000066 RID: 102
	internal sealed class UriDecorator : ProtoDecoratorBase
	{
		// Token: 0x0600032C RID: 812 RVA: 0x00010BC9 File Offset: 0x0000EDC9
		public UriDecorator(TypeModel model, IProtoSerializer tail) : base(tail)
		{
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00010BD2 File Offset: 0x0000EDD2
		public override Type ExpectedType
		{
			get
			{
				return UriDecorator.expectedType;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00008C9D File Offset: 0x00006E9D
		public override bool RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000D470 File Offset: 0x0000B670
		public override bool ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00010BD9 File Offset: 0x0000EDD9
		public override void Write(object value, ProtoWriter dest)
		{
			this.Tail.Write(((Uri)value).AbsoluteUri, dest);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00010BF4 File Offset: 0x0000EDF4
		public override object Read(object value, ProtoReader source)
		{
			string text = (string)this.Tail.Read(null, source);
			if (text.Length != 0)
			{
				return new Uri(text);
			}
			return null;
		}

		// Token: 0x0400028A RID: 650
		private static readonly Type expectedType = typeof(Uri);
	}
}
