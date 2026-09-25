using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x02000091 RID: 145
	public static class FormUpload
	{
		// Token: 0x06000674 RID: 1652 RVA: 0x00024C50 File Offset: 0x00022E50
		public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
		{
			string text = string.Format("----------{0:N}", Guid.NewGuid());
			string contentType = "multipart/form-data; boundary=" + text;
			byte[] multipartFormData = FormUpload.GetMultipartFormData(postParameters, text);
			return FormUpload.PostForm(postUrl, userAgent, contentType, multipartFormData);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00024C90 File Offset: 0x00022E90
		private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
		{
			HttpWebRequest httpWebRequest = WebRequest.Create(postUrl) as HttpWebRequest;
			if (httpWebRequest == null)
			{
				throw new NullReferenceException("request is not a http request");
			}
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = contentType;
			httpWebRequest.UserAgent = userAgent;
			httpWebRequest.CookieContainer = new CookieContainer();
			httpWebRequest.ContentLength = (long)formData.Length;
			httpWebRequest.ServicePoint.Expect100Continue = false;
			using (Stream requestStream = httpWebRequest.GetRequestStream())
			{
				requestStream.Write(formData, 0, formData.Length);
				requestStream.Close();
			}
			return httpWebRequest.GetResponse() as HttpWebResponse;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00024D30 File Offset: 0x00022F30
		private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
		{
			Stream stream = new MemoryStream();
			bool flag = false;
			foreach (KeyValuePair<string, object> keyValuePair in postParameters)
			{
				if (flag)
				{
					stream.Write(FormUpload.encoding.GetBytes("\r\n"), 0, FormUpload.encoding.GetByteCount("\r\n"));
				}
				flag = true;
				if (keyValuePair.Value is FormUpload.FileParameter)
				{
					FormUpload.FileParameter fileParameter = (FormUpload.FileParameter)keyValuePair.Value;
					string s = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n", new object[]
					{
						boundary,
						keyValuePair.Key,
						fileParameter.FileName ?? keyValuePair.Key,
						fileParameter.ContentType ?? "application/octet-stream"
					});
					stream.Write(FormUpload.encoding.GetBytes(s), 0, FormUpload.encoding.GetByteCount(s));
					stream.Write(fileParameter.File, 0, fileParameter.File.Length);
				}
				else
				{
					string s2 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}", boundary, keyValuePair.Key, keyValuePair.Value);
					stream.Write(FormUpload.encoding.GetBytes(s2), 0, FormUpload.encoding.GetByteCount(s2));
				}
			}
			string s3 = "\r\n--" + boundary + "--\r\n";
			stream.Write(FormUpload.encoding.GetBytes(s3), 0, FormUpload.encoding.GetByteCount(s3));
			stream.Position = 0L;
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			stream.Close();
			return array;
		}

		// Token: 0x04000426 RID: 1062
		private static readonly Encoding encoding = Encoding.UTF8;

		// Token: 0x02000168 RID: 360
		public class FileParameter
		{
			// Token: 0x17000402 RID: 1026
			// (get) Token: 0x06001176 RID: 4470 RVA: 0x00077A38 File Offset: 0x00075C38
			// (set) Token: 0x06001177 RID: 4471 RVA: 0x00077A40 File Offset: 0x00075C40
			public byte[] File { get; set; }

			// Token: 0x17000403 RID: 1027
			// (get) Token: 0x06001178 RID: 4472 RVA: 0x00077A49 File Offset: 0x00075C49
			// (set) Token: 0x06001179 RID: 4473 RVA: 0x00077A51 File Offset: 0x00075C51
			public string FileName { get; set; }

			// Token: 0x17000404 RID: 1028
			// (get) Token: 0x0600117A RID: 4474 RVA: 0x00077A5A File Offset: 0x00075C5A
			// (set) Token: 0x0600117B RID: 4475 RVA: 0x00077A62 File Offset: 0x00075C62
			public string ContentType { get; set; }

			// Token: 0x0600117C RID: 4476 RVA: 0x00077A6B File Offset: 0x00075C6B
			public FileParameter(byte[] file) : this(file, null)
			{
			}

			// Token: 0x0600117D RID: 4477 RVA: 0x00077A75 File Offset: 0x00075C75
			public FileParameter(byte[] file, string filename) : this(file, filename, null)
			{
			}

			// Token: 0x0600117E RID: 4478 RVA: 0x00077A80 File Offset: 0x00075C80
			public FileParameter(byte[] file, string filename, string contenttype)
			{
				this.File = file;
				this.FileName = filename;
				this.ContentType = contenttype;
			}
		}
	}
}
