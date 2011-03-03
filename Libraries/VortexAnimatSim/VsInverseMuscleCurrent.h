// CurrentStimulus.h: interface for the CurrentStimulus class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_INVERSE_MUSCLE_STIM_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
#define AFX_INVERSE_MUSCLE_STIM_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace VortexAnimatSim
{
	namespace ExternalStimuli
	{

		class VORTEX_PORT VsInverseMuscleCurrent  : public AnimatSim::ExternalStimuli::ExternalStimulus
		{
		protected:
			Organism *m_lpOrganism;
			string m_strOrganismID;

			string m_strNeuralModule;
			string m_strTargetNodeID;

			Node *m_lpNode;
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
			VsInverseMuscleCurrent();
			virtual ~VsInverseMuscleCurrent();

			virtual void Load(CStdXml &oXml);

			virtual float *GetDataPointer(string strDataType);

			//ActiveItem overrides
			virtual string Type() {return "InverseMuscleCurrent";};
			virtual void Initialize();  
			virtual void Activate();
			virtual void StepSimulation();
			virtual void Deactivate();

			//virtual void Modify(Simulator *lpSim, ActivatedItem *lpItem);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim

#endif // !defined(AFX_INVERSE_MUSCLE_STIM_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
