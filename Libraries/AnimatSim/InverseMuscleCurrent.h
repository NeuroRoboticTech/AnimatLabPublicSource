// InverseMuscleCurrent.h: interface for the CurrentStimulus class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace AnimatSim
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT InverseMuscleCurrent  : public ExternalStimulus
		{
		protected:
			string m_strTargetNodeID;
			Node *m_lpTargetNode; 

			float *m_lpExternalCurrent;

			string m_strMuscleID;
			LinearHillMuscle *m_lpMuscle;

			float m_fltCurrent;
			float m_fltPrevCurrent;
			float m_fltOffset;
			
			int m_iIndex;
			float m_fltLength;
			float m_fltVelocity;

			float m_fltT;
			float m_fltA;
			float m_fltVm;

			float m_fltRestPotential;
			float m_fltConductance;

			string m_strMuscleLengthData;
			CStdArray<float> m_aryTime;
			CStdArray<float> m_aryLength;
			CStdArray<float> m_aryVelocity;

			void LoadMuscleData(string strFilename);

		public:
			InverseMuscleCurrent();
			virtual ~InverseMuscleCurrent();

			virtual void RestPotential(float fltV);
			virtual float RestPotential();

			virtual void Conductance(float fltG);
			virtual float Conductance();

			virtual void TargetNodeID(string strID);
			virtual string TargetNodeID();
			virtual Node *TargetNode();

			virtual void MuscleID(string strID);
			virtual string MuscleID();
			virtual LinearHillMuscle *Muscle();

			virtual void MuscleLengthData(string strFilename);
			virtual string MuscleLengthData();

			virtual void Load(CStdXml &oXml);

			virtual float *GetDataPointer(const string &strDataType);
			virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

			//ActiveItem overrides
			virtual string Type() {return "InverseMuscleCurrent";};
			virtual void Initialize();  
			virtual void Activate();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();
		};

	}			//ExternalStimuli
}				//AnimatSim
