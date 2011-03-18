/**
\file	ExternalStimuliMgr.h

\brief	Declares the external stimuli manager class. 
**/

#pragma once

namespace AnimatSim
{
	namespace ExternalStimuli
	{
		/**
		\brief	This class is derived from the ActivatedItemMgr and it handles all of the external stimuli. 
		
		\author	dcofer
		\date	3/17/2011
		**/
		class ANIMAT_PORT ExternalStimuliMgr : public ActivatedItemMgr    
		{
		protected:

			ExternalStimulus *LoadExternalStimuli(CStdXml &oXml);

		public:
			ExternalStimuliMgr();
			virtual ~ExternalStimuliMgr();

			virtual BOOL AddStimulus(string strXml);
			virtual BOOL RemoveStimulus(string strID);

			//virtual void Load(string strProjectPath, string strFileName);
			virtual void Load(CStdXml &oXml);
		};

	}			//ExternalStimuli
}				//AnimatSim
