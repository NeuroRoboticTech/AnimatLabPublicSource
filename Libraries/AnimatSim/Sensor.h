// AlSensor.h: interface for the CAlSensor class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALSENSOR_H__41341DA4_DDA9_4B00_9198_27B628231EE2__INCLUDED_)
#define AFX_ALSENSOR_H__41341DA4_DDA9_4B00_9198_27B628231EE2__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

				class ANIMAT_PORT Sensor : public RigidBody   
				{
				protected:
					float m_fltRadius;

				public:
					Sensor();
					virtual ~Sensor();

					virtual void CreateParts();
					virtual void CreateJoints();

					virtual void Load(CStdXml &oXml);
				};

		}		//Bodies
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALSENSOR_H__41341DA4_DDA9_4B00_9198_27B628231EE2__INCLUDED_)
