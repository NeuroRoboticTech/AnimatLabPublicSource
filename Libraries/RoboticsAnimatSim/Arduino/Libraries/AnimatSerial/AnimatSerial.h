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

#ifndef AnimatSerial_h
#define AnimatSerial_h

#include "HardwareSerial.h"

#define HEADER_SIZE 5
#define DATA_SIZE 6
#define FOOTER_SIZE 1

#define MAX_ANIMAT_BUFFER 128

#define PACKET_INFO_SIZE 6  //Size of the header, packet size, message id, and checksum in bytes
#define START_MESSAGE_INFO_BYTE 5
#define DATA_SIZE 6	
	
class AnimatData
{
public:
	union id_tag {
	 unsigned char bval[2];
	 unsigned int ival;
	} id;
	
	union value_tag {
	 unsigned char bval[4];
	 float fval;
	} value;
	
	//unsigned int m_id;
	//float m_value;

	AnimatData() {
		id.ival = 0;
		value.fval = 0;
	}
};

/* the Commander will send out a frame at about 30hz, this class helps decipher the output. */
class AnimatSerial
{    
  public:
    AnimatSerial(); 
    AnimatSerial(HardwareSerial *ss, unsigned int inTotal, unsigned int outTotal); 
	~AnimatSerial();
	
    void begin(unsigned long baud);
    int readMsgs();         // must be called regularly to clean out Serial buffer
	void writeMsgs();
	void writeAllMsgs();
	void writeResendMsg();
	
	bool isChanged();
	bool isChanged(unsigned int id);
	bool getData(unsigned int index, AnimatData &data);
	bool addData(unsigned int id, float val);
	
	unsigned int getInDataTotal() {return inDataTotal;}
	unsigned int getOutDataTotal() {return outDataTotal;}
	
	void clearChanged();

	private:

	void setInDataValue(int id, float val);

	void clearInData();
	void clearOutData();

	AnimatData *inData;
	AnimatData *outData;
	bool *changed;
	bool dataChanged;
	
	unsigned int inDataTotal;
	unsigned int outDataTotal;

	unsigned int inDataCount;
	unsigned int outDataCount;

	// internal variables used for reading messages
    unsigned char vals[MAX_ANIMAT_BUFFER];  // temporary values, moved after we confirm checksum
    int index;              // -1 = waiting for new packet
    int checksum;
    unsigned char status; 	
	
	int messageID;
	int packetSize;

	union size_tag {
	 unsigned char bval[2];
	 unsigned int ival;
	} size;
	
	union id_tag {
	 unsigned char bval[2];
	 unsigned int ival;
	} id;
	
	union value_tag {
	 unsigned char bval[4];
	 float fval;
	} value;
	
	HardwareSerial *stream;
};

#endif
