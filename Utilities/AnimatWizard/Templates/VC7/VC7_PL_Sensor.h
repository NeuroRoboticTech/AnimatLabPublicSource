// [*BODY_PART_NAME*].h: interface for the VsOdorSensor class.
//
//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000
#pragma once
#endif 

namespace [*PROJECT_NAME*]
{
	namespace Bodies
	{

class [*TAG_NAME*]_PORT [*BODY_PART_NAME*] : public VortexAnimatLibrary::Environment::Bodies::VsSensor
{
protected:

public:
	[*BODY_PART_NAME*]();
	virtual ~[*BODY_PART_NAME*]();

	virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
	virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
};

	}		//Bodies
}				//[*PROJECT_NAME*]
