// [*MUSCLE_NAME*].h: interface for the [*MUSCLE_NAME*] class.
//
//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000
#pragma once
#endif 

namespace Test
{
	namespace Bodies
	{

class TEST_PORT [*MUSCLE_NAME*] : public VortexAnimatLibrary::Environment::Bodies::VsMuscleBase
{
protected:

	virtual void CalculateTension(Simulator *lpSim);

public:
	[*MUSCLE_NAME*]();
	virtual ~[*MUSCLE_NAME*]();

	virtual void CalculateInverseDynamics(Simulator *lpSim, float fltLength, float fltVelocity, float fltT, float &fltVm, float &fltA);
	virtual float *GetDataPointer(string strDataType);
	virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
};

	}		//Bodies
}				//Test
