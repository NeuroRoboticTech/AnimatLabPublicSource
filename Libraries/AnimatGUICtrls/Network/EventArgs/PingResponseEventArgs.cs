using System;
using System.Net;

namespace AnimatGuiCtrls.Network
{
	public delegate void PingResponseEventHandler(object sender, PingResponseEventArgs e);

	public class PingResponseEventArgs : EventArgs
	{
		#region Properties

		private IPAddress serverAddress;
		public IPAddress ServerAddress
		{
			get { return serverAddress; }
		}

		private PingResponseType result;
		public PingResponseType Result
		{
			get { return result; }
		}

		private int responseTime;
		public int ResponseTime
		{
			get { return responseTime; }
		}

		private int byteCount;
		public int ByteCount
		{
			get { return byteCount; }
		}

		private bool cancel;
		public bool Cancel
		{
			get { return cancel; }
			set { cancel = value; }
		}

		#endregion

		public PingResponseEventArgs(IPAddress serverAddress, PingResponseType result, int responseTime, int byteCount, bool cancel)
		{
			this.serverAddress = serverAddress;
			this.result = result;
			this.responseTime = responseTime;
			this.byteCount = byteCount;
			this.cancel = cancel;
		}
	}
}
