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
			std::string m_strTargetNodeID;

			/// The post-fix equation that controls the input values to the node.
			std::string m_strInputEquation;

			/// Pointer to the post-fix evaluator.
			CStdPostFixEval *m_lpEval;

			/// The current input value that will be applied to the node.
			float m_fltInput;

		public:
			ExternalInputStimulus();
			virtual ~ExternalInputStimulus();
			
			static ExternalInputStimulus *CastToDerived(AnimatBase *lpBase) {return static_cast<ExternalInputStimulus*>(lpBase);}
			
			virtual std::string Type();

			virtual std::string TargetNodeID();
			virtual void TargetNodeID(std::string strID);
			
			virtual float Input();
			virtual void Input(float fltVal);

			virtual std::string InputEquation();
			virtual void InputEquation(std::string strVal);

			virtual void Load(CStdXml &oXml);

			virtual void Activate();
			virtual void Initialize();
			virtual void StepSimulation();
			virtual void Deactivate();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim
