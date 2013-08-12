/**
\file	LinearJoint.h

\brief	Declares the linear 1-D, 2-D, and 3-D class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			/**
			\brief	A Linear movement type of joint.
			
			\details

			\author	dcofer
			\date	3/24/2011
			**/
			class ANIMAT_PORT LinearJoint : public Joint    
			{
			protected:
				int m_iLinearType;

			public:
				LinearJoint();
				virtual ~LinearJoint();

				virtual void LinearType(string strType);
				virtual void LinearType(int iType);
				virtual int LinearType();

				virtual float PlaneWidth();
				virtual float PlaneSize();

				virtual float CylinderRadius();
				virtual float CylinderHeight();

#pragma region DataAccesMethods

				virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
