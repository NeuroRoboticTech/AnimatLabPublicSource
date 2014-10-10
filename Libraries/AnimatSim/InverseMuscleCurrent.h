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
			std::string m_strTargetNodeID;
			Node *m_lpTargetNode; 

			float *m_lpExternalCurrent;

			std::string m_strMuscleID;
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

			std::string m_strMuscleLengthData;
			CStdArray<float> m_aryTime;
			CStdArray<float> m_aryLength;
			CStdArray<float> m_aryVelocity;

			void LoadMuscleData(std::string strFilename);

		public:
			InverseMuscleCurrent();
			virtual ~InverseMuscleCurrent();
			
			static InverseMuscleCurrent *CastToDerived(AnimatBase *lpBase) {return static_cast<InverseMuscleCurrent*>(lpBase);}

			virtual void RestPotential(float fltV);
			virtual float RestPotential();

			virtual void Conductance(float fltG);
			virtual float Conductance();

			virtual void TargetNodeID(std::string strID);
			virtual std::string TargetNodeID();
			virtual Node *TargetNode();

			virtual void MuscleID(std::string strID);
			virtual std::string MuscleID();
			virtual LinearHillMuscle *Muscle();

			virtual void MuscleLengthData(std::string strFilename);
			virtual std::string MuscleLengthData();

			virtual void Load(CStdXml &oXml);

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			//ActiveItem overrides
			virtual std::string Type() {return "InverseMuscleCurrent";};
			virtual void Initialize();  
			virtual void Activate();
			virtual void ResetSimulation();  
			virtual void StepSimulation();
			virtual void Deactivate();
		};

	}			//ExternalStimuli
}				//AnimatSim
