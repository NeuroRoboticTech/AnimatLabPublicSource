/**
\file	OdorSensor.h

\brief	Declares the odor sensor class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{
			/**
			\brief	Odor sensor part type. 
			
			\detials This part is added to a rigid body and detects odorants that are emitted from odor sources
			in the environment. You can specify the Odor type that this sensor will detect.  

			\author	dcofer
			\date	3/10/2011
			**/
			class ANIMAT_PORT OdorSensor : public Sensor  
			{
			protected:
				/// The odor concentration value
				float m_fltOdorValue;

				/// Pointer to the type odor to detect
				OdorType *m_lpOdorType;

				/// Identifier for the odor type
				string m_strOdorTypeID;

				virtual void SetOdorTypePointer(string strID);

			public:
				OdorSensor();
				virtual ~OdorSensor();

				virtual void OdorTypeID(string strID);
				virtual string OdorTypeID();

				virtual void ResetSimulation();
				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual float *GetDataPointer(string strDataType);
				virtual void StepSimulation();
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
