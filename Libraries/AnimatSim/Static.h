// AlStaticJoint.h: interface for the Static class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALSTATICJOINT_H__EE83858A_9B8D_4823_A346_4EC0197D4A1A__INCLUDED_)
#define AFX_ALSTATICJOINT_H__EE83858A_9B8D_4823_A346_4EC0197D4A1A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			/*! \brief 
				A static type of joint that can not move.

				\remarks
				This type of joint is constrained so that it can not move at all
				relative to its parent. It can not rotate or translate. In essence
				when you use this joint you are making these two rigid bodies behave
				as if they were one part. When the parent part moves the same motion
				will be applied to the child part.

				\sa
				Joint, Hinge, Static
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT Static : public Joint  
			{
			public:
				Static();
				virtual ~Static();

				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALSTATICJOINT_H__EE83858A_9B8D_4823_A346_4EC0197D4A1A__INCLUDED_)
