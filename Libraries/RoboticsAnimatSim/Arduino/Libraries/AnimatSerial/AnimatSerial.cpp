/*
  AnimatSerial.cpp - Library for interfacing with Animat serial stream
  Copyright (c) 2015 David Cofer, NeuroRobotic Technologies.  All right reserved.

  This library is free software; you can redistribute it and/or
  modify it under the terms of the GNU Lesser General Public
  License as published by the Free Software Foundation; either
  version 2.1 of the License, or (at your option) any later version.

  This library is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
  Lesser General Public License for more details.

  You should have received a copy of the GNU Lesser General Public
  License along with this library; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

#include <Arduino.h>
#include "AnimatSerial.h"

/* Constructor */
AnimatSerial::AnimatSerial(){
    index = -1;
    status = 0;
	stream = NULL;
	
	inDataTotal = 0;
	outDataTotal = 0;
	inDataCount = 0;
	outDataCount = 0;
	inData = NULL;
	outData = NULL;
	changed = NULL;
	dataChanged = false;
	
	messageID  = -1;
	packetSize = 0;
}

AnimatSerial::AnimatSerial(HardwareSerial *ss, unsigned int inTotal, unsigned int outTotal){
    index = -1;
    status = 0;
	stream = ss;
	
	inDataTotal = inTotal;
	outDataTotal = outTotal;	
	inDataCount = 0;
	outDataCount = 0;
	
	messageID  = -1;
	packetSize = 0;
	
	if(inDataTotal > 0)
	{
		inData = new AnimatData[inDataTotal];
		changed = new bool[inDataTotal];
	}
		
	if(outDataTotal > 0)
		outData = new AnimatData[outDataTotal];
		
	clearInData();
	clearChanged();
	clearOutData();
}

AnimatSerial::~AnimatSerial()
{
	if(inData)
	{
		delete[] inData;
		inData = NULL;
	}

	if(outData)
	{
		delete[] outData;
		outData = NULL;
	}
}

bool AnimatSerial::isChanged()
{
	return dataChanged;
}

void AnimatSerial::clearInData()
{
	if(inData)
	{
		for(int i=0; i<inDataTotal; i++)
		{
			inData[i].value.fval = 0;
			changed[i] = 0;
		}
	}
}

void AnimatSerial::clearOutData()
{
	if(outData)
	{
		for(int i=0; i<outDataTotal; i++)
			outData[i].value.fval = 0;
	}
}

void AnimatSerial::clearChanged()
{
	dataChanged = false;
	if(changed)
	{
		for(int i=0; i<inDataTotal; i++)
			changed[i] = false;
	}
}	

bool AnimatSerial::isChanged(unsigned int index)
{
	if(changed && index < inDataTotal)
		return changed[index];
	else
		return false;
}

bool AnimatSerial::getData(unsigned int index, AnimatData &data)
{
	if(index < inDataTotal)
	{
		data = inData[index];
		return true;
	}
	else
		return false;
}

bool AnimatSerial::addData(unsigned int id, float val)
{
	if(outDataCount < outDataTotal)
	{
		outData[0].id.ival = id;
		outData[0].value.fval = val;

		//Serial.print("Add Data: ");
		//Serial.print(outData[0].id.ival, HEX);
		//Serial.print(", ");
		//Serial.print(outData[0].id.bval[0], HEX);
		//Serial.print(", ");
		//Serial.print(outData[0].id.bval[1], HEX);
		//Serial.print(", ");
		//Serial.print(outData[0].value.fval);
		//Serial.println("");
			
		outDataCount++;
		return true;
	}
	else
		return false;
}

void AnimatSerial::setInDataValue(int id, float val) 
{
	if(inData && changed && id < inDataTotal)
	{
		inData[id].value.fval = val;
		changed[id] = true;
		dataChanged = true;
	}
}

void AnimatSerial::begin(unsigned long baud){
	if(stream != NULL)
		stream->begin(baud);
	else
	{
		stream = &Serial;
		stream->begin(baud);
	}
}

/* process messages coming from CommanderHS 
 *  format = 0xFF RIGHT_H RIGHT_V LEFT_H LEFT_V BUTTONS EXT CHECKSUM */
