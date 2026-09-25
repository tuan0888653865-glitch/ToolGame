using System;
using System.ComponentModel;

namespace ProtoBuf
{
	// Token: 0x02000026 RID: 38
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	[ImmutableObject(true)]
	public sealed class ProtoBeforeDeserializationAttribute : Attribute
	{
	}
}
