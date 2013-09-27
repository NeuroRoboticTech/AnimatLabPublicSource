/**
\file	ExternalStimulus.h

\brief	Declares the external stimulus base class. 
**/

#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::ExternalStimuli

	\brief	Contains all of the classes that are used to generate external stimuli to body parts, joints, or neural items. 
	**/
	namespace ExternalStimuli
	{
		/**
		\brief	External stimulus base class. 

		\details This is the base class for all stimulus types. If you want to create a new stimulus it needs to be
		derived from this base class.
		
		\author	dcofer
		\date	3/16/2011
		**/
		class ANIMAT_PORT ExternalStimulus : public ActivatedItem 
		{   
		public:
			ExternalStimulus();
			virtual ~ExternalStimulus();

			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

			//ActiveItem overrides
			virtual bool operator<(ActivatedItem *lpItem);
		};

	}			//ExternalStimuli
}				//AnimatSim
