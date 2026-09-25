using System;
using System.ComponentModel;

namespace ProtoBuf
{
	// Token: 0x02000025 RID: 37
	[ImmutableObject(true)]
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class ProtoAfterSerializationAttribute : Attribute
	{
	}
}
