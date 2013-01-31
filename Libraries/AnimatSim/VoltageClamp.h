/**
\file	VoltageClamp.h

\brief	Declares the voltage clamp class. 
**/
#pragma once


namespace AnimatSim
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT VoltageClamp  : public ExternalStimulus
		{
		protected:
			/// GUID ID of the node that will be clamped.
			string m_strTargetNodeID;

			/// Pointer to the external current data variable within the node to clamp.
			/// We will be adding our reverse currents here to clamp the voltage to a given level.
			float *m_lpExternalCurrent;

			/// The total current data variable within the node that is clamped
			float *m_lpTotalCurrent;

			/// The resting voltage data variable within the node to clamp.
			float *m_lpVrest;

			/// The conductance data variable within the node to clamp
			float *m_lpGm;

			/// The target voltage for the clamp.
			float m_fltVtarget;

			/// The target current for the clamp.
			float m_fltTargetCurrent;

			/// The active current is the target current - total current in the cell.
			float m_fltActiveCurrent;

		public:
			VoltageClamp();
			virtual ~VoltageClamp();

			virtual string Type();
			
			virtual string TargetNodeID();
			virtual void TargetNodeID(string strID);

			virtual float Vtarget();
			virtual void Vtarget(float fltVal);

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual void Initialize();  
			virtual void Activate();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim
