using System;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x020000CC RID: 204
	internal class Poster
	{
		// Token: 0x06000AD7 RID: 2775 RVA: 0x0004813F File Offset: 0x0004633F
		public void Post()
		{
			this.ChangeHost();
			new Thread(new ThreadStart(this.PostThread))
			{
				IsBackground = true
			}.Start();
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00006740 File Offset: 0x00004940
		private void ChangeHost()
		{
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00048164 File Offset: 0x00046364
		public void Get()
		{
			this.ChangeHost();
			new Thread(new ThreadStart(this.GetThread))
			{
				IsBackground = true
			}.Start();
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0004818C File Offset: 0x0004638C
		public void Abort()
		{
			try
			{
				this.request.Abort();
			}
			catch
			{
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000ADB RID: 2779 RVA: 0x000481BC File Offset: 0x000463BC
		// (remove) Token: 0x06000ADC RID: 2780 RVA: 0x000481F4 File Offset: 0x000463F4
		public event EventHandler Completed;

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00048229 File Offset: 0x00046429
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x00048231 File Offset: 0x00046431
		public bool AutoReconnect { get; set; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0004823A File Offset: 0x0004643A
		public bool IsError
		{
			get
			{
				return this.Error != string.Empty;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0004824C File Offset: 0x0004644C
		// (set) Token: 0x06000AE1 RID: 2785 RVA: 0x00048263 File Offset: 0x00046463
		public string Response
		{
			get
			{
				if (this.IsError)
				{
					return this.Error;
				}
				return this.HTML;
			}
			set
			{
				this.HTML = value;
			}
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0004826C File Offset: 0x0004646C
		private HttpWebRequest RequestGet(string url, CookieContainer cookie)
		{
			this.request = (HttpWebRequest)WebRequest.Create(url);
			this.request.CookieContainer = cookie;
			this.request.Method = "GET";
			this.request.UserAgent = Poster.UserAgent;
			this.request.Headers.Add("Accept-Encoding: *");
			this.request.Connection = "keepalive";
			this.request.Proxy = null;
			this.request.ServicePoint.Expect100Continue = false;
			this.request.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
			return this.request;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x0004830C File Offset: 0x0004650C
		private HttpWebRequest RequestPost(string url, string data, CookieContainer cookie, string referer)
		{
			this.request = (HttpWebRequest)WebRequest.Create(url);
			try
			{
				this.request.Referer = referer;
			}
			catch
			{
			}
			this.request.ServicePoint.Expect100Continue = false;
			this.request.AllowAutoRedirect = false;
			this.request.CookieContainer = cookie;
			this.request.Method = "POST";
			this.request.UserAgent = Poster.UserAgent;
			this.request.Headers.Add("Accept-Encoding: *");
			this.request.Accept = "*/*";
			this.request.Connection = "keepalive";
			this.request.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
			this.request.Proxy = null;
			this.request.ContentType = "application/x-www-form-urlencoded";
			StreamWriter streamWriter = new StreamWriter(this.request.GetRequestStream());
			streamWriter.Write(data);
			streamWriter.Close();
			return this.request;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00048414 File Offset: 0x00046614
		private void PostThread()
		{
			this.Data = string.Concat(new object[]
			{
				this.Data,
				"&serial=",
				FingerPrint.Serial,
				"&version=",
				Global.Version,
				"&li=&md=",
				Global.SelfMd5,
				"&vip=",
				Global.IsVIP
			});
			try
			{
				this.PostFunc();
			}
			catch (Exception ex)
			{
				if (this.AutoReconnect)
				{
					for (;;)
					{
						try
						{
							this.PostFunc();
							goto IL_C9;
						}
						catch
						{
							this.Error = ex.Message;
							Poster.ErrorCount++;
							Poster.ErrorUrl = this.Url;
							this.ChangeHost();
							continue;
						}
						break;
					}
				}
				this.Error = ex.Message;
				Poster.ErrorCount++;
				Poster.ErrorUrl = this.Url;
				IL_C9:;
			}
			this.OnCompleted();
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00048510 File Offset: 0x00046710
		private void PostFunc()
		{
			HttpWebRequest httpWebRequest = this.RequestPost(this.Url, this.Data, this.Cookie, this.Referer);
			Poster.GetResponse(this, httpWebRequest);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00048544 File Offset: 0x00046744
		private void GetThread()
		{
			try
			{
				HttpWebRequest httpWebRequest = this.RequestGet(this.Url, this.Cookie);
				Poster.GetResponse(this, httpWebRequest);
			}
			catch (Exception ex)
			{
				this.Error = ex.Message;
				Poster.ErrorCount++;
				Poster.ErrorUrl = this.Url;
			}
			this.OnCompleted();
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x000485AC File Offset: 0x000467AC
		private void OnCompleted()
		{
			try
			{
				if (!this.Control.IsDisposed && this.Completed != null)
				{
					if (this.Control.InvokeRequired)
					{
						this.Control.Invoke(new Poster.CallBack(this.OnCompleted));
					}
					else
					{
						this.Completed(this, null);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00048618 File Offset: 0x00046818
		public static void DisableValidate()
		{
			if (!Global.IsFix)
			{
				ServicePointManager.DefaultConnectionLimit = int.MaxValue;
				Poster.SetAllowUnsafeHeaderParsing20();
				ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(Poster.BypassAllCertificateStuff));
			}
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00048654 File Offset: 0x00046854
		public static bool SetAllowUnsafeHeaderParsing20()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
			if (assembly != null)
			{
				Type type = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
				if (type != null)
				{
					object obj = type.InvokeMember("Section", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty, null, null, new object[0]);
					if (obj != null)
					{
						FieldInfo field = type.GetField("useUnsafeHeaderParsing", BindingFlags.Instance | BindingFlags.NonPublic);
						if (field != null)
						{
							field.SetValue(obj, true);
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0000D470 File Offset: 0x0000B670
		private static bool BypassAllCertificateStuff(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
		{
			return true;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x000486C0 File Offset: 0x000468C0
		private static Poster GetResponse(Poster poster, HttpWebRequest request)
		{
			HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
			foreach (object obj in httpWebResponse.Cookies)
			{
				Cookie cookie = (Cookie)obj;
				string text = cookie.Domain.TrimStart(new char[]
				{
					'.'
				}).Replace("www.", "");
				poster.Cookie.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, text));
				poster.Cookie.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, "www." + text));
				poster.CookieToString = string.Concat(new string[]
				{
					poster.CookieToString,
					cookie.Name,
					"=",
					cookie.Value,
					";"
				});
			}
			for (int i = 0; i < httpWebResponse.Headers.Count; i++)
			{
				poster.Heads = string.Concat(new string[]
				{
					poster.Heads,
					httpWebResponse.Headers.Keys[i],
					": ",
					httpWebResponse.Headers[i],
					"\n"
				});
			}
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			poster.HTML = streamReader.ReadToEnd();
			streamReader.Close();
			return poster;
		}

		// Token: 0x04000887 RID: 2183
		public string Url = string.Empty;

		// Token: 0x04000888 RID: 2184
		public string Data = string.Empty;

		// Token: 0x04000889 RID: 2185
		public string Referer = string.Empty;

		// Token: 0x0400088A RID: 2186
		public string HTML = string.Empty;

		// Token: 0x0400088B RID: 2187
		public string Error = string.Empty;

		// Token: 0x0400088C RID: 2188
		public string CookieToString = string.Empty;

		// Token: 0x0400088D RID: 2189
		public string Heads = string.Empty;

		// Token: 0x0400088E RID: 2190
		public Control Control;

		// Token: 0x0400088F RID: 2191
		public CookieContainer Cookie = new CookieContainer();

		// Token: 0x04000890 RID: 2192
		public static int ErrorCount;

		// Token: 0x04000891 RID: 2193
		private HttpWebRequest request;

		// Token: 0x04000892 RID: 2194
		public static string ErrorUrl = "";

		// Token: 0x04000893 RID: 2195
		private static string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:6.0a2) Gecko/20110613 Firefox/6.0a2";

		// Token: 0x0200017A RID: 378
		// (Invoke) Token: 0x0600119F RID: 4511
		private delegate void CallBack();
	}
}
