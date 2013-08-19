/**
\file	OsgPrismatic.cpp

\brief	Implements the vs prismatic class.
**/

#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgPrismaticLimit.h"
#include "OsgPrismatic.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"
#include "OsgDragger.h"

namespace OsgAnimatSim
{
	namespace Environment
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
OsgPrismatic::OsgPrismatic()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
OsgPrismatic::~OsgPrismatic()
{
}


void OsgPrismatic::DeletePrismaticGraphics(osg::ref_ptr<osg::MatrixTransform> osgJointMT, OsgPrismaticLimit *lpUpperLimit, OsgPrismaticLimit *lpLowerLimit, OsgPrismaticLimit *lpPosFlap)
{
    if(osgJointMT.valid())
    {
		if(lpUpperLimit && lpUpperLimit->BoxMT()) osgJointMT->removeChild(lpUpperLimit->BoxMT());
		if(lpUpperLimit && lpUpperLimit->CylinderMT()) osgJointMT->removeChild(lpUpperLimit->CylinderMT());

        if(lpLowerLimit && lpLowerLimit->BoxMT()) osgJointMT->removeChild(lpLowerLimit->BoxMT());
		if(lpLowerLimit && lpLowerLimit->CylinderMT()) osgJointMT->removeChild(lpLowerLimit->CylinderMT());

		if(lpPosFlap && lpPosFlap->BoxMT()) osgJointMT->removeChild(lpPosFlap->BoxMT());
    }
}

void OsgPrismatic::CreatePrismaticGraphics(float fltBoxSize, float fltRadius, float fltLimitPos, 
                                           bool bIsShowPosition, CStdColor vColor,
                                           osg::ref_ptr<osg::MatrixTransform> osgJointMT, OsgPrismaticLimit *lpUpperLimit, 
                                           OsgPrismaticLimit *lpLowerLimit, OsgPrismaticLimit *lpPosFlap)
{
	lpUpperLimit->SetupLimitGraphics(fltBoxSize, fltRadius, fltLimitPos, bIsShowPosition, vColor);
	lpLowerLimit->SetupLimitGraphics(fltBoxSize, fltRadius, fltLimitPos, bIsShowPosition, vColor);
	lpPosFlap->SetupLimitGraphics(fltBoxSize, fltRadius, fltLimitPos, bIsShowPosition, vColor);

	osgJointMT->addChild(lpUpperLimit->BoxMT());
	osgJointMT->addChild(lpUpperLimit->CylinderMT());

	osgJointMT->addChild(lpLowerLimit->BoxMT());
	osgJointMT->addChild(lpLowerLimit->CylinderMT());

	osgJointMT->addChild(lpPosFlap->BoxMT());
}

	}			// Environment
}				//OsgAnimatSim
