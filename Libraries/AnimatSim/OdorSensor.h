// OdorSensor.h: interface for the OdorSensor class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALODOR_SENSOR_H__EBC36518_3B2A_4634_9AB6_474F65149FCF__INCLUDED_)
#define AFX_ALODOR_SENSOR_H__EBC36518_3B2A_4634_9AB6_474F65149FCF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			/*! \brief 
				Specifies a point on a rigid body where a muscle is to be attached.
			   
				\remarks
				This type of part specifies the position of a muscle attachment point.
				All CAlMuscle objects must specify the ID for two of these types of 
				object to determine where the ends of the muscle connect to the 
				parent and child rigid bodies. 

				\sa
				Body, Joint, CAlBox, CAlPlane, CAlCylinder, 
				CAlCone, CAlMuscle, CAlOdorSensor, CAlSphere                                
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT OdorSensor : public Sensor  
			{
			protected:
				float m_fltOdorValue;
				OdorType *m_lpOdorType;

			public:
				OdorSensor();
				virtual ~OdorSensor();

				virtual float *GetDataPointer(string strDataType);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALODOR_SENSOR_H__EBC36518_3B2A_4634_9AB6_474F65149FCF__INCLUDED_)
