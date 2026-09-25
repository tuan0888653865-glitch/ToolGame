using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace TinhKiemAuto
{
	// Token: 0x020000BE RID: 190
	internal class ModifyRegistry
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x000433E6 File Offset: 0x000415E6
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x000433EE File Offset: 0x000415EE
		public bool ShowError
		{
			get
			{
				return this.showError;
			}
			set
			{
				this.showError = value;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x000433F7 File Offset: 0x000415F7
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x000433FF File Offset: 0x000415FF
		public string SubKey
		{
			get
			{
				return this.subKey;
			}
			set
			{
				this.subKey = value;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x00043408 File Offset: 0x00041608
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x00043410 File Offset: 0x00041610
		public RegistryKey BaseRegistryKey
		{
			get
			{
				return this.baseRegistryKey;
			}
			set
			{
				this.baseRegistryKey = value;
			}
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0004341C File Offset: 0x0004161C
		public string Read(string KeyName)
		{
			RegistryKey registryKey = this.baseRegistryKey.OpenSubKey(this.subKey);
			if (registryKey == null)
			{
				return null;
			}
			string result;
			try
			{
				result = (string)registryKey.GetValue(KeyName.ToUpper());
			}
			catch (Exception e)
			{
				this.ShowErrorMessage(e, "Reading registry " + KeyName.ToUpper());
				result = null;
			}
			return result;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00043484 File Offset: 0x00041684
		public bool Write(string KeyName, object Value)
		{
			bool result;
			try
			{
				this.baseRegistryKey.CreateSubKey(this.subKey).SetValue(KeyName.ToUpper(), Value);
				result = true;
			}
			catch (Exception e)
			{
				this.ShowErrorMessage(e, "Writing registry " + KeyName.ToUpper());
				result = false;
			}
			return result;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x000434E0 File Offset: 0x000416E0
		public bool DeleteKey(string KeyName)
		{
			bool result;
			try
			{
				RegistryKey registryKey = this.baseRegistryKey.CreateSubKey(this.subKey);
				if (registryKey == null)
				{
					result = true;
				}
				else
				{
					registryKey.DeleteValue(KeyName);
					result = true;
				}
			}
			catch (Exception e)
			{
				this.ShowErrorMessage(e, "Deleting SubKey " + this.subKey);
				result = false;
			}
			return result;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00043540 File Offset: 0x00041740
		public bool DeleteSubKeyTree()
		{
			bool result;
			try
			{
				RegistryKey registryKey = this.baseRegistryKey;
				if (registryKey.OpenSubKey(this.subKey) != null)
				{
					registryKey.DeleteSubKeyTree(this.subKey);
				}
				result = true;
			}
			catch (Exception e)
			{
				this.ShowErrorMessage(e, "Deleting SubKey " + this.subKey);
				result = false;
			}
			return result;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x000435A0 File Offset: 0x000417A0
		public int SubKeyCount()
		{
			int result;
			try
			{
				RegistryKey registryKey = this.baseRegistryKey.OpenSubKey(this.subKey);
				if (registryKey != null)
				{
					result = registryKey.SubKeyCount;
				}
				else
				{
					result = 0;
				}
			}
			catch (Exception e)
			{
				this.ShowErrorMessage(e, "Retriving subkeys of " + this.subKey);
				result = 0;
			}
			return result;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x000435FC File Offset: 0x000417FC
		public int ValueCount()
		{
			int result;
			try
			{
				RegistryKey registryKey = this.baseRegistryKey.OpenSubKey(this.subKey);
				if (registryKey != null)
				{
					result = registryKey.ValueCount;
				}
				else
				{
					result = 0;
				}
			}
			catch (Exception e)
			{
				this.ShowErrorMessage(e, "Retriving keys of " + this.subKey);
				result = 0;
			}
			return result;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00043658 File Offset: 0x00041858
		private void ShowErrorMessage(Exception e, string Title)
		{
			if (this.showError)
			{
				MessageBox.Show(e.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x040007B8 RID: 1976
		private bool showError;

		// Token: 0x040007B9 RID: 1977
		private string subKey = "SOFTWARE\\" + Application.ProductName.ToUpper();

		// Token: 0x040007BA RID: 1978
		private RegistryKey baseRegistryKey = Registry.LocalMachine;
	}
}
