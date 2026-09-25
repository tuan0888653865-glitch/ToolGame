using System;
using System.ComponentModel;

namespace ProtoBuf
{
	// Token: 0x02000024 RID: 36
	[ImmutableObject(true)]
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class ProtoAfterDeserializationAttribute : Attribute
	{
	}
}
