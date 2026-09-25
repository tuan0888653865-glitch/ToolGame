using System;
using System.ComponentModel;

namespace ProtoBuf
{
	// Token: 0x02000027 RID: 39
	[ImmutableObject(true)]
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class ProtoBeforeSerializationAttribute : Attribute
	{
	}
}