int AnimatSerial::readMsgs()
{
	if(stream != NULL) 
	{	
		while(stream->available() > 0)
		{
			//Get first header byte
			if(index == -1)
			{        
				// looking for new packet
				if(stream->read() == 0xff)
				{
					vals[0] = 0xff;
					checksum = (int) vals[0];
					index = 1;
					messageID  = -1;
					packetSize = 0;
				}
			}
			//Get second header byte
			else if(index == 1)
			{
				vals[index] = (unsigned char) stream->read();
				if(vals[index] == 0xff)
				{            
					checksum += (int) vals[index];
					index++;
				}
				else
					index = -1;  //Start over if the second byte is not 0xff
			}
			//Get the message ID
			else if(index==2)
			{
				vals[index] = (unsigned char) stream->read();
				messageID = vals[index];

				checksum += (int) vals[index];
				index++;
			}
			//Get the message size byte 1
			else if(index==3)
			{
				vals[index] = (unsigned char) stream->read();
				packetSize = vals[index];

				checksum += (int) vals[index];
				index++;
			}
			//Get the message size byte 2
			else if(index==4)
			{
				vals[index] = (unsigned char) stream->read();
				packetSize += (vals[index] << 8);

				//If the message size is greater than 128 then something is wrong. Start over.
				if(packetSize > 128)
					index = -1;
				else
				{
					checksum += (int) vals[index];
					index++;
				}
			}
			else if(index > 4 && index <= packetSize)
			{
				vals[index] = (unsigned char) stream->read();

				if(index == (packetSize-1))
				{ // packet complete
					//The transmitted checksum is the last entry
					int iChecksum = vals[index];

					if(checksum%256 != iChecksum)
					{
						//Serial.println("Invalid Checksum. Sending resend msg.");
						writeResendMsg();
						// packet error!
						index = -1;
					}
					else
					{
						//Serial.print("Found msg ID: ");
						//Serial.println(messageID);
						if(messageID == 2)
							writeAllMsgs();
						else if(messageID == 1)
						{
							int iStop = packetSize - 1; //Exclude the checksum at the end

							for(int iIdx=START_MESSAGE_INFO_BYTE; iIdx<iStop; iIdx+=DATA_SIZE)
							{
								id.bval[0] = vals[iIdx];
								id.bval[1] = vals[iIdx+1];
								value.bval[0] = vals[iIdx+2];
								value.bval[1] = vals[iIdx+3];
								value.bval[2] = vals[iIdx+4];
								value.bval[3] = vals[iIdx+5];

								//Serial.print("A. Received Data ID: ");
								//Serial.print(id.ival);
								//Serial.print(", val: ");
								//Serial.println(value.fval, 8);
								//Serial.print(", Val: ");
								//Serial.print(value.bval[0]);
								//Serial.print(", ");
								//Serial.print(value.bval[1]);
								//Serial.print(", ");
								//Serial.print(value.bval[2]);
								//Serial.print(", ");
								//Serial.println(value.bval[3]);
								setInDataValue(id.ival, value.fval);
							}
						}
					}

					index = -1;
					stream->flush();
					return 1;
				}
				else
				{
					checksum += (int) vals[index];
					index++;
				}

				if(index >= MAX_ANIMAT_BUFFER)
					index = -1;
			}

		}
	}
	
	return 0;
}

void AnimatSerial::writeMsgs(){
	if(stream != NULL && outDataCount > 0) {
		int checksum = 0xFF + 0xFF + 0x01;
		
		//First write the header
		stream->write((byte) 0xFF);
		stream->write((byte) 0xFF);
		stream->write((byte) 0x01);
		//Serial.print("0xFF, 0xFF, 0x01, ");

		size.ival = HEADER_SIZE + (DATA_SIZE * outDataCount) + FOOTER_SIZE;
		stream->write((byte) size.bval[0]);
		checksum += size.bval[0];
		stream->write((byte) size.bval[1]);
		checksum += size.bval[1];
		
		//Serial.print(size.bval[0], HEX);
		//Serial.print(", ");
		//Serial.print(size.bval[1], HEX);
		//Serial.print(", ");
		
		for(int i=0; i<outDataCount; i++) {
			stream->write((byte) outData[i].id.bval[0]); //
			stream->write((byte) outData[i].id.bval[1]);
			stream->write((byte) outData[i].value.bval[0]);
			stream->write((byte) outData[i].value.bval[1]);
			stream->write((byte) outData[i].value.bval[2]);
			stream->write((byte) outData[i].value.bval[3]);
			
			checksum += outData[i].id.bval[0];
			checksum += outData[i].id.bval[1];
			checksum += outData[i].value.bval[0];
			checksum += outData[i].value.bval[1];
			checksum += outData[i].value.bval[2];
			checksum += outData[i].value.bval[3];

			//Serial.print(outData[i].id.ival, HEX);
			//Serial.print(", ");
			//Serial.print(outData[i].id.bval[0], HEX);
			//Serial.print(", ");
			//Serial.print(outData[i].id.bval[1], HEX);
			//Serial.print(", ");
			//Serial.print(outData[i].value.bval[0], HEX);
			//Serial.print(", ");
			//Serial.print(outData[i].value.bval[1], HEX);
			//Serial.print(", ");
			//Serial.print(outData[i].value.bval[2], HEX);
			//Serial.print(", ");
			//Serial.print(outData[i].value.bval[3], HEX);
			//Serial.print(", ");
		}

		unsigned char bchecksum = (unsigned char) (checksum%256);
		stream->write(bchecksum);

		//Serial.println(checksum, HEX);
		//Serial.println("");
		//Serial.println(bchecksum, HEX);
		//Serial.println("");
		//Serial.print("Size: ");
		//Serial.println(size.ival);
		
		outDataCount = 0;
	}
}

void AnimatSerial::writeResendMsg(){
	if(stream != NULL) {
		int checksum = 0xFF + 0xFF + 0x02;
		
		//First write the header
		stream->write((byte) 0xFF);
		stream->write((byte) 0xFF);
		stream->write((byte) 0x02);
		//Serial.print("Writing Resend message 0xFF, 0xFF, 0x02, ");

		size.ival = HEADER_SIZE + FOOTER_SIZE;
		stream->write((byte) size.bval[0]);
		checksum += size.bval[0];
		stream->write((byte) size.bval[1]);
		checksum += size.bval[1];
		
		//Serial.print(size.bval[0], HEX);
		//Serial.print(", ");
		//Serial.print(size.bval[1], HEX);
		//Serial.print(", ");

		unsigned char bchecksum = (unsigned char) (checksum%256);
		stream->write(bchecksum);

		//Serial.println(checksum, HEX);
		//Serial.println("");
		//Serial.println(bchecksum, HEX);
		//Serial.println("");
		//Serial.print("Size: ");
		//Serial.println(size.ival);
	}
}

void AnimatSerial::writeAllMsgs(){
	//Serial.println("Writing resend message");
	if(stream != NULL && outDataTotal > 0) {
		for(int i=0; i<outDataTotal; i++)
			addData(i, outData[i].value.fval);
			
		writeMsgs();
	}
}