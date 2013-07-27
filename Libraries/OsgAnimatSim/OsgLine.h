// OsgLine.h: interface for the OsgLine class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{

		class ANIMAT_OSG_PORT OsgLine : public VsRigidBody
		{
		protected:
			LineBase *m_lpLineBase;
			osg::ref_ptr<osg::Vec3Array> m_aryLines;
			float fltA;

			osg::Geometry *CreateLineGeometry();

			virtual void SetupGraphics();

			//Remove the texture and culling options for the line.
			virtual void SetTexture(string strTexture) {};
			virtual void SetCulling() {};
			virtual void CreateGraphicsGeometry();
			virtual void CreatePhysicsGeometry();
			virtual int BuildLines(osg::Geometry *linesGeom);
			virtual void SetThisPointers();
			virtual void DeleteGraphics();

		public:
			OsgLine();
			virtual ~OsgLine();

			virtual void Initialize() {};
			virtual void CalculateForceVector(Attachment *lpPrim, Attachment *lpSec, float fltTension, CStdFPoint &oPrimPos, CStdFPoint &oSecPos, CStdFPoint &oPrimForce);
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void StepSimulation(float fltTension);
			virtual void CreateParts();
			virtual void DrawLine();
		};

	}			// Visualization
}				//OsgAnimatSim
