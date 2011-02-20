// ClassFactory.h: interface for the ClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000
#pragma once
#endif 

namespace [*PROJECT_NAME*]
{

	class ClassFactory : public IStdClassFactory
	{
	public:
		ClassFactory();
		virtual ~ClassFactory();

		virtual RigidBody *CreateRigidBody(string strType, BOOL bThrowError = TRUE);
		virtual Joint *CreateJoint(string strType, BOOL bThrowError = TRUE);
		virtual ExternalStimulus *CreateExternalStimulus(string strType, BOOL bThrowError = TRUE);

		virtual CStdSerialize *CreateObject(string strClassType, string strObjectType, BOOL bThrowError = TRUE);
	};

}			//[*PROJECT_NAME*]
