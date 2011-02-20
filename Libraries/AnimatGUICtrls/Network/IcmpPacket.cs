using System;

namespace AnimatGuiCtrls.Network
{
	internal class IcmpPacket
	{
		public const int ICMP_ECHO = 8;
		public const int PING_DATA_SIZE = 32; //sizeof(IcmpPacket) - 8;
		public const int ICMP_PACKET_SIZE = PING_DATA_SIZE + 8;

		private Byte Type;    // type of message
		private Byte SubCode;    // type of sub code
		private UInt16 CheckSum;   // ones complement CalculateChecksum of struct
		private UInt16 Identifier;      // identifier
		private UInt16 SequenceNumber;     // sequence number  
		private Byte[] Data;

		public IcmpPacket()
		{
			// Construct the this to send
			this.Type = ICMP_ECHO; //8
			this.SubCode = 0;
			this.CheckSum = UInt16.Parse("0");
			this.Identifier = UInt16.Parse("45");
			this.SequenceNumber = UInt16.Parse("0");

			this.Data = new Byte[PING_DATA_SIZE];

			//Initilize the Packet.Data
			for (int i = 0; i < this.Data.Length; i++)
				this.Data[i] = (byte)'#';

			//Create initial checksum
			UpdateChecksum();
		}

		/// <summary>
		/// Converts an IcmpPacket to a byte array.
		/// </summary>
		/// <returns>Byte array of contents of ICMP packet</returns>
		public byte[] ToByteArray()
		{
			//Variable to hold the total Packet size
			byte[] buffer = new byte[ICMP_PACKET_SIZE];
			int index = 0;

			byte[] b_type = new byte[1];
			b_type[0] = (this.Type);

			byte[] b_code = new byte[1];
			b_code[0] = (this.SubCode);

			byte[] b_cksum = BitConverter.GetBytes(this.CheckSum);
			byte[] b_id = BitConverter.GetBytes(this.Identifier);
			byte[] b_seq = BitConverter.GetBytes(this.SequenceNumber);

			Array.Copy(b_type, 0, buffer, index, b_type.Length);
			index += b_type.Length;

			Array.Copy(b_code, 0, buffer, index, b_code.Length);
			index += b_code.Length;

			Array.Copy(b_cksum, 0, buffer, index, b_cksum.Length);
			index += b_cksum.Length;

			Array.Copy(b_id, 0, buffer, index, b_id.Length);
			index += b_id.Length;

			Array.Copy(b_seq, 0, buffer, index, b_seq.Length);
			index += b_seq.Length;

			// copy the data	        
			Array.Copy(this.Data, 0, buffer, index, PING_DATA_SIZE);
			index += PING_DATA_SIZE;

			if (index != ICMP_PACKET_SIZE) //sizeof(IcmpPacket)
				return null;
			else
				return buffer;
		}

		/// <summary>
		/// Converts ICMP packet to UInt16 array
		/// </summary>
		/// <returns>UInt16 array of contents of ICMP packet</returns>
		public UInt16[] ToUInt16Array()
		{
			//Get the Half size of the Packet
			int checksumBufferLength = (int)Math.Ceiling((double)ICMP_PACKET_SIZE / 2);

			//Create byte array
			byte[] buffer = this.ToByteArray();
			if (buffer == null)
				return null;

			//Create a UInt16 Array
			UInt16[] checksumBuffer = new UInt16[checksumBufferLength];

			//Code to initialize the Uint16 array 
			int icmpHeaderBufferIndex = 0;
			for (int i = 0; i < checksumBufferLength; i++)
			{
				checksumBuffer[i] = BitConverter.ToUInt16(buffer, icmpHeaderBufferIndex);
				icmpHeaderBufferIndex += 2;
			}

			return checksumBuffer;
		}

		/// <summary>
		/// This Method has the algorithm to make the checksum
		/// </summary>
		public void UpdateChecksum()
		{
			UInt16[] buffer;
			int cksum = 0;
			int counter = 0;
			int size = 0;

			buffer = this.ToUInt16Array();
			if (buffer == null)
				throw new Exception("Unable to create UInt16 array.  Please check packet properties.");

			size = buffer.Length;

			while (size > 0)
			{
				UInt16 val = buffer[counter];

				cksum += Convert.ToInt32(buffer[counter]);
				counter += 1;
				size -= 1;
			}

			cksum = (cksum >> 16) + (cksum & 0xffff);
			cksum += (cksum >> 16);

			this.CheckSum = (UInt16)(~cksum);
		}
	}
}
