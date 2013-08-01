/**
\file	PropertyControlStimulus.h

\brief	Declares a stimulus class that can set any property on any object in the system. 
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
		class ANIMAT_PORT PropertyControlStimulus  : public ExternalStimulus
		{
		protected:
			/// GUID ID of the target node to enable.
			string m_strTargetID;

			/// Pointer to the target node
			AnimatBase *m_lpTargetObject;

			float m_fltPreviousSetVal;
			float m_fltSetThreshold;
			float m_fltInitialValue;
			float m_fltFinalValue;

			string m_strEquation;
			CStdPostFixEval *m_lpEval;

			string m_strPropertyName;
			AnimatBase::AnimatPropertyType m_ePropertyType;

			virtual void SetPropertyValue(float fltVal);

		public:
			PropertyControlStimulus();
			virtual ~PropertyControlStimulus();
			
			virtual string Type();

			virtual string TargetID();
			virtual void TargetID(string strID);
						
			virtual AnimatBase *TargetObject();

			virtual void PropertyName(string strPropName);
			virtual string PropertyName();

			virtual void SetThreshold(float fltThreshold);
			virtual float SetThreshold();

			virtual void InitialValue(float fltVal);
			virtual float InitialValue();

			virtual void FinalValue(float fltVal);
			virtual float FinalValue();
			
			string Equation() {return m_strEquation;};
			void Equation(string strVal);

			virtual void Initialize();
			virtual void Activate();
			virtual void StepSimulation();
			virtual void Deactivate();
			virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

			virtual void Load(CStdXml &oXml);

		};

	}			//ExternalStimuli
}				//VortexAnimatSim
