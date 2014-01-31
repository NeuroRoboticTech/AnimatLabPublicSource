// RbMaterialType.h: interface for the RbMaterialType class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{

		class ROBOTICS_PORT RbMaterialType : public AnimatSim::Environment::MaterialType
		{
		protected:
			/// All the rigid bodies associated with this material type.
			CStdMap<std::string, RigidBody *> m_aryBodies;

			/// The primary linear coefficient of friction parameter.
			float m_fltFrictionLinearPrimary;
			
			/// The angular primary coefficient of friction parameter.(this simulates rolling resistance)
			float m_fltFrictionAngularPrimary;
			
			/// The restitution of the collision between those two materials.
			float m_fltRestitution;

			virtual void SetMaterialProperties();
			virtual void RegisterMaterialType();

		public:
			RbMaterialType();
			virtual ~RbMaterialType();

			virtual CStdMap<std::string, RigidBody *> *Bodies() {return &m_aryBodies;};

			virtual float FrictionLinearPrimary();
			virtual void FrictionLinearPrimary(float fltVal);

			virtual float FrictionAngularPrimary();
			virtual void FrictionAngularPrimary(float fltVal);
            virtual float FrictionAngularPrimaryConverted();

			virtual float Restitution();
			virtual void Restitution(float fltVal);

			virtual int GetMaterialID(std::string strName);
			virtual void Initialize();

			virtual void AddRigidBodyAssociation(RigidBody *lpBody);
			virtual void RemoveRigidBodyAssociation(RigidBody *lpBody);
			virtual RigidBody *FindBodyByID(std::string strID, bool bThrowError = true);

			virtual void CreateDefaultUnits();
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

			virtual void Load(CStdXml &oXml);
		};

	}			// Visualization
}				//RoboticsAnimatSim
