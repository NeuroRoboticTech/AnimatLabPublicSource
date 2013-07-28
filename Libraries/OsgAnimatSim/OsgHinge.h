/**
\file	OsgHinge.h

\brief	Declares the vortex hinge class.
**/

#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{

		/**
		\namespace	OsgAnimatSim::Environment::Joints

		\brief	Joint classes that use the vortex physics engine. 
		**/
		namespace Joints
		{
			/**
			\brief	Vortex hinge joint class.

			\details This class implements a hinge joint. This type of joint
			prevents linear motion for all three dimensions and it prevents angular
			motion for two axises. Allowing the two connected bodies to rotate about
			one axis freely. You can define constraint limits to prevent the motion
			beyond certain angular limits. This type of joint is also motorized and thus
			implements the IMotorized interface using the VsMotorized class. This allows
			the user to control the movement of this joint as if it were a servo motor or
			a velocity controlled motor.
			
			\author	dcofer
			\date	4/15/2011
			**/
			class ANIMAT_OSG_PORT OsgHinge : public OsgJoint, AnimatSim::Environment::Joints::Hinge     
			{
			protected:
				/// The rotation of the hinge in degrees.
				float m_fltRotationDeg;

				//Graphics objects for the hinge drawing code
				/// The osg cylinder geometry.
				osg::ref_ptr<osg::Geometry> m_osgCylinder;

                /// The osg cylinder geode.
                osg::ref_ptr<osg::Geode> m_osgCylinderGeode;

				/// The osg cylinder matrix transform.
				osg::ref_ptr<osg::MatrixTransform> m_osgCylinderMT;

				/// The osg cylinder material.
				osg::ref_ptr<osg::Material> m_osgCylinderMat;

				/// The osg cylinder state set.
				osg::ref_ptr<osg::StateSet> m_osgCylinderSS;

    			virtual void DeleteJointGraphics();
                virtual void CreateJointGraphics();
				virtual void SetupGraphics();
				virtual void UpdateData();
				virtual void CreateCylinderGraphics();

			public:
				OsgHinge();
				virtual ~OsgHinge();

				virtual void SetAlpha();

#pragma region DataAccesMethods

				virtual float *GetDataPointer(const string &strDataType);
				virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
				virtual void StepSimulation();
			};

		}		//Joints
	}			// Environment
}				//OsgAnimatSim
