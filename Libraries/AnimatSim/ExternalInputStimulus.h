/**
\file	ExternalInputStimulus.h

\brief	Declares the external input stimulus class. 
**/

#pragma once

namespace AnimatSim
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT ExternalInputStimulus  : public ExternalStimulus
		{
		protected:
			/// GUID ID of the target node to enable.
			string m_strTargetNodeID;

			/// The post-fix equation that controls the input values to the node.
			string m_strInputEquation;

			/// Pointer to the post-fix evaluator.
			CStdPostFixEval *m_lpEval;

			/// The current input value that will be applied to the node.
			float m_fltInput;

		public:
			ExternalInputStimulus();
			virtual ~ExternalInputStimulus();
			
			virtual string Type();

			virtual string TargetNodeID();
			virtual void TargetNodeID(string strID);
			
			virtual float Input();
			virtual void Input(float fltVal);

			virtual string InputEquation();
			virtual void InputEquation(string strVal);

			virtual void Load(CStdXml &oXml);

			virtual void Activate();
			virtual void Initialize();
			virtual void StepSimulation();
			virtual void Deactivate();

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim
