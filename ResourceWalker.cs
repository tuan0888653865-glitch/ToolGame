using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000D5 RID: 213
	public class ResourceWalker
	{
		// Token: 0x06000B34 RID: 2868 RVA: 0x00049508 File Offset: 0x00047708
		public ResourceWalker(PortableExecutable image)
		{
			IMAGE_DATA_DIRECTORY image_DATA_DIRECTORY = image.NTHeader.OptionalHeader.DataDirectory[2];
			if (image_DATA_DIRECTORY.VirtualAddress > 0U && image_DATA_DIRECTORY.Size > 0U)
			{
				uint ptrFromRVA;
				IMAGE_RESOURCE_DIRECTORY image_RESOURCE_DIRECTORY;
				if (!image.Read<IMAGE_RESOURCE_DIRECTORY>((long)((ulong)(ptrFromRVA = image.GetPtrFromRVA(image_DATA_DIRECTORY.VirtualAddress))), SeekOrigin.Begin, out image_RESOURCE_DIRECTORY))
				{
					throw image.GetLastError();
				}
				IMAGE_RESOURCE_DIRECTORY_ENTRY entry = new IMAGE_RESOURCE_DIRECTORY_ENTRY
				{
					SubdirectoryRva = 2147483648U
				};
				this.Root = new ResourceWalker.ResourceDirectory(image, entry, false, ptrFromRVA);
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x0004958B File Offset: 0x0004778B
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x00049593 File Offset: 0x00047793
		public ResourceWalker.ResourceDirectory Root { get; private set; }

		// Token: 0x0200017F RID: 383
		public class ResourceDirectory : ResourceWalker.ResourceObject
		{
			// Token: 0x060011A7 RID: 4519 RVA: 0x00077DEC File Offset: 0x00075FEC
			public ResourceDirectory(PortableExecutable owner, IMAGE_RESOURCE_DIRECTORY_ENTRY entry, bool named, uint root) : base(owner, entry, named, root)
			{
				if (!owner.Read<IMAGE_RESOURCE_DIRECTORY>((long)((ulong)(root + (entry.SubdirectoryRva ^ 2147483648U))), SeekOrigin.Begin, out this._base))
				{
					throw owner.GetLastError();
				}
			}

			// Token: 0x060011A8 RID: 4520 RVA: 0x00077E20 File Offset: 0x00076020
			private void Initialize()
			{
				List<ResourceWalker.ResourceDirectory> list = new List<ResourceWalker.ResourceDirectory>();
				List<ResourceWalker.ResourceFile> list2 = new List<ResourceWalker.ResourceFile>();
				int numberOfNamedEntries = (int)this._base.NumberOfNamedEntries;
				for (int i = 0; i < numberOfNamedEntries + (int)this._base.NumberOfIdEntries; i++)
				{
					IMAGE_RESOURCE_DIRECTORY_ENTRY image_RESOURCE_DIRECTORY_ENTRY;
					if (this._owner.Read<IMAGE_RESOURCE_DIRECTORY_ENTRY>((long)((ulong)(this._root + 16U + (this._entry.SubdirectoryRva ^ 2147483648U)) + (ulong)((long)i * 8L)), SeekOrigin.Begin, out image_RESOURCE_DIRECTORY_ENTRY))
					{
						if ((image_RESOURCE_DIRECTORY_ENTRY.SubdirectoryRva & 2147483648U) != 0U)
						{
							list.Add(new ResourceWalker.ResourceDirectory(this._owner, image_RESOURCE_DIRECTORY_ENTRY, i < numberOfNamedEntries, this._root));
						}
						else
						{
							list2.Add(new ResourceWalker.ResourceFile(this._owner, image_RESOURCE_DIRECTORY_ENTRY, i < numberOfNamedEntries, this._root));
						}
					}
				}
				this._files = list2.ToArray();
				this._dirs = list.ToArray();
			}

			// Token: 0x17000409 RID: 1033
			// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00077EF8 File Offset: 0x000760F8
			public ResourceWalker.ResourceDirectory[] Directories
			{
				get
				{
					if (this._dirs == null)
					{
						this.Initialize();
					}
					return this._dirs;
				}
			}

			// Token: 0x1700040A RID: 1034
			// (get) Token: 0x060011AA RID: 4522 RVA: 0x00077F0E File Offset: 0x0007610E
			public ResourceWalker.ResourceFile[] Files
			{
				get
				{
					if (this._files == null)
					{
						this.Initialize();
					}
					return this._files;
				}
			}

			// Token: 0x04000E9B RID: 3739
			private IMAGE_RESOURCE_DIRECTORY _base;

			// Token: 0x04000E9C RID: 3740
			private ResourceWalker.ResourceDirectory[] _dirs;

			// Token: 0x04000E9D RID: 3741
			private ResourceWalker.ResourceFile[] _files;

			// Token: 0x04000E9E RID: 3742
			private const uint SZ_DIRECTORY = 16U;

			// Token: 0x04000E9F RID: 3743
			private const uint SZ_ENTRY = 8U;
		}

		// Token: 0x02000180 RID: 384
		public class ResourceFile : ResourceWalker.ResourceObject
		{
			// Token: 0x060011AB RID: 4523 RVA: 0x00077F24 File Offset: 0x00076124
			public ResourceFile(PortableExecutable owner, IMAGE_RESOURCE_DIRECTORY_ENTRY entry, bool named, uint root) : base(owner, entry, named, root)
			{
				if (!owner.Read<IMAGE_RESOURCE_DATA_ENTRY>((long)((ulong)(this._root + entry.DataEntryRva)), SeekOrigin.Begin, out this._base))
				{
					throw owner.GetLastError();
				}
			}

			// Token: 0x060011AC RID: 4524 RVA: 0x00077F58 File Offset: 0x00076158
			public byte[] GetData()
			{
				byte[] array = new byte[this._base.Size];
				if (!this._owner.Read((long)((ulong)this._owner.GetPtrFromRVA(this._base.OffsetToData)), SeekOrigin.Begin, array))
				{
					throw this._owner.GetLastError();
				}
				return array;
			}

			// Token: 0x04000EA0 RID: 3744
			private IMAGE_RESOURCE_DATA_ENTRY _base;
		}

		// Token: 0x02000181 RID: 385
		public abstract class ResourceObject
		{
			// Token: 0x060011AD RID: 4525 RVA: 0x00077FAC File Offset: 0x000761AC
			public ResourceObject(PortableExecutable owner, IMAGE_RESOURCE_DIRECTORY_ENTRY entry, bool named, uint root)
			{
				this._owner = owner;
				this._entry = entry;
				this.IsNamedResource = named;
				if (named)
				{
					ushort num = 0;
					if (owner.Read<ushort>((long)((ulong)(root + (entry.NameRva & 2147483647U))), SeekOrigin.Begin, out num))
					{
						byte[] array = new byte[(int)num << 1];
						if (owner.Read(0L, SeekOrigin.Current, array))
						{
							this._name = Encoding.Unicode.GetString(array);
						}
					}
					if (this._name == null)
					{
						throw owner.GetLastError();
					}
				}
				this._root = root;
			}

			// Token: 0x1700040B RID: 1035
			// (get) Token: 0x060011AE RID: 4526 RVA: 0x00078031 File Offset: 0x00076231
			public int Id
			{
				get
				{
					if (!this.IsNamedResource)
					{
						return (int)this._entry.IntegerId;
					}
					return -1;
				}
			}

			// Token: 0x1700040C RID: 1036
			// (get) Token: 0x060011AF RID: 4527 RVA: 0x00078048 File Offset: 0x00076248
			// (set) Token: 0x060011B0 RID: 4528 RVA: 0x00078050 File Offset: 0x00076250
			public bool IsNamedResource { get; protected set; }

			// Token: 0x1700040D RID: 1037
			// (get) Token: 0x060011B1 RID: 4529 RVA: 0x00078059 File Offset: 0x00076259
			public string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x04000EA2 RID: 3746
			protected IMAGE_RESOURCE_DIRECTORY_ENTRY _entry;

			// Token: 0x04000EA3 RID: 3747
			private string _name;

			// Token: 0x04000EA4 RID: 3748
			protected PortableExecutable _owner;

			// Token: 0x04000EA5 RID: 3749
			protected uint _root;
		}
	}
}
