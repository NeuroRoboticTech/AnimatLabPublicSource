/**
\file	OsgPrismatic.h

\brief	Declares the vs prismatic class.
**/

#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class ANIMAT_OSG_PORT OsgPrismatic
			{
			protected:
                virtual void DeletePrismaticGraphics(osg::ref_ptr<osg::MatrixTransform> osgJointMT, OsgPrismaticLimit *lpUpperLimit, OsgPrismaticLimit *lpLowerLimit, OsgPrismaticLimit *lpPosFlap);
                virtual void CreatePrismaticGraphics(float fltBoxSize, float fltRadius, float fltLimitPos, 
                                                     bool bIsShowPosition, CStdColor vColor,
                                                     osg::ref_ptr<osg::MatrixTransform> osgJointMT, OsgPrismaticLimit *lpUpperLimit, 
                                                     OsgPrismaticLimit *lpLowerLimit, OsgPrismaticLimit *lpPosFlap);

			public:
				OsgPrismatic();
				virtual ~OsgPrismatic();
			};

		}		//Joints
	}			// Environment
}				//OsgAnimatSim
