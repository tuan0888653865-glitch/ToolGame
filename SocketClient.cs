using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ProtoBuf.Serializers;
using TinhKiemAuto.AutoControl;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x02000119 RID: 281
	public sealed class SocketClient : IDisposable
	{
		// Token: 0x06000F02 RID: 3842 RVA: 0x00072EC8 File Offset: 0x000710C8
		public SocketClient(IPEndPoint hostEndPoint)
		{
			this.hostEndPoint = hostEndPoint;
			this.autoConnectEvent = new AutoResetEvent(false);
			this.autoSendEvent = new AutoResetEvent(false);
			this.clientSocket = new Socket(this.hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			this.sendEventArgs = new SocketAsyncEventArgs();
			this.sendEventArgs.UserToken = this.clientSocket;
			this.sendEventArgs.RemoteEndPoint = this.hostEndPoint;
			this.sendEventArgs.Completed += this.OnSend;
			this.receiveEventArgs = new SocketAsyncEventArgs();
			this.receiveEventArgs.UserToken = new AsyncUserToken(this.clientSocket);
			this.receiveEventArgs.RemoteEndPoint = this.hostEndPoint;
			this.receiveEventArgs.SetBuffer(new byte[this.bufferSize], 0, this.bufferSize);
			this.receiveEventArgs.Completed += this.OnReceive;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00072FC8 File Offset: 0x000711C8
		public void Connect()
		{
			SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
			socketAsyncEventArgs.UserToken = this.clientSocket;
			socketAsyncEventArgs.RemoteEndPoint = this.hostEndPoint;
			socketAsyncEventArgs.Completed += this.OnConnect;
			this.clientSocket.ConnectAsync(socketAsyncEventArgs);
			this.autoConnectEvent.WaitOne();
			if (socketAsyncEventArgs.SocketError != SocketError.Success)
			{
				return;
			}
			if (!this.clientSocket.ReceiveAsync(this.receiveEventArgs))
			{
				this.ProcessReceive(this.receiveEventArgs);
			}
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00073046 File Offset: 0x00071246
		public void SendData(byte[] message)
		{
			this.Send(SocketClient.BuildMessage(message));
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00073054 File Offset: 0x00071254
		public static byte[] BuildMessage(byte[] data)
		{
			byte[] array = DataHelper.MAHOA(data, "e9b3390206d8dfc5ffc9b09284c0bbde");
			byte[] bytes = BitConverter.GetBytes(array.Length);
			byte[] array2 = new byte[bytes.Length + array.Length];
			bytes.CopyTo(array2, 0);
			array.CopyTo(array2, bytes.Length);
			return array2;
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00073096 File Offset: 0x00071296
		public void Disconnect()
		{
			this.clientSocket.Disconnect(false);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x000730A4 File Offset: 0x000712A4
		public void Send(byte[] message)
		{
			if (message != null)
			{
				this.sendEventArgs.SetBuffer(message, 0, message.Length);
				this.clientSocket.SendAsync(this.sendEventArgs);
				this.autoSendEvent.WaitOne();
			}
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x000730D7 File Offset: 0x000712D7
		private void OnConnect(object sender, SocketAsyncEventArgs e)
		{
			this.autoConnectEvent.Set();
			this.connected = (e.SocketError == SocketError.Success);
			bool flag = this.connected;
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x000730FB File Offset: 0x000712FB
		private void OnSend(object sender, SocketAsyncEventArgs e)
		{
			this.autoSendEvent.Set();
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00073109 File Offset: 0x00071309
		private void OnReceive(object sender, SocketAsyncEventArgs e)
		{
			this.ProcessReceive(e);
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00073114 File Offset: 0x00071314
		private void ProcessReceive(SocketAsyncEventArgs e)
		{
			if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
			{
				AsyncUserToken asyncUserToken = e.UserToken as AsyncUserToken;
				this.ProcessReceivedData(asyncUserToken.DataStartOffset, asyncUserToken.NextReceiveOffset - asyncUserToken.DataStartOffset + e.BytesTransferred, 0, asyncUserToken, e);
				asyncUserToken.NextReceiveOffset += e.BytesTransferred;
				if (asyncUserToken.NextReceiveOffset == e.Buffer.Length)
				{
					asyncUserToken.NextReceiveOffset = 0;
					if (asyncUserToken.DataStartOffset < e.Buffer.Length)
					{
						int num = e.Buffer.Length - asyncUserToken.DataStartOffset;
						Buffer.BlockCopy(e.Buffer, asyncUserToken.DataStartOffset, e.Buffer, 0, num);
						asyncUserToken.NextReceiveOffset = num;
					}
					asyncUserToken.DataStartOffset = 0;
				}
				e.SetBuffer(asyncUserToken.NextReceiveOffset, e.Buffer.Length - asyncUserToken.NextReceiveOffset);
				if (!asyncUserToken.Socket.ReceiveAsync(e))
				{
					this.ProcessReceive(e);
					return;
				}
			}
			else
			{
				this.ProcessError(e);
			}
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00073210 File Offset: 0x00071410
		private void ProcessReceivedData(int dataStartOffset, int totalReceivedDataSize, int alreadyProcessedDataSize, AsyncUserToken token, SocketAsyncEventArgs e)
		{
			if (alreadyProcessedDataSize >= totalReceivedDataSize)
			{
				return;
			}
			if (token.MessageSize == null)
			{
				if (totalReceivedDataSize > 4)
				{
					byte[] array = new byte[4];
					Buffer.BlockCopy(e.Buffer, dataStartOffset, array, 0, 4);
					int value = BitConverter.ToInt32(array, 0);
					token.MessageSize = new int?(value);
					token.DataStartOffset = dataStartOffset + 4;
					this.ProcessReceivedData(token.DataStartOffset, totalReceivedDataSize, alreadyProcessedDataSize + 4, token, e);
					return;
				}
			}
			else
			{
				int value2 = token.MessageSize.Value;
				if (totalReceivedDataSize - alreadyProcessedDataSize >= value2)
				{
					byte[] array2 = new byte[value2];
					Buffer.BlockCopy(e.Buffer, dataStartOffset, array2, 0, value2);
					this.ProcessMessage(array2);
					token.DataStartOffset = dataStartOffset + value2;
					token.MessageSize = null;
					this.ProcessReceivedData(token.DataStartOffset, totalReceivedDataSize, alreadyProcessedDataSize + value2, token, e);
				}
			}
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x000732EC File Offset: 0x000714EC
		private void ProcessMessage(byte[] messageData)
		{
			byte[] TMPDATA = DataHelper.GIAIMA(messageData, "e9b3390206d8dfc5ffc9b09284c0bbde");
			if (TMPDATA.Length != 0)
			{
				new Thread(delegate()
				{
					this.DoWork(TMPDATA);
				})
				{
					IsBackground = true
				}.Start();
			}
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00073340 File Offset: 0x00071540
		public void DoWork(byte[] dataBuf)
		{
			try
			{
				PacketDef packetDef = new PacketDef();
				packetDef = DataHelper.BytesToObject<PacketDef>(dataBuf, 0, dataBuf.Length);
				if (packetDef.IDPacket == 1000)
				{
					this.LoginProsecc(packetDef.data);
				}
			}
			catch (Exception ex)
			{
				LogManager.WriteLog(LogTypes.Error, ex.ToString());
			}
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x00073398 File Offset: 0x00071598
		public void LoginProsecc(byte[] dataBuf)
		{
			PacketSend packetSend = DataHelper.BytesToObject<PacketSend>(dataBuf, 0, dataBuf.Length);
			string hardwareID = packetSend.HardwareID;
			Console.Write("NHẬN ĐƯỢC TỪ PHÍA MÁY CHỦ :" + hardwareID);
			foreach (KeyValuePair<string, AutoReport> keyValuePair in packetSend.DanhSachGame)
			{
				Global.SetInfo(keyValuePair.Value);
			}
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00073410 File Offset: 0x00071610
		private void ProcessError(SocketAsyncEventArgs e)
		{
			Socket socket = e.UserToken as Socket;
			if (socket != null && socket.Connected)
			{
				try
				{
					socket.Shutdown(SocketShutdown.Both);
				}
				catch (Exception)
				{
				}
				finally
				{
					if (socket.Connected)
					{
						socket.Close();
					}
				}
			}
			this.connected = false;
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00073474 File Offset: 0x00071674
		public void Dispose()
		{
			this.autoConnectEvent.Close();
			if (this.clientSocket.Connected)
			{
				this.clientSocket.Close();
			}
		}

		// Token: 0x04000C97 RID: 3223
		private int bufferSize = 60000;

		// Token: 0x04000C98 RID: 3224
		private const int MessageHeaderSize = 4;

		// Token: 0x04000C99 RID: 3225
		private Socket clientSocket;

		// Token: 0x04000C9A RID: 3226
		public bool connected;

		// Token: 0x04000C9B RID: 3227
		private IPEndPoint hostEndPoint;

		// Token: 0x04000C9C RID: 3228
		private AutoResetEvent autoConnectEvent;

		// Token: 0x04000C9D RID: 3229
		private AutoResetEvent autoSendEvent;

		// Token: 0x04000C9E RID: 3230
		private SocketAsyncEventArgs sendEventArgs;

		// Token: 0x04000C9F RID: 3231
		private SocketAsyncEventArgs receiveEventArgs;
	}
}
