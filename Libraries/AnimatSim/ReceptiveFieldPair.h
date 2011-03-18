// ReceptiveFieldPair.h: interface for the ReceptiveFieldPair class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALRECEPTIVE_FIELD_PAIR_H__8AABAE57_5434_4AEE_9C0B_B494E10A7AAC__INCLUDED_)
#define AFX_ALRECEPTIVE_FIELD_PAIR_H__8AABAE57_5434_4AEE_9C0B_B494E10A7AAC__INCLUDED_

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

		class ANIMAT_PORT ReceptiveFieldPair : public AnimatBase 
		{
		public:
			StdVector3 m_vVertex;
			Node *m_lpTargetNode;
			string m_strTargetNodeID;
			ReceptiveField *m_lpField;

			ReceptiveFieldPair();
			virtual ~ReceptiveFieldPair();

			void Initialize();
			void StepSimulation();

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALRECEPTIVE_FIELD_PAIR_H__8AABAE57_5434_4AEE_9C0B_B494E10A7AAC__INCLUDED_)
