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

			osg::Geometry *CreateLineGeometry();

			virtual void SetupGraphics();

			//Remove the texture and culling options for the line.
			virtual void SetTexture(string strTexture) {};
			virtual void SetCulling() {};

		public:
			VsLine();
			virtual ~VsLine();

			virtual void Initialize() {};
			virtual void CalculateForceVector(Attachment *lpPrim, Attachment *lpSec, float fltTension, CStdFPoint &oPrimPos, CStdFPoint &oSecPos, CStdFPoint &oPrimForce);
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void StepSimulation(float fltTension);
			virtual void CreateParts();
			virtual void DrawLine();
		};

	}			// Visualization
}				//VortexAnimatSim
