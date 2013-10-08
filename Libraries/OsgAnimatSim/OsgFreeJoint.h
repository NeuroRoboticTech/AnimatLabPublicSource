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
			    virtual void DeletePhysics(bool bIncludeChildren);

		    public:
			    OsgFreeJoint();
			    virtual ~OsgFreeJoint();

    #pragma region DataAccesMethods

			    virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			    virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);
                virtual bool Physics_IsDefined() {return m_bPhsyicsDefined;};

    #pragma endregion

			    virtual void CreateJoint();
		    };

    	}			// Joints
	}			// Environment
}				//VortexAnimatSim
