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
			float fltA;

			osg::Geometry *CreateLineGeometry();

			virtual void SetupGraphics();

			//Remove the texture and culling options for the line.
			virtual void SetTexture(std::string strTexture) {};
			virtual void SetCulling() {};
			virtual void CreateGraphicsGeometry();
			virtual void CreatePhysicsGeometry();
			virtual int BuildLines(osg::Geometry *linesGeom);
			virtual void SetThisPointers();
			virtual void DeleteGraphics();

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
