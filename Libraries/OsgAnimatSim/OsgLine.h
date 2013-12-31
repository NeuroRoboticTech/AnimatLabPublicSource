// OsgLine.h: interface for the OsgLine class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{

		class ANIMAT_OSG_PORT OsgLine
		{
		protected:
            osg::Geometry* m_lpLinesGeom;
			LineBase *m_lpLineBase;
			osg::ref_ptr<osg::Vec3Array> m_aryLines;
			float fltA;

			osg::Geometry *CreateLineGeometry();

            virtual void SetThisLinePointers();
			virtual int BuildLines(osg::Geometry *linesGeom);

            virtual CStdFPoint GetOffsetFromParentCOM(RigidBody *lpParent, const CStdFPoint &vPos);

		public:
			OsgLine();
			virtual ~OsgLine();

			virtual void CalculateForceVector(Attachment *lpPrim, Attachment *lpSec, float fltTension, CStdFPoint &oPrimPos, CStdFPoint &oSecPos, CStdFPoint &oPrimForce);
			virtual void StepLineSimulation(bool bEnabled, float fltTension);
			virtual void DrawLine();
		};

	}			// Visualization
}				//OsgAnimatSim
