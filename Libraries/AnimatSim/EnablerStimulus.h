/**
\file	EnablerStimulus.h

\brief	Declares the enabler stimulus class. 
**/

#pragma once

namespace AnimatSim
{
	namespace ExternalStimuli
	{
		/**
		\brief	This stimulus enables or disables a joint or body part for a specified period of time. 
		
		\author	dcofer
		\date	3/17/2011
		**/
		class ANIMAT_PORT EnablerStimulus  : public ExternalStimulus
		{
		protected:
			/// GUID ID of the target node to enable.
			string m_strTargetNodeID;

			/// Tells whether the node is enabled while stimulus is active or not.
			BOOL m_bEnableWhenActive;

		public:
			EnablerStimulus();
			virtual ~EnablerStimulus();
			
			virtual string Type();

			virtual string TargetNodeID();
			virtual void TargetNodeID(string strID);

			virtual BOOL EnableWhenActive();
			virtual void EnableWhenActive(BOOL bVal);

			virtual void Initialize();
			virtual void Activate();
			virtual void StepSimulation();
			virtual void Deactivate();
			virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

			virtual void Load(CStdXml &oXml);

		};

	}			//ExternalStimuli
}				//VortexAnimatSim
