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
			std::string m_strTargetNodeID;

			/// Tells whether the node is enabled while stimulus is active or not.
			bool m_bEnableWhenActive;

		public:
			EnablerStimulus();
			virtual ~EnablerStimulus();
			
			static EnablerStimulus *CastToDerived(AnimatBase *lpBase) {return static_cast<EnablerStimulus*>(lpBase);}
			
			virtual std::string Type();

			virtual std::string TargetNodeID();
			virtual void TargetNodeID(std::string strID);

			virtual bool EnableWhenActive();
			virtual void EnableWhenActive(bool bVal);

			virtual void Initialize();
			virtual void Activate();
			virtual void StepSimulation();
			virtual void Deactivate();
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void Load(CStdXml &oXml);

		};

	}			//ExternalStimuli
}				//VortexAnimatSim
