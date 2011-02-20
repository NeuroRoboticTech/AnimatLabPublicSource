// VsHinge.h: interface for the VsHinge class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSHINGEJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
#define AFX_VSHINGEJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class VORTEX_PORT VsHinge : public VsJoint, public Hinge     
			{
			protected:
				Vx::VxHinge *m_vxHinge;
				float m_fltRotationDeg;

				//Graphics objects for the hinge drawing code
				osg::ref_ptr<osg::Geometry> m_osgCylinder;
				osg::ref_ptr<osg::MatrixTransform> m_osgCylinderMT;
				osg::ref_ptr<osg::Geometry> m_osgMinFlap;
				osg::ref_ptr<osg::MatrixTransform> m_osgMinFlapTranslateMT;
				osg::ref_ptr<osg::MatrixTransform> m_osgMinFlapRotateMT;
				osg::ref_ptr<osg::Geometry> m_osgMaxFlap;
				osg::ref_ptr<osg::MatrixTransform> m_osgMaxFlapTranslateMT;
				osg::ref_ptr<osg::MatrixTransform> m_osgMaxFlapRotateMT;
				osg::ref_ptr<osg::Geometry> m_osgPosFlap;
				osg::ref_ptr<osg::MatrixTransform> m_osgPosFlapTranslateMT;
				osg::ref_ptr<osg::MatrixTransform> m_osgPosFlapRotateMT;
				osg::ref_ptr<osg::MatrixTransform> m_osgPosFlapReferenceMT;

				osg::ref_ptr<osg::Material> m_osgPosFlapMat;
				osg::ref_ptr<osg::StateSet> m_osgPosFlapSS;
				osg::ref_ptr<osg::Material> m_osgMinFlapMat;
				osg::ref_ptr<osg::StateSet> m_osgMinFlapSS;
				osg::ref_ptr<osg::Material> m_osgMaxFlapMat;
				osg::ref_ptr<osg::StateSet> m_osgMaxFlapSS;
				osg::ref_ptr<osg::Material> m_osgCylinderMat;
				osg::ref_ptr<osg::StateSet> m_osgCylinderSS;

				osg::ref_ptr<osg::MatrixTransform> m_osgHingeMT;

				long iVal;

				virtual void CalculateServoVelocity();
				virtual void SetVelocityToDesired();
				virtual void UpdateData(Simulator *lpSim, Structure *lpStructure);
				virtual void SetupPhysics(Simulator *lpSim, Structure *lpStructure);
				virtual void DeletePhysics();
				virtual void ResetGraphicsAndPhysics();

			public:
				VsHinge();
				virtual ~VsHinge();

				virtual void Enabled(BOOL bValue) 
				{
					EnableMotor(bValue);
					m_bEnabled = bValue;
				};

				virtual void ConstraintLow(float fltVal);
				virtual void ConstraintHigh(float fltVal);
				virtual void Rotation(CStdFPoint &oPoint);

				//virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 
				virtual void SetupGraphics(Simulator *lpSim, Structure *lpStructure);
				//virtual void BuildLocalMatrix();
				virtual void SetAlpha();
				//virtual void SetVisible(BOOL bVisible);

#pragma region DataAccesMethods

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma endregion

				virtual void EnableMotor(BOOL bVal);
				virtual void CreateJoint(Simulator *lpSim, Structure *lpStructure);
				//virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure);
				virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSHINGEJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
