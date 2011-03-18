
#pragma once

namespace AnimatSim
{
	namespace Environment
	{

		class ANIMAT_PORT ConstraintLimit : public AnimatBase
		{
		protected:
			Joint *m_lpJoint;

			float m_fltLimitPos;
			float m_fltDamping;
			float m_fltRestitution;
			float m_fltStiffness;
			BOOL m_bIsLowerLimit;

			CStdColor m_vColor;

		public:
			//This is a test comment.
			ConstraintLimit();
			virtual ~ConstraintLimit();

			virtual float LimitPos();
			virtual void LimitPos(float fltVal, BOOL bUseScaling = TRUE, BOOL bOverrideSameCheck = FALSE);

			virtual float Damping();
			virtual void Damping(float fltVal, BOOL bUseScaling = TRUE);

			virtual float Restitution();
			virtual void Restitution(float fltVal);

			virtual float Stiffness();
			virtual void Stiffness(float fltVal, BOOL bUseScaling = TRUE);

			virtual void Color(float fltR, float fltG, float fltB, float fltA);
			virtual CStdColor *Color();
			virtual void Color(string strXml);

			virtual void Alpha(float fltA) = 0;
			virtual float Alpha();

			virtual void IsLowerLimit(BOOL bVal) {m_bIsLowerLimit = bVal;};
			virtual BOOL IsLowerLimit() {return m_bIsLowerLimit;};

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, float fltPosition, BOOL bVerify = TRUE);
			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify);
			virtual void VerifySystemPointers();
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual void Load(CStdXml &oXml, string strName);
			virtual void SetupGraphics() = 0;
		};

	}			// Environment
}				//AnimatSim
