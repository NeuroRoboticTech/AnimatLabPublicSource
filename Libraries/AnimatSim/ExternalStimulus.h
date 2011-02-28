
#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::ExternalStimuli

	\brief	Contains all of the classes that are used to generate external stimuli to body parts, joints, or neural items. 
	**/
	namespace ExternalStimuli
	{

		class ANIMAT_PORT ExternalStimulus : public ActivatedItem 
		{   
		public:
			ExternalStimulus();
			virtual ~ExternalStimulus();

			float m_fltInput;

			virtual float *GetDataPointer(string strDataType) {return NULL;};
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

			//ActiveItem overrides
			virtual void Load(Simulator *lpSim, CStdXml &oXml) = 0;
			virtual void Save(Simulator *lpSim, CStdXml &oXml) = 0;
			virtual BOOL operator<(ActivatedItem *lpItem);
		};

	}			//ExternalStimuli
}				//AnimatSim
