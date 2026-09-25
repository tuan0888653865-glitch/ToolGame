using System;
using System.Collections.Generic;
using System.IO;

namespace TinhKiemAuto
{
	// Token: 0x020000CB RID: 203
	[Serializable]
	public class PortableExecutable : MemoryIterator
	{
		// Token: 0x06000ACA RID: 2762 RVA: 0x00047F59 File Offset: 0x00046159
		public PortableExecutable(string path) : this(File.ReadAllBytes(path))
		{
			this.FileLocation = path;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00047F70 File Offset: 0x00046170
		public PortableExecutable(byte[] data) : base(data)
		{
			string text = string.Empty;
			IMAGE_NT_HEADER32 image_NT_HEADER = default(IMAGE_NT_HEADER32);
			IMAGE_DOS_HEADER image_DOS_HEADER = default(IMAGE_DOS_HEADER);
			if (base.Read<IMAGE_DOS_HEADER>(out image_DOS_HEADER) && image_DOS_HEADER.e_magic == 23117)
			{
				if (base.Read<IMAGE_NT_HEADER32>((long)((ulong)image_DOS_HEADER.e_lfanew), SeekOrigin.Begin, out image_NT_HEADER) && (long)image_NT_HEADER.Signature == 17744L)
				{
					if (image_NT_HEADER.OptionalHeader.Magic == 267)
					{
						if (image_NT_HEADER.OptionalHeader.DataDirectory[14].Size > 0U)
						{
							text = "Image contains a CLR runtime header. Currently only native binaries are supported; no .NET dependent libraries.";
						}
					}
					else
					{
						text = "File is of the PE32+ format. Currently support only extends to PE32 images. Either recompile the binary as x86, or choose a different target.";
					}
				}
				else
				{
					text = "Invalid NT header found in image.";
				}
			}
			else
			{
				text = "Invalid DOS Header found in image";
			}
			if (string.IsNullOrEmpty(text))
			{
				this.NTHeader = image_NT_HEADER;
				this.DOSHeader = image_DOS_HEADER;
				return;
			}
			base.Dispose();
			throw new ArgumentException(text);
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0004803F File Offset: 0x0004623F
		public IEnumerable<IMAGE_IMPORT_DESCRIPTOR> EnumImports()
		{
			IMAGE_DATA_DIRECTORY image_DATA_DIRECTORY = this.NTHeader.OptionalHeader.DataDirectory[1];
			if (image_DATA_DIRECTORY.Size > 0U)
			{
				uint num = this.GetPtrFromRVA(image_DATA_DIRECTORY.VirtualAddress);
				uint num2 = typeof(IMAGE_IMPORT_DESCRIPTOR).SizeOf();
				IMAGE_IMPORT_DESCRIPTOR image_IMPORT_DESCRIPTOR;
				while (base.Read<IMAGE_IMPORT_DESCRIPTOR>((long)((ulong)num), SeekOrigin.Begin, out image_IMPORT_DESCRIPTOR) && image_IMPORT_DESCRIPTOR.OriginalFirstThunk > 0U && image_IMPORT_DESCRIPTOR.Name > 0U)
				{
					yield return image_IMPORT_DESCRIPTOR;
					num += num2;
				}
			}
			yield break;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0004804F File Offset: 0x0004624F
		public IEnumerable<IMAGE_SECTION_HEADER> EnumSectionHeaders()
		{
			uint numberOfSections = (uint)this.NTHeader.FileHeader.NumberOfSections;
			long num = (long)((ulong)((uint)this.NTHeader.FileHeader.SizeOfOptionalHeader + typeof(IMAGE_FILE_HEADER).SizeOf() + 4U + this.DOSHeader.e_lfanew));
			uint num2 = typeof(IMAGE_SECTION_HEADER).SizeOf();
			uint num4;
			for (uint num3 = 0U; num3 < numberOfSections; num3 = num4 + 1U)
			{
				IMAGE_SECTION_HEADER image_SECTION_HEADER;
				if (base.Read<IMAGE_SECTION_HEADER>(num + (long)((ulong)(num3 * num2)), SeekOrigin.Begin, out image_SECTION_HEADER))
				{
					yield return image_SECTION_HEADER;
				}
				num4 = num3;
			}
			yield break;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00048060 File Offset: 0x00046260
		private IMAGE_SECTION_HEADER GetEnclosingSectionHeader(uint rva)
		{
			foreach (IMAGE_SECTION_HEADER image_SECTION_HEADER in this.EnumSectionHeaders())
			{
				if (rva >= image_SECTION_HEADER.VirtualAddress && rva < image_SECTION_HEADER.VirtualAddress + ((image_SECTION_HEADER.VirtualSize > 0U) ? image_SECTION_HEADER.VirtualSize : image_SECTION_HEADER.SizeOfRawData))
				{
					return image_SECTION_HEADER;
				}
			}
			throw new EntryPointNotFoundException("RVA does not exist within any of the current sections.");
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x000480E0 File Offset: 0x000462E0
		public uint GetPtrFromRVA(uint rva)
		{
			IMAGE_SECTION_HEADER enclosingSectionHeader = this.GetEnclosingSectionHeader(rva);
			return rva - (enclosingSectionHeader.VirtualAddress - enclosingSectionHeader.PointerToRawData);
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00048104 File Offset: 0x00046304
		public byte[] ToArray()
		{
			return base.GetUnderlyingData();
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x0004810C File Offset: 0x0004630C
		// (set) Token: 0x06000AD2 RID: 2770 RVA: 0x00048114 File Offset: 0x00046314
		public IMAGE_DOS_HEADER DOSHeader { get; private set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x0004811D File Offset: 0x0004631D
		// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x00048125 File Offset: 0x00046325
		public string FileLocation { get; private set; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0004812E File Offset: 0x0004632E
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x00048136 File Offset: 0x00046336
		public IMAGE_NT_HEADER32 NTHeader { get; private set; }
	}
}
