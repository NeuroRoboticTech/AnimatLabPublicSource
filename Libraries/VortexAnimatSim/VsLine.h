// VsLine.h: interface for the VsLine class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{

		class VORTEX_PORT VsLine : public VsRigidBody 
		{
		protected:
			LineBase *m_lpLineBase;
			osg::ref_ptr<osg::Vec3Array> m_aryLines;
			osg::ref_ptr<osg::Geometry> m_osgGeometry; 
			float fltA;

			osg::Geometry *CreateLineGeometry(Simulator *lpSim, Structure *lpStructure);

			virtual void SetupGraphics(Simulator *lpSim, Structure *lpStructure);

			//Remove the texture and culling options for the line.
			virtual void SetTexture(string strTexture) {};
			virtual void SetCulling() {};

		public:
			VsLine();
			virtual ~VsLine();

			virtual void Initialize(Simulator *lpSim, Structure *lpStructure) {};
			virtual void CalculateForceVector(Simulator *lpSim,Attachment *lpPrim, Attachment *lpSec, float fltTension, CStdFPoint &oPrimPos, CStdFPoint &oSecPos, CStdFPoint &oPrimForce);
			virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure);
			virtual void AfterResetSimulation(Simulator *lpSim, Structure *lpStructure);
			virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure, float fltTension);
			virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
			virtual void DrawLine(Simulator *lpSim, Structure *lpStructure);
		};

	}			// Visualization
}				//VortexAnimatSim
