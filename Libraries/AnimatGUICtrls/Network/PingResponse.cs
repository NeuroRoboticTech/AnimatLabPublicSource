using System;
using System.Net;

namespace AnimatGuiCtrls.Network
{
	public class PingResponse
	{
		#region Properties

		private IPEndPoint serverEndPoint;
		public IPEndPoint ServerEndPoint
		{
			get { return serverEndPoint; }
			set { serverEndPoint = value; }
		}

		private IPEndPoint clientEndPoint;
		public IPEndPoint ClientEndPoint
		{
			get { return clientEndPoint; }
			set { clientEndPoint = value; }
		}

		private PingResponseType pingResult;
		public PingResponseType PingResult
		{
			get { return pingResult; }
			set { pingResult = value; }
		}

		private string errorMessage;
		public string ErrorMessage
		{
			get { return errorMessage; }
			set { errorMessage = value; }
		}

		private int packetsSent = Constants.InvalidInt;
		public int PacketsSent
		{
			get { return packetsSent; }
			set { packetsSent = value; }
		}

		private int packetsReceived = Constants.InvalidInt;
		public int PacketsReceived
		{
			get { return packetsReceived; }
			set { packetsReceived = value; }
		}

		public int Lost
		{
			get { return packetsSent - packetsReceived; }
		}

		private int minimumTime = Constants.InvalidInt;
		public int MinimumTime
		{
			get
			{
				//Check to see if the minimum time has already been calculated
				if (minimumTime == Constants.InvalidInt)
				{
					if (responseTimes == null || responseTimes.Length == 0)
					{
						minimumTime = -1;
					}
					else
					{
						minimumTime = responseTimes[0];
						for (int i = 1; i < responseTimes.Length; i++)
						{
							if (responseTimes[i] != Constants.InvalidInt && responseTimes[i] < minimumTime)
								minimumTime = responseTimes[i];
						}

						//Handle all ping responses failing (thus giving Contants.InvalidInt times)
						if (minimumTime == Constants.InvalidInt)
							minimumTime = -1;
					}
				}

				if (minimumTime == -1)
					return Constants.InvalidInt;
				else
					return minimumTime;
			}
		}

		private int maximumTime = Constants.InvalidInt;
		public int MaximumTime
		{
			get
			{
				//Check to see if the maximum time has already been calculated
				if (maximumTime == Constants.InvalidInt)
				{
					if (responseTimes == null || responseTimes.Length == 0)
					{
						minimumTime = -1;
					}
					else
					{
						maximumTime = responseTimes[0];
						for (int i = 1; i < responseTimes.Length; i++)
						{
							if (responseTimes[i] != Constants.InvalidInt && responseTimes[i] > maximumTime)
								maximumTime = responseTimes[i];
						}

						//Handle all ping responses failing (thus giving Contants.InvalidInt times)
						if (maximumTime == Constants.InvalidInt)
							maximumTime = -1;
					}
				}

				if (maximumTime == -1)
					return Constants.InvalidInt;
				else
					return maximumTime;
			}
		}

		private int averageTime = Constants.InvalidInt;
		public int AverageTime
		{
			get
			{
				//Check to see if the average time has already been calculated
				if (averageTime == Constants.InvalidInt)
				{
					averageTime = 0;
					int validPings = 0;
					for (int i = 0; i < responseTimes.Length; i++)
					{
						if (responseTimes[i] != Constants.InvalidInt)
						{
							averageTime += responseTimes[i];
							validPings++;
						}
					}

					//Handle all ping responses failing
					if (validPings == 0)
						averageTime = -1;
					else
						averageTime = (int)(averageTime / validPings);
				}

				if (averageTime == -1)
					return Constants.InvalidInt;
				else
					return averageTime;
			}
		}

		private int[] responseTimes;
		public int[] ResponseTimes
		{
			get { return responseTimes; }
			set { responseTimes = value; }
		}

		#endregion

		public PingResponse() { }

		public PingResponse(int expectedResponses)
		{
			this.responseTimes = new int[expectedResponses];
		}
		
		public PingResponse(int packetsSent, int packetsReceived, int[] responseTimes)
		{
			this.packetsSent = packetsSent;
			this.packetsReceived = packetsReceived;
			this.responseTimes = responseTimes;
		}
	}
}
