/**
\file	OsgFreeJoint.h

\brief	Declares the vs universal class.
**/

#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

		    class ANIMAT_OSG_PORT OsgFreeJoint : public OsgJoint, public BallSocket     
		    {
		    protected:
                bool m_bPhsyicsDefined;

			    virtual void SetupPhysics();
			    virtual void DeletePhysics();

		    public:
			    OsgFreeJoint();
			    virtual ~OsgFreeJoint();

    #pragma region DataAccesMethods

			    virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			    virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
                virtual bool Physics_IsDefined() {return m_bPhsyicsDefined;};

    #pragma endregion

			    virtual void CreateJoint();
		    };

    	}			// Joints
	}			// Environment
}				//VortexAnimatSim
