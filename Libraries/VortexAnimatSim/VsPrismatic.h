/**
\file	VsPrismatic.h

\brief	Declares the vs prismatic class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class VORTEX_PORT VsPrismatic : public VsMotorizedJoint, public AnimatSim::Environment::Joints::Prismatic     
			{
			protected:
				Vx::VxPrismatic *m_vxPrismatic;

				//Graphics objects for the hinge drawing code
				osg::ref_ptr<osg::Geometry> m_osgCylinder;
				osg::ref_ptr<osg::MatrixTransform> m_osgCylinderMT;
				osg::ref_ptr<osg::Material> m_osgCylinderMat;
				osg::ref_ptr<osg::StateSet> m_osgCylinderSS;

				osg::ref_ptr<osg::MatrixTransform> m_osgPrismaticMT;

				virtual void SetupPhysics();
				virtual void DeletePhysics();
				virtual void CreateCylinderGraphics();
				virtual void ResetGraphicsAndPhysics();

			public:
				VsPrismatic();
				virtual ~VsPrismatic();

				virtual void Rotation(CStdFPoint &oPoint, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
				virtual void JointPosition(float fltPos);

				virtual void SetupGraphics();
				virtual void SetAlpha();

#pragma region DataAccesMethods

				virtual float *GetDataPointer(string strDataType);
				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma endregion

				virtual void EnableLimits(BOOL bVal);
				virtual void CreateJoint();
				virtual void StepSimulation();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
