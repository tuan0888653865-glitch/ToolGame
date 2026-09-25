using System;
using System.Collections;

namespace ProtoBuf.Meta
{
	// Token: 0x02000068 RID: 104
	internal class BasicList : IEnumerable
	{
		// Token: 0x0600033C RID: 828 RVA: 0x00010D05 File Offset: 0x0000EF05
		public void CopyTo(Array array, int offset)
		{
			this.head.CopyTo(array, offset);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00010D14 File Offset: 0x0000EF14
		public int Add(object value)
		{
			return (this.head = this.head.Append(value)).Length - 1;
		}

		// Token: 0x170000B1 RID: 177
		public object this[int index]
		{
			get
			{
				return this.head[index];
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00010D4B File Offset: 0x0000EF4B
		public object TryGet(int index)
		{
			return this.head.TryGet(index);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00010D59 File Offset: 0x0000EF59
		public void Trim()
		{
			this.head = this.head.Trim();
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00010D6C File Offset: 0x0000EF6C
		public int Count
		{
			get
			{
				return this.head.Length;
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00010D79 File Offset: 0x0000EF79
		public IEnumerator GetEnumerator()
		{
			return new BasicList.NodeEnumerator(this.head);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00010D86 File Offset: 0x0000EF86
		internal int IndexOf(BasicList.IPredicate predicate)
		{
			return this.head.IndexOf(predicate);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00010D94 File Offset: 0x0000EF94
		internal int IndexOfReference(object instance)
		{
			return this.head.IndexOfReference(instance);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00010DA4 File Offset: 0x0000EFA4
		internal bool Contains(object value)
		{
			using (IEnumerator enumerator = this.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (object.Equals(enumerator.Current, value))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00010DFC File Offset: 0x0000EFFC
		internal static BasicList GetContiguousGroups(int[] keys, object[] values)
		{
			if (keys == null)
			{
				throw new ArgumentNullException("keys");
			}
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length < keys.Length)
			{
				throw new ArgumentException("Not all keys are covered by values", "values");
			}
			BasicList basicList = new BasicList();
			BasicList.Group group = null;
			for (int i = 0; i < keys.Length; i++)
			{
				if (i == 0 || keys[i] != keys[i - 1])
				{
					group = null;
				}
				if (group == null)
				{
					group = new BasicList.Group(keys[i]);
					basicList.Add(group);
				}
				group.Items.Add(values[i]);
			}
			return basicList;
		}

		// Token: 0x0400028B RID: 651
		private static readonly BasicList.Node nil = new BasicList.Node(null, 0);

		// Token: 0x0400028C RID: 652
		protected BasicList.Node head = BasicList.nil;

		// Token: 0x02000157 RID: 343
		private sealed class NodeEnumerator : IEnumerator
		{
			// Token: 0x06001146 RID: 4422 RVA: 0x000775C3 File Offset: 0x000757C3
			public NodeEnumerator(BasicList.Node node)
			{
				this.node = node;
			}

			// Token: 0x06001147 RID: 4423 RVA: 0x000775D9 File Offset: 0x000757D9
			void IEnumerator.Reset()
			{
				this.position = -1;
			}

			// Token: 0x170003FD RID: 1021
			// (get) Token: 0x06001148 RID: 4424 RVA: 0x000775E2 File Offset: 0x000757E2
			public object Current
			{
				get
				{
					return this.node[this.position];
				}
			}

			// Token: 0x06001149 RID: 4425 RVA: 0x000775F8 File Offset: 0x000757F8
			public bool MoveNext()
			{
				int length = this.node.Length;
				if (this.position <= length)
				{
					int num = this.position + 1;
					this.position = num;
					return num < length;
				}
				return false;
			}

			// Token: 0x04000E14 RID: 3604
			private int position = -1;

			// Token: 0x04000E15 RID: 3605
			private readonly BasicList.Node node;
		}

		// Token: 0x02000158 RID: 344
		protected sealed class Node
		{
			// Token: 0x170003FE RID: 1022
			public object this[int index]
			{
				get
				{
					if (index >= 0 && index < this.length)
					{
						return this.data[index];
					}
					throw new ArgumentOutOfRangeException("index");
				}
				set
				{
					if (index >= 0 && index < this.length)
					{
						this.data[index] = value;
						return;
					}
					throw new ArgumentOutOfRangeException("index");
				}
			}

			// Token: 0x0600114C RID: 4428 RVA: 0x00077675 File Offset: 0x00075875
			public object TryGet(int index)
			{
				if (index < 0 || index >= this.length)
				{
					return null;
				}
				return this.data[index];
			}

			// Token: 0x170003FF RID: 1023
			// (get) Token: 0x0600114D RID: 4429 RVA: 0x0007768E File Offset: 0x0007588E
			public int Length
			{
				get
				{
					return this.length;
				}
			}

			// Token: 0x0600114E RID: 4430 RVA: 0x00077696 File Offset: 0x00075896
			internal Node(object[] data, int length)
			{
				this.data = data;
				this.length = length;
			}

			// Token: 0x0600114F RID: 4431 RVA: 0x000776AC File Offset: 0x000758AC
			public void RemoveLastWithMutate()
			{
				if (this.length == 0)
				{
					throw new InvalidOperationException();
				}
				this.length--;
			}

			// Token: 0x06001150 RID: 4432 RVA: 0x000776CC File Offset: 0x000758CC
			public BasicList.Node Append(object value)
			{
				int num = this.length + 1;
				object[] array;
				if (this.data == null)
				{
					array = new object[10];
				}
				else if (this.length == this.data.Length)
				{
					array = new object[this.data.Length * 2];
					Array.Copy(this.data, array, this.length);
				}
				else
				{
					array = this.data;
				}
				array[this.length] = value;
				return new BasicList.Node(array, num);
			}

			// Token: 0x06001151 RID: 4433 RVA: 0x00077740 File Offset: 0x00075940
			public BasicList.Node Trim()
			{
				if (this.length == 0 || this.length == this.data.Length)
				{
					return this;
				}
				object[] destinationArray = new object[this.length];
				Array.Copy(this.data, destinationArray, this.length);
				return new BasicList.Node(destinationArray, this.length);
			}

			// Token: 0x06001152 RID: 4434 RVA: 0x00077794 File Offset: 0x00075994
			internal int IndexOfReference(object instance)
			{
				for (int i = 0; i < this.length; i++)
				{
					if (instance == this.data[i])
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06001153 RID: 4435 RVA: 0x000777C0 File Offset: 0x000759C0
			internal int IndexOf(BasicList.IPredicate predicate)
			{
				for (int i = 0; i < this.length; i++)
				{
					if (predicate.IsMatch(this.data[i]))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06001154 RID: 4436 RVA: 0x000777F1 File Offset: 0x000759F1
			internal void CopyTo(Array array, int offset)
			{
				if (this.length > 0)
				{
					Array.Copy(this.data, 0, array, offset, this.length);
				}
			}

			// Token: 0x04000E16 RID: 3606
			private readonly object[] data;

			// Token: 0x04000E17 RID: 3607
			private int length;
		}

		// Token: 0x02000159 RID: 345
		internal interface IPredicate
		{
			// Token: 0x06001155 RID: 4437
			bool IsMatch(object obj);
		}

		// Token: 0x0200015A RID: 346
		internal class Group
		{
			// Token: 0x06001156 RID: 4438 RVA: 0x00077810 File Offset: 0x00075A10
			public Group(int first)
			{
				this.First = first;
				this.Items = new BasicList();
			}

			// Token: 0x04000E18 RID: 3608
			public readonly int First;

			// Token: 0x04000E19 RID: 3609
			public readonly BasicList Items;
		}
	}
}
