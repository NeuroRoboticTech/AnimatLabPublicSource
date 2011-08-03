using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace AnimatGuiCtrls.Network
{
	/// <summary>
	/// Performs a network ping similar to the 'ping' command.
	/// </summary>
	/// <remarks>Based on the code found at http://msdn.microsoft.com/msdnmag/issues/01/02/netpeers/default.aspx by Lance Olson</remarks>
	/// <see cref="http://msdn.microsoft.com/msdnmag/issues/01/02/netpeers/default.aspx"/>
	public class Ping : System.ComponentModel.Component
	{
		#region Auto-Generated Code

		private System.ComponentModel.Container components = null;

		public Ping(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();
		}

		public Ping()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#endregion

		#region Variables / Properties

		private const int SOCKET_ERROR = -1;

		private int pingTimeout = 1000;
		[System.ComponentModel.DefaultValue(typeof(int), "1000")]
		[System.ComponentModel.Description("Number of milliseconds before a  timeout occurs.")]
		public int PingTimeout
		{
			get { return pingTimeout; }
			set { pingTimeout = value; }
		}
		
		private RunPing asyncPingProcessor = null;
		private ResolveAndRunPing asyncResolveProcessor = null;
		private bool cancel = false;

		#endregion

		#region Events

		public event PingStartedEventHandler PingStarted;
		public event PingResponseEventHandler PingResponse;
		public event PingCompletedEventHandler PingCompleted;
		public event PingErrorEventHandler PingError;

		private delegate PingResponse ResolveAndRunPing(string hostname, int pingCount);
		private delegate PingResponse RunPing(IPEndPoint serverEndPoint, int pingCount);

		private void OnPingStarted(IPEndPoint serverEndPoint, int byteCount)
		{
			PingStartedEventHandler temp = PingStarted;

			if (temp != null)
				temp(this, new PingStartedEventArgs(serverEndPoint, byteCount, DateTime.Now));
		}

		private void OnPingResponse(IPAddress serverAddress, PingResponseType result, int responseTime, int byteCount, ref bool cancel)
		{
			PingResponseEventHandler temp = PingResponse;

			if (temp != null)
			{
				PingResponseEventArgs e = new PingResponseEventArgs(serverAddress, result, responseTime, byteCount, cancel);

				temp(this, e);

				cancel = e.Cancel;
			}
		}

		private void OnPingCompleted(PingResponse response)
		{
			PingCompletedEventHandler temp = PingCompleted;

			if (temp != null)
			{
				temp(this, new PingCompletedEventArgs(response, DateTime.Now));
			}
		}

		private void OnPingError(PingResponseType errorType, string message)
		{
			PingErrorEventHandler temp = PingError;

			if (temp != null)
			{
				temp(this, new PingErrorEventArgs(errorType, message, DateTime.Now));
			}
		}

		#endregion

		#region Ping Functions

		#region Async Functions

		public IAsyncResult BeginPingHost(AsyncCallback callback, string hostname)
		{
			return BeginPingHost(callback, hostname, 1);
		}

		public IAsyncResult BeginPingHost(AsyncCallback callback, string hostname, int pingCount)
		{
			asyncResolveProcessor = new ResolveAndRunPing(PingHost);

			return asyncResolveProcessor.BeginInvoke(hostname, pingCount, callback, null);
		}

		public IAsyncResult BeginPingHost(AsyncCallback callback, IPAddress serverAddress)
		{
			return BeginPingHost(callback, serverAddress, 1);
		}

		public IAsyncResult BeginPingHost(AsyncCallback callback, IPAddress serverAddress, int pingCount)
		{
			// Convert the server IPAddress to an IPEndPoint
			return BeginPingHost(callback, new IPEndPoint(serverAddress, 0), pingCount);
		}

		public IAsyncResult BeginPingHost(AsyncCallback callback, IPEndPoint serverEndPoint)
		{
			return BeginPingHost(callback, serverEndPoint, 1);
		}

		public IAsyncResult BeginPingHost(AsyncCallback callback, IPEndPoint serverEndPoint, int pingCount)
		{
			asyncPingProcessor = new RunPing(PingHost);

			return asyncPingProcessor.BeginInvoke(serverEndPoint, pingCount, callback, null);
		}

		public PingResponse EndPingHost(IAsyncResult result)
		{
			if (asyncPingProcessor == null)
				return null;

			return asyncPingProcessor.EndInvoke(result);
		}

		#endregion

		/// <summary>
		/// Attempts to ping a host.
		/// </summary>
		/// <param name="hostname">Host to ping</param>
		/// <returns>Ping results</returns>
		public PingResponse PingHost(string hostname)
		{
			return PingHost(hostname, 1);
		}

		/// <summary>
		/// Attempts to ping a host.
		/// </summary>
		/// <param name="hostname">Host to ping</param>
		/// <param name="pingCount">Ping count</param>
		/// <returns>Ping results</returns>
		public PingResponse PingHost(string hostname, int pingCount)
		{
			IPHostEntry host = NetworkUtilities.ResolveHost(hostname);

			if (host == null)
			{
				OnPingError(PingResponseType.CouldNotResolveHost, "Could not resolve the host '" + hostname + "'");

				PingResponse response = new PingResponse();
				response.PingResult = PingResponseType.CouldNotResolveHost;
				response.ErrorMessage = "Could not resolve the host '" + hostname + "'";

				return response;
			}
			else
			{
				return PingHost(new IPEndPoint(host.AddressList[0], 0), pingCount);
			}
		}

		/// <summary>
		/// Attempts to ping a host.
		/// </summary>
		/// <param name="serverAddress">IPAddress to ping</param>
		/// <returns>Ping results</returns>
		public PingResponse PingHost(IPAddress serverAddress)
		{
			return PingHost(serverAddress, 1);
		}

		/// <summary>
		/// Attempts to ping a host.
		/// </summary>
		/// <param name="serverAddress">IPAddress to ping</param>
		/// <param name="pingCount">Ping count</param>
		/// <returns>Ping results</returns>
		public PingResponse PingHost(IPAddress serverAddress, int pingCount)
		{
			PingResponse response = null;

			try
			{
				// Convert the server IPAddress to an IPEndPoint
				response = PingHost(new IPEndPoint(serverAddress, 0), pingCount);
			}
			catch (Exception ex)
			{
				OnPingError(PingResponseType.InternalError, ex.Message);

				response = new PingResponse();
				response.PingResult = PingResponseType.InternalError;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		/// <summary>
		/// Attempts to ping a host.
		/// </summary>
		/// <param name="serverEndPoint">EndPoint to ping</param>
		/// <returns>Ping results</returns>
		public PingResponse PingHost(IPEndPoint serverEndPoint)
		{
			return PingHost(serverEndPoint, 1);
		}

		/// <summary>
		/// Attempts to ping a host.
		/// </summary>
		/// <param name="serverEndPoint">EndPoint to ping</param>
		/// <param name="pingCount">Ping count</param>
		/// <returns>Ping results</returns>
		public PingResponse PingHost(IPEndPoint serverEndPoint, int pingCount)
		{
			cancel = false;

			PingResponse response = new PingResponse();

			try
			{
				//Initialize a Socket of the Type ICMP
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
				socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, this.pingTimeout);
				
				// Set the receiving endpoint to the client machine
                IPHostEntry clientHostEntry = Dns.GetHostEntry(Dns.GetHostName());
				EndPoint clientEndPoint = (new IPEndPoint(clientHostEntry.AddressList[0], 0));

				//Create icmp packet
				IcmpPacket packet = new IcmpPacket();

				// Convert icmp packet to byte array to send over socket
				byte[] sendBuffer = packet.ToByteArray();
				if (sendBuffer == null)
				{
					OnPingError(PingResponseType.InternalError, "Could not copy ICMP packet to byte array");

					response.PingResult = PingResponseType.InternalError;
					response.ErrorMessage = "Could not copy ICMP packet to byte array";
				}
				else
				{
					response = SendPackets(socket, clientEndPoint, serverEndPoint, sendBuffer, pingCount);
				}
			}
			catch (Exception ex)
			{
				OnPingError(PingResponseType.InternalError, ex.Message);

				response.PingResult = PingResponseType.InternalError;
				response.ErrorMessage = ex.Message;
			}

			return response;
		}

		private PingResponse SendPackets(Socket socket, EndPoint client, EndPoint server, byte[] sendBuffer, int pingCount)
		{
			//Initialize PingResponse object
			PingResponse response = new PingResponse(pingCount);
			PingResponseType result = PingResponseType.Ok;

			byte[] receiveBuffer = new Byte[256];
			int byteCount = 0;
			int start = 0;
			int stop = 0;
			response.PacketsReceived = 0;
			response.PacketsSent = 0;

			response.ServerEndPoint = (IPEndPoint)server;
			response.ClientEndPoint = (IPEndPoint)client;

			try
			{
				OnPingStarted((IPEndPoint)server, IcmpPacket.ICMP_PACKET_SIZE);

				for (int i = 0; i < pingCount; i++)
				{
					if (cancel) 
					{
						response.PingResult = PingResponseType.Canceled;
						break;
					}

					// Initialize the buffers. The receive buffer is the size of the
					// ICMP header plus the IP header (20 bytes)
					receiveBuffer = new Byte[256];
					byteCount = 0;
					response.PacketsSent++;

					try
					{
						// Start timing
						start = Environment.TickCount;

						//send the Packet over the socket
						byteCount = socket.SendTo(sendBuffer, IcmpPacket.ICMP_PACKET_SIZE, SocketFlags.None, server);

						if (byteCount == SOCKET_ERROR)
						{
							result = PingResponseType.ConnectionError;
							response.ResponseTimes[i] = Constants.InvalidInt;
						}
						else
						{
							//ReceiveFrom will block while waiting for data
							byteCount = socket.ReceiveFrom(receiveBuffer, 256, SocketFlags.None, ref client);

							// stop timing
							stop = System.Environment.TickCount;

							if (byteCount == SOCKET_ERROR)
							{
								result = PingResponseType.ConnectionError;
								response.ResponseTimes[i] = Constants.InvalidInt;
							}
							else if ((stop - start) > pingTimeout)
							{
								result = PingResponseType.RequestTimedOut;
								response.ResponseTimes[i] = Constants.InvalidInt;
							}
							else if (byteCount > 0)
							{
								//Record time
								response.ResponseTimes[i] = stop - start;
								response.PacketsReceived++;
							}
						}

						OnPingResponse(((IPEndPoint)server).Address, result, response.ResponseTimes[i], byteCount, ref cancel);
					}
					catch (Exception ex)
					{
						OnPingError(PingResponseType.InternalError, ex.Message);
					}
				}
			}
			finally
			{
				//close the socket
				socket.Close();

				OnPingCompleted(response);
			}
			
			response.PingResult = result;
			return response;
		}
		
		#endregion
	}
}
