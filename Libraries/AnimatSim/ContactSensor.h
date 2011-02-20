// Plane.h: interface for the Plane class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALCONTACT_SENSOR_H__8AABAE57_5434_4AEE_9C0B_B494E10A7AAC__INCLUDED_)
#define AFX_ALCONTACT_SENSOR_H__8AABAE57_5434_4AEE_9C0B_B494E10A7AAC__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{
		/*! \brief 
			Keeps track of pairs of rigid bodies that should not be allowed to collide.

			\remarks

			\sa
			Body, Joint, CAlBox, Plane, CAlCylinder, 
			CAlCone, CAlMuscle, CAlAttachment, CAlSphere                                
				
			\ingroup AnimatSim
		*/

		class ANIMAT_PORT ContactSensor : public AnimatBase  
		{
		protected:
			CStdPtrArray<ReceptiveField> m_aryFields;
			float m_fltReceptiveFieldDistance;
			Gain *m_lpFieldGain;
			Gain *m_lpCurrentGain;
			float m_fltMaxForce;

			void LoadReceptiveField(CStdXml &oXml);

			BOOL FindReceptiveField(CStdPtrArray<ReceptiveField> &aryFields, float fltX, float fltY, float fltZ, int &iIndex);
			void AddVertex(CStdPtrArray<ReceptiveField> &aryFields, float fltX, float fltY, float fltZ);

			void DumpVertices(CStdPtrArray<ReceptiveField> &aryFields);

		public:
			ContactSensor();
			virtual ~ContactSensor();

			Gain *FieldGain() {return m_lpFieldGain;};
			Gain *CurrentGain() {return m_lpCurrentGain;};
			float ReceptiveFieldDistance() {return m_fltReceptiveFieldDistance;};

			ReceptiveField *GetReceptiveField(int iIndex);
			BOOL FindReceptiveField(float fltX, float fltY, float fltZ, int &iIndex);
			int FindClosestReceptiveField(float fltX, float fltY, float fltZ);
			void AddVertex(float fltX, float fltY, float fltZ);
			void FinishedAddingVertices();

			void ClearCurrents();
			void ProcessContact(Simulator *lpSim, StdVector3 vPos, float fltForceMagnitude);
			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALCONTACT_SENSOR_H__8AABAE57_5434_4AEE_9C0B_B494E10A7AAC__INCLUDED_)
